﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;

class InGameMenuItemJournal : InGameMenu
{
	private static Viewport Viewport;
	private static SpriteFont MiddleFont = FontManager.GetFont("Arial12");
	private static SpriteFont LargerFont = FontManager.GetFont("Arial16");
	private static Texture2D JournalBackground = TextureManager.GetTexture("blackSquare");
	private static Texture2D TooltipTexture;  // Tooltip background texture

	// All the item objects that we want to show in the journal.
	private static List<ItemContainerable> Items = new List<ItemContainerable>();

	// Known status: itemName -> bool
	private static Dictionary<string, bool> KnownItems = new();

	// How many items per column (you can tweak this)
	private const int ITEMS_PER_COLUMN = 4;

	// Spacing around each item cell
	private const int CELL_SPACING_X = 20;
	private const int CELL_SPACING_Y = 20;

	// Extra margins inside the journal background
	private const int MARGIN_X = 40;
	private const int MARGIN_Y = 60; // extra top space for headline

	// Field to track which item is currently hovered
	private ItemContainerable hoveredItem = null;

	public static void Init()
	{
		AddAllItems();
		Load();
	}
	public InGameMenuItemJournal(Viewport viewport)
	{
		Viewport = viewport;
		TooltipTexture = TextureManager.GetTexture("blackSquare");
	}

	private static void AddAllItems()
	{
		var ItemsList = ItemDatabase.GetAllItems();
		KnownItems.Clear();
		Items.Clear();
		foreach (var item in ItemsList)
		{
			Items.Add(item.Clone());
		}

		// Initialize known status for each item
		foreach (var item in Items)
		{
			if (!KnownItems.ContainsKey(item.Name))
			{
				KnownItems.Add(item.Name, false); // all unknown initially
			}
		}
	}
	public static void ShowAll()
	{
		// Create a temporary list of keys to avoid modifying the collection while iterating.
		foreach (var key in new List<string>(KnownItems.Keys))
		{
			KnownItems[key] = true;
		}
	}
	public static void UpdateKnownItems(List<ItemContainer> containers)
	{
		foreach (var container in containers)
		{
			if (!container.IsEmpty())
			{
				string itemName = container.Item.Name;
				if (KnownItems.ContainsKey(itemName))
				{
					KnownItems[itemName] = true;
				}
			}
		}
		Save();
	}

