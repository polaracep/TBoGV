using Microsoft.Xna.Framework.Input;
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
	public List<Effect> Effects = new List<Effect>();
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
		ItemContainer kalkulacka = new();
		kalkulacka.Item = new ItemCalculator(Vector2.Zero);
		kalkulacka.Selected = true;
        weapon.Item = new ItemDagger(Vector2.Zero);
		ItemContainers = new List<ItemContainer>() { weapon, armor, effect, kalkulacka, new ItemContainer(), new ItemContainer()};
		SpriteForeground = TextureManager.GetTexture("whiteSquare");
		SpriteToolTip = TextureManager.GetTexture("containerBorder");
		Font = FontManager.GetFont("Arial8");
		MiddleFont = FontManager.GetFont("Arial12");
		LargerFont = FontManager.GetFont("Arial16");
	}
	public void AddEffect(Effect effect)
	{
		foreach (var e in Effects)
		{
			if (effect.Name == e.Name)
			{
				e.ChangeLevel(effect.Level);
				return;
			}
		}
		Effects.Add(effect);		
	}
	public void RemoveEffect(Effect effect)
	{
		for (int i = 0; i < Effects.Count; i++)
		{
			if (Effects[i].GetType() == effect.GetType())
			{
				Effects.RemoveAt(i);
				return;
			}
		}
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
    public Dictionary<StatTypes, int> SetStats()
	{ 
		return SetStats(new Dictionary<StatTypes, int>()
        {
            { StatTypes.MAX_HP, 0 },
            { StatTypes.DAMAGE, 0 },
            { StatTypes.PROJECTILE_COUNT, 0 },
            { StatTypes.XP_GAIN, 0 },        // Získávání XP v %  
			{ StatTypes.ATTACK_SPEED, 0 },
            { StatTypes.MOVEMENT_SPEED, 0 }
        });
    }

    public Dictionary<StatTypes, int> SetStats(Dictionary<StatTypes, int> BaseStats)
	{
		Dictionary<StatTypes, int> finalStats = new Dictionary<StatTypes, int> (BaseStats);

		foreach (var container in ItemContainers)
		{
			if (container.Item != null)
			{
				foreach (var stat in container.Item.Stats)
				{
					if (container.Item.ItemType == ItemTypes.WEAPON && (stat.Key == StatTypes.ATTACK_SPEED || stat.Key == StatTypes.DAMAGE))
						continue;
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
		foreach (var effect in Effects)
		{
			foreach (var stat in effect.Stats)
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
		return finalStats;
	}
	public List<EffectTypes> GetEffect()
	{
		List<EffectTypes> effects = new List<EffectTypes>();
		foreach (var container in ItemContainers)
			if (!container.IsEmpty())
				foreach (var effect in container.Item.Effects)			
					effects.Add(effect);
		foreach (var effect in Effects)
			foreach (var e in effect.Effects)
				effects.Add(e);
		return effects;	
	}
	public float GetWeaponDmg()
	{
		return ItemContainers[0].IsEmpty() ? 1 : ItemContainers[0].Item.Stats[StatTypes.DAMAGE];
    }
    public float GetWeaponAttackSpeed()
    {
        return ItemContainers[0].IsEmpty() ? 1500 : ItemContainers[0].Item.Stats[StatTypes.ATTACK_SPEED];
    }
	public Texture2D GetWeaponSprite()
	{
		return ItemContainers[0].IsEmpty() ? TextureManager.GetTexture("projectile") : ItemContainers[0].Item.GetSprite();
	}
	public void Update(Viewport viewport, Player player, MouseState mouseState)
	{
		hoveredItem = null; // Reset hovered item
		int separatorWidth = 10;
		Position = new Vector2(
			viewport.Width / 2 - ItemContainers.Count * (ItemContainers[0].Size.X) / 2 - separatorWidth / 2,
			viewport.Height - ItemContainers[0].Size.Y);

		for (int i = 0; i < ItemContainers.Count; i++)
		{
			Vector2 containerPosition = Position + new Vector2(i * ItemContainers[i].Size.X, 0);
			containerPosition.X += (i > 2) ? separatorWidth : 0;
			ItemContainers[i].SetPosition(containerPosition);

			// Check if mouse is over an item
			if (ItemContainers[i].GetRectangle().Contains(mouseState.Position) && !ItemContainers[i].IsEmpty())
			{
				hoveredItem = ItemContainers[i];
			}
		}

		// Handle scroll wheel selection (for basic items only)
		int scrollDelta = PrevScrollWheelValue - mouseState.ScrollWheelValue;
		if (scrollDelta > 0)
		{
			selectedItemIndex = 3 + ((selectedItemIndex + 1 - 3) % (ItemContainers.Count - 3));
			SetActiveItemContainer();
		}
		else if (scrollDelta < 0)
		{
			selectedItemIndex = 3 + ((selectedItemIndex - 1 - 3 + (ItemContainers.Count - 3)) % (ItemContainers.Count - 3));
			SetActiveItemContainer();
		}
		PrevScrollWheelValue = mouseState.ScrollWheelValue;

		// Handle number key selection for basic items (starting at index 3)
		KeyboardState keyboardState = Keyboard.GetState();
		// Check for number keys D1 through D9.
		for (int i = 1; i <= 9; i++)
		{
			Keys key = Keys.D1 + (i - 1);
			if (keyboardState.IsKeyDown(key))
			{
				int containerIndex = 3 + (i - 1);
				if (containerIndex < ItemContainers.Count)
				{
					selectedItemIndex = containerIndex;
					SetActiveItemContainer();
				}
			}
		}
	}
	private void SetActiveItemContainer()
	{
		for (int i = 0; i < ItemContainers.Count; i++)
		{
			ItemContainers[i].Selected = (i == selectedItemIndex);
		}
	}
	private ItemContainer GetSelectedItemContainer()
	{
        for (int i = 0; i < ItemContainers.Count; i++)
			if (ItemContainers[i].Selected)
				return ItemContainers[i];
		return null;
    }
	private void DrawTooltip(SpriteBatch spriteBatch, ItemContainer item)
	{
		// Get formatted text
		string name = item.Item.Name;
		string description = item.Item.Description;
		string stats = FormatStats(item.Item.Stats, item.ContainerType == ItemTypes.WEAPON);

		// Measure text sizes
		Vector2 nameSize = LargerFont.MeasureString(name);
		Vector2 descriptionSize = MiddleFont.MeasureString(description);
		Vector2 statsSize = MiddleFont.MeasureString(stats);

		// Determine tooltip width & height
		float tooltipWidth = Math.Max(Math.Max(nameSize.X, descriptionSize.X), statsSize.X) + 20;
		float tooltipHeight = nameSize.Y + descriptionSize.Y + 10 + statsSize.Y ; // Extra 10px space after description
		Vector2 tooltipPosition = new Vector2(Math.Max(Mouse.GetState().X + 10 - tooltipWidth, 0), Math.Max(Mouse.GetState().Y + 10 - tooltipHeight, 0));
		Rectangle backgroundRect = new Rectangle(tooltipPosition.ToPoint(), new Point((int)tooltipWidth, (int)tooltipHeight));

		// Draw background
		spriteBatch.Draw(SpriteForeground, backgroundRect, new Color(60,60,60, 200));

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
		spriteBatch.DrawString(MiddleFont, stats, statsPosition, Color.LightCyan);
	}
	public bool PickUpItem(ItemContainerable item)
	{
		switch (item.ItemType)
		{
			case ItemTypes.EFFECT:
				if (ItemContainers[2].IsEmpty())
				{
					ItemContainers[2].Item = item;
					return true;
				}
				return false;
			case ItemTypes.WEAPON:
                if (ItemContainers[0].IsEmpty())
				{
					ItemContainers[0].Item = item;
					return true;
				}
				return false;
			case ItemTypes.ARMOR:
                if (ItemContainers[1].IsEmpty())
				{
					ItemContainers[1].Item = item;
					return true;
				}
				return false;
            case ItemTypes.BASIC:
				for (int i = 3; i < ItemContainers.Count; i++)
				{
					if (ItemContainers[i].IsEmpty())
					{
						ItemContainers[i].Item = item;
						return true;
					}
                }
				return false;
			default:
				return false;
		}
	}
    public ItemContainerable SwapItem(ItemContainerable item)
    {
		ItemContainerable itemToDrop;
        switch (item.ItemType)
        {
            case ItemTypes.EFFECT:
				itemToDrop = ItemContainers[2].Item;
                ItemContainers[2].Item = item;
				break;
            case ItemTypes.WEAPON:
                itemToDrop = ItemContainers[0].Item;
                ItemContainers[0].Item = item;
                break;
            case ItemTypes.ARMOR:
                itemToDrop = ItemContainers[1].Item;
                ItemContainers[1].Item = item;
                break;
            case ItemTypes.BASIC:
				itemToDrop = GetSelectedItemContainer().Item;
                GetSelectedItemContainer().Item = item;
				break;
            default:
                return item;
        }
        itemToDrop.Position = item.Position;
		return itemToDrop;
    }

    private string FormatStats(Dictionary<StatTypes, int> stats, bool weapon)
    {
        if (stats == null || stats.Count == 0) return "No stats available";

        StringBuilder sb = new StringBuilder();
        foreach (var stat in stats)
        {
            string displayName = stat.Key switch
            {
                StatTypes.MAX_HP => "Biologie",
                StatTypes.DAMAGE => weapon ? "Sila zbrane" :"Matematika",
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
	
}

