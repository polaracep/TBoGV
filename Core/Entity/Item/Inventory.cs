﻿using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace TBoGV;

public class Inventory
{
	static SpriteFont Font;
	static SpriteFont MiddleFont;
	static SpriteFont LargerFont;
	static Texture2D SpriteForeground;
	static Texture2D SpriteToolTip;

	public List<ItemContainer> ItemContainers;
	public int selectedItemIndex = 0;
	private int PrevScrollWheelValue;
	ItemContainer? hoveredItem;
	Vector2 Position {  get; set; }
	public Inventory()
	{
		ItemContainer weapon = new();
		weapon.ContainerType = ItemTypes.WEAPON;
		ItemContainer armor = new();
		armor.ContainerType = ItemTypes.ARMOR;
		ItemContainer effect = new();
		effect.ContainerType = ItemTypes.EFFECT;

		weapon.Item = new ItemDoping(Vector2.Zero);
		ItemContainers = new List<ItemContainer>() { weapon, armor, effect, new ItemContainer(), new ItemContainer(), new ItemContainer()};
		SpriteForeground = TextureManager.GetTexture("whiteSquare");
		SpriteToolTip = TextureManager.GetTexture("container");
		Font = FontManager.GetFont("Arial8");
		MiddleFont = FontManager.GetFont("Arial12");
		LargerFont = FontManager.GetFont("Arial24");

	}

	public void Draw(SpriteBatch spriteBatch)
	{
		foreach (var container in ItemContainers)
		{
			container.Draw(spriteBatch);
		}
		if (hoveredItem != null)
		{
			DrawTooltip(spriteBatch, hoveredItem);
		}
	}

	public Dictionary<StatTypes, int> SetStats(Dictionary<StatTypes, int> BaseStats)
	{
		Dictionary<StatTypes, int> finalStats = new Dictionary<StatTypes, int>(BaseStats);

		foreach (var container in ItemContainers)
		{
			if (container.Item != null)
			{
				foreach (var stat in container.Item.Stats)
				{
					if (finalStats.ContainsKey(stat.Key))
					{
						finalStats[stat.Key] += stat.Value;
					}
					else
					{
						finalStats[stat.Key] = stat.Value;
					}
				}
			}
		}
	return finalStats;
	}
	public void Update(Viewport viewport, Player player, MouseState mouseState)
	{
		hoveredItem = null; // Reset hovered item

		Position = new Vector2(viewport.Width/2 - ItemContainers.Count * (ItemContainers[0].Size.X)/2 , viewport.Height - ItemContainers[0].Size.Y);
		for (int i = 0; i < ItemContainers.Count; i++)
		{
			Vector2 containerPosition = Position + new Vector2(i * ItemContainers[i].Size.X, 0);
			ItemContainers[i].SetPosition(containerPosition);
			// Check if mouse is over an item
			if (ItemContainers[i].GetRectangle().Contains(mouseState.Position) && !ItemContainers[i].IsEmpty())
			{
				hoveredItem = ItemContainers[i];
			}
		}

		int scrollDelta = PrevScrollWheelValue - mouseState.ScrollWheelValue;
		if (scrollDelta > 0)
		{
			selectedItemIndex = (selectedItemIndex + 1) % ItemContainers.Count;
			SetActiveItemContainer();
		}
		else if (scrollDelta < 0)
		{
			selectedItemIndex = (selectedItemIndex - 1 + ItemContainers.Count) % ItemContainers.Count;
			SetActiveItemContainer();
		}
		PrevScrollWheelValue = mouseState.ScrollWheelValue;
	}
	private void SetActiveItemContainer()
	{
		for (int i = 0; i < ItemContainers.Count; i++)
		{
			ItemContainers[i].Selected = (i == selectedItemIndex);
		}
	}
	private void DrawTooltip(SpriteBatch spriteBatch, ItemContainer item)
	{


		// Get formatted text
		string name = item.Item.Name;
		string description = item.Item.Description;
		string stats = FormatStats(item.Item.Stats);

		// Measure text sizes
		Vector2 nameSize = LargerFont.MeasureString(name);
		Vector2 descriptionSize = MiddleFont.MeasureString(description);
		Vector2 statsSize = Font.MeasureString(stats);

		// Determine tooltip width & height
		float tooltipWidth = Math.Max(Math.Max(nameSize.X, descriptionSize.X), statsSize.X) + 20;
		float tooltipHeight = nameSize.Y + descriptionSize.Y + 10 + statsSize.Y + 20; // Extra 10px space after description
		Vector2 tooltipPosition = new Vector2(Mouse.GetState().X + 10 - tooltipWidth, Mouse.GetState().Y + 10 - tooltipHeight);
		Rectangle backgroundRect = new Rectangle(tooltipPosition.ToPoint(), new Point((int)tooltipWidth, (int)tooltipHeight));

		// Draw background
		spriteBatch.Draw(SpriteForeground, backgroundRect, new Color(141, 122, 38));

		// Draw text
		Vector2 textPosition = tooltipPosition + new Vector2(10, 5);
		spriteBatch.DrawString(LargerFont, name, textPosition, Color.White);
		textPosition.Y += nameSize.Y;

		spriteBatch.DrawString(MiddleFont, description, textPosition, Color.LightGray);
		textPosition.Y += descriptionSize.Y + 10; // Add extra space below description

		// Center stats within the tooltip
		Vector2 statsPosition = new Vector2(
			tooltipPosition.X + (tooltipWidth - statsSize.X) / 2, // Center horizontally
			textPosition.Y
		);
		spriteBatch.DrawString(Font, stats, statsPosition, Color.White);
	}

	private string FormatStats(Dictionary<StatTypes, int> stats)
	{
		if (stats == null || stats.Count == 0) return "No stats available";

		StringBuilder sb = new StringBuilder();
		foreach (var stat in stats)
		{
			sb.AppendLine($"{stat.Key}: {stat.Value}");
		}
		return sb.ToString();
	}
}

