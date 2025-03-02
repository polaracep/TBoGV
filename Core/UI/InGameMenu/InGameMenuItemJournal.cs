using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;

class InGameMenuItemJournal : InGameMenu
{
	private static Viewport Viewport;
	private static SpriteFont MiddleFont;
	private static SpriteFont LargerFont;
	private static Texture2D JournalBackground;
	private static Texture2D TooltipTexture;  // Tooltip background texture

	// All the item objects that we want to show in the journal.
	private List<ItemContainerable> Items = new List<ItemContainerable>();

	// Known status: itemName -> bool
	private Dictionary<string, bool> KnownItems = new Dictionary<string, bool>();

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

	public InGameMenuItemJournal(Viewport viewport)
	{
		Viewport = viewport;
		JournalBackground = TextureManager.GetTexture("blackSquare");
		TooltipTexture = TextureManager.GetTexture("blackSquare");
		MiddleFont = FontManager.GetFont("Arial12");
		LargerFont = FontManager.GetFont("Arial16");

		// Create and add all item instances
		AddAllItems();

		Active = false;
	}

	private void AddAllItems()
	{
		// Define an array of item types
		//var itemTypes = new Type[]
		var ItemsList = new ItemContainerable[]
		{
			new ItemCalculator(Vector2.Zero),
			new ItemDoping(Vector2.Zero),
			new ItemMonster(Vector2.Zero),
			new ItemFancyShoes(Vector2.Zero),
			new ItemFlipFlop(Vector2.Zero),
			new ItemTrackShoes(Vector2.Zero),
			new ItemMap(Vector2.Zero),
			new ItemMathProblem(Vector2.Zero),
			new ItemTeeth(Vector2.Zero),
			new ItemAdBlock(Vector2.Zero),
			new ItemExplosive(Vector2.Zero),
			new ItemDagger(Vector2.Zero),
			new ItemPencil(Vector2.Zero),
			new ItemBookBio(Vector2.Zero),
			new ItemBookCzech(Vector2.Zero),
			new ItemBookMath(Vector2.Zero),
			new ItemBookZsv(Vector2.Zero),
			new ItemBookPhysics(Vector2.Zero),
			new ItemCross(Vector2.Zero),
			new ItemBryle(Vector2.Zero),
			new ItemBook(Vector2.Zero),
			new ItemBookPE(Vector2.Zero),
			new ItemPen(Vector2.Zero),
			new ItemScissors(Vector2.Zero),
			new ItemRubbedBoots(Vector2.Zero),
			new ItemFixa(Vector2.Zero),
			new ItemLabcoat(Vector2.Zero),
		};

		// Use a foreach loop to add items to the list
		/*
		foreach (var itemType in itemTypes)
		{
			// Use reflection to create the item instance
			var item = Activator.CreateInstance(itemType, new object[] { Vector2.Zero });
			Items.Add((ItemContainerable)item);
		}
		*/
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
	public void ShowAll()
	{
		// Create a temporary list of keys to avoid modifying the collection while iterating.
		foreach (var key in new List<string>(KnownItems.Keys))
		{
			KnownItems[key] = true;
		}
	}
	private void UpdateKnownItems(List<ItemContainer> containers)
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
	}

	public override void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, double dt)
	{
		base.Update(viewport, player, mouseState, keyboardState, dt);

		if (!Active)
			return;

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
		if (!Active)
			return;

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
		string headline = "Item Journal";
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
	private string FormatStats(Dictionary<StatTypes, int> stats, bool weapon)
	{
		if (stats == null || stats.Count == 0) return "";

		StringBuilder sb = new StringBuilder();
		foreach (var stat in stats)
		{
			string displayName = stat.Key switch
			{
				StatTypes.MAX_HP => "Biologie",
				StatTypes.DAMAGE => weapon ? "Sila zbrane" : "Matematika",
				StatTypes.PROJECTILE_COUNT => "Fyzika",
				StatTypes.XP_GAIN => "Zsv",
				StatTypes.ATTACK_SPEED => weapon ? "Rychlost utoku" : "Cestina",
				StatTypes.MOVEMENT_SPEED => "Telocvik",
				_ => stat.Key.ToString()
			};

			string valueString = stat.Key == StatTypes.ATTACK_SPEED && weapon
				? $"{stat.Value / 1000.0} s"
				: stat.Value.ToString();

			sb.AppendLine($"{displayName}: {valueString}");
		}
		return sb.ToString();
	}

	public void OpenMenu()
	{
		Active = true;
	}

	private void ResetJournal()
	{
		Active = false;
	}
}