	public override void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, double dt)
	{
		base.Update(viewport, player, mouseState, keyboardState, dt);

		UpdateKnownItems(player.Inventory.ItemContainers);

		// Reset hovered item before checking
		hoveredItem = null;

		// Check each item (using its drawn rectangle) to see if the mouse is over it.
		// We must compute the same layout as in Draw().
		float maxItemWidth = 0f;
		float maxItemHeight = 0f;
		foreach (var item in Items)
		{
			if (item.Size.X > maxItemWidth) maxItemWidth = item.Size.X;
			if (item.Size.Y > maxItemHeight) maxItemHeight = item.Size.Y;
		}
		int totalItems = Items.Count;
		int columns = (int)Math.Ceiling(totalItems / (float)ITEMS_PER_COLUMN);
		int cellWidth = (int)(maxItemWidth + CELL_SPACING_X);
		int cellHeight = (int)(maxItemHeight + CELL_SPACING_Y);
		int journalWidth = MARGIN_X * 2 + columns * cellWidth;
		int journalHeight = MARGIN_Y * 2 + ITEMS_PER_COLUMN * cellHeight;
		int journalX = (Viewport.Width - journalWidth) / 2;
		int journalY = (Viewport.Height - journalHeight) / 2;
		// Starting positions for items
		float startX = journalX + MARGIN_X;
		float startY = journalY + MARGIN_Y;

		// Loop through items to check for hover.
		for (int i = 0; i < Items.Count; i++)
		{
			var item = Items[i];
			int col = i / ITEMS_PER_COLUMN;
			int row = i % ITEMS_PER_COLUMN;
			float itemX = startX + col * cellWidth + (cellWidth - item.Size.X) / 2;
			float itemY = startY + row * cellHeight + (cellHeight - item.Size.Y) / 2;
			// Create a rectangle for the item based on its position and size.
			Rectangle itemRect = new Rectangle((int)itemX, (int)itemY, (int)item.Size.X, (int)item.Size.Y);

			// Only consider known items for tooltip.
			if (KnownItems[item.Name])
			{
				if (itemRect.Contains(mouseState.Position))
				{
					hoveredItem = item;
					break;
				}
			}
		}
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);

		// 1) Measure items to find the largest item width/height
		float maxItemWidth = 0f;
		float maxItemHeight = 0f;
		foreach (var item in Items)
		{
			if (item.Size.X > maxItemWidth) maxItemWidth = item.Size.X;
			if (item.Size.Y > maxItemHeight) maxItemHeight = item.Size.Y;
		}

		int totalItems = Items.Count;
		int columns = (int)Math.Ceiling(totalItems / (float)ITEMS_PER_COLUMN);
		int cellWidth = (int)(maxItemWidth + CELL_SPACING_X);
		int cellHeight = (int)(maxItemHeight + CELL_SPACING_Y);
		int journalWidth = MARGIN_X * 2 + columns * cellWidth;
		int journalHeight = MARGIN_Y * 2 + ITEMS_PER_COLUMN * cellHeight;
		int journalX = (Viewport.Width - journalWidth) / 2;
		int journalY = (Viewport.Height - journalHeight) / 2;

		Rectangle journalRect = new Rectangle(journalX, journalY, journalWidth, journalHeight);
		spriteBatch.Draw(JournalBackground, journalRect, Color.Black * 0.7f);

		// Draw headline
		string headline = "Deníček";
		Vector2 headlineSize = LargerFont.MeasureString(headline);
		Vector2 headlinePos = new Vector2(
			journalRect.X + (journalRect.Width - headlineSize.X) / 2,
			journalRect.Y + 20
		);
		spriteBatch.DrawString(LargerFont, headline, headlinePos, Color.White);

		// Layout items in a grid
		float startX = journalRect.X + MARGIN_X;
		float startY = journalRect.Y + MARGIN_Y;
		for (int i = 0; i < Items.Count; i++)
		{
			var item = Items[i];
			int col = i / ITEMS_PER_COLUMN;
			int row = i % ITEMS_PER_COLUMN;
			float itemX = startX + col * cellWidth + (cellWidth - item.Size.X) / 2;
			float itemY = startY + row * cellHeight + (cellHeight - item.Size.Y) / 2;
			item.Position = new Vector2(itemX, itemY);

			// Set the known status before drawing
			item.IsKnown = KnownItems[item.Name];

			item.Draw(spriteBatch);
		}

		// If a known item is hovered, draw its tooltip.
		if (hoveredItem != null)
		{
			DrawTooltip(spriteBatch, hoveredItem);
		}
	}

	private void DrawTooltip(SpriteBatch spriteBatch, ItemContainerable item)
	{
		// Get formatted text from the item.
		string name = item.Name;
		string description = item.Description;
		string stats = FormatStats(item.Stats, item.ItemType == ItemTypes.WEAPON);

		// Measure text sizes.
		Vector2 nameSize = LargerFont.MeasureString(name);
		Vector2 descriptionSize = MiddleFont.MeasureString(description);
		Vector2 statsSize = MiddleFont.MeasureString(stats);
		float tooltipWidth = Math.Max(Math.Max(nameSize.X, descriptionSize.X), statsSize.X) + 20;
		float tooltipHeight = nameSize.Y + descriptionSize.Y + 10 + statsSize.Y;
		Vector2 tooltipPosition = new Vector2(Math.Max(Mouse.GetState().X + 10 - tooltipWidth, 0), Math.Max(Mouse.GetState().Y + 10 - tooltipHeight, 0));
		Rectangle backgroundRect = new Rectangle(tooltipPosition.ToPoint(), new Point((int)tooltipWidth, (int)tooltipHeight));

		// Draw tooltip background.
		spriteBatch.Draw(TooltipTexture, backgroundRect, new Color(60, 60, 60, 200));

		// Draw the text.
		Vector2 textPosition = tooltipPosition + new Vector2(10, 5);
		spriteBatch.DrawString(LargerFont, name, textPosition, Color.White);
		textPosition.Y += nameSize.Y;
		spriteBatch.DrawString(MiddleFont, description, textPosition, Color.LightGray);
		textPosition.Y += descriptionSize.Y + 10;
		Vector2 statsPosition = new Vector2(
			tooltipPosition.X + (tooltipWidth - statsSize.X) / 2,
			textPosition.Y
		);
		spriteBatch.DrawString(MiddleFont, stats, statsPosition, Color.LightCyan);
	}

	// A simple helper to format stats text; adjust as needed.
	private string FormatStats(Dictionary<StatTypes, float> stats, bool weapon)
	{
		if (stats == null || stats.Count == 0) return "";

		StringBuilder sb = new StringBuilder();
		foreach (var stat in stats)
		{
			string displayName = stat.Key switch
			{
				StatTypes.MAX_HP => "Biologie",
				StatTypes.DAMAGE => weapon ? "Síla zbraně" : "Matematika",
				StatTypes.PROJECTILE_COUNT => "Fyzika",
				StatTypes.XP_GAIN => "Zsv",
				StatTypes.ATTACK_SPEED => weapon ? "Rychlost útoků" : "Čeština",
				StatTypes.MOVEMENT_SPEED => "Tělocvik",
				_ => stat.Key.ToString()
			};

			string valueString = stat.Key == StatTypes.ATTACK_SPEED && weapon
				? $"{stat.Value / 1000.0} s"
				: stat.Value.ToString();

			sb.AppendLine($"{displayName}: {valueString}");
		}
		return sb.ToString();
	}
	private static string itemJournalPath = "./itemJournal.json";
	private static Dictionary<string, object> Serialize()
	{
		Dictionary<string, object> output = [];
		foreach (var item in KnownItems)
			output[item.Key] = item.Value;
		return output;
	}

	private static void Deserialize(Dictionary<string, object> pairs)
	{
		try
		{
			foreach (var item in pairs)
			{
				if (item.Value != null)
					KnownItems[item.Key] = Convert.ToBoolean(item.Value);
			}
		}
		catch (Exception e)
		{
			Console.WriteLine("item journal error: " + e.Message);
		}
	}

	public static void Save()
	{
		Dictionary<string, object> serialized = Serialize();
		FileHelper.Save(itemJournalPath, serialized, SaveType.GENERIC);
	}

	public static void Load()
	{
		var data = FileHelper.Load<Dictionary<string, object>>(itemJournalPath, SaveType.GENERIC);
		if (data == null)
		{
			AddAllItems();
			Save();
			data = FileHelper.Load<Dictionary<string, object>>(itemJournalPath, SaveType.GENERIC);
		}
		Deserialize(data);
	}
}
