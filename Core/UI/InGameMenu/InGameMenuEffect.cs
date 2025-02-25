using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using System.Text;
namespace TBoGV;

internal class InGameMenuEffect : InGameMenu
{
	static Viewport Viewport;
	static SpriteFont MiddleFont;
	static SpriteFont LargerFont;
	static Texture2D SpriteForeground;
	public Dictionary<StatTypes, int> Stats { get; set; }
	public List<Effect> Effects { get; set; }
	public InGameMenuEffect(Viewport viewport)
	{
		Viewport = viewport;
		SpriteBackground = TextureManager.GetTexture("blackSquare");
		MiddleFont = FontManager.GetFont("Arial12");
		SpriteForeground = TextureManager.GetTexture("whiteSquare");
		LargerFont = FontManager.GetFont("Arial16");

		Active = false;
    }
    public override void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, double dt)
    {
        base.Update(viewport, player, mouseState, keyboardState, dt);


		Stats = player.Inventory.SetStats(player.LevelUpStats);
		Effects = player.Inventory.Effects;
		Viewport = viewport;
	}
	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);

		if (Stats == null || Stats.Count == 0) return;

		// Convert stats to a formatted list of (label, value, effect)
		List<(string label, string value, string effect)> statEntries = Stats.Select(stat =>
			(GetStatName(stat.Key), stat.Value.ToString(), GetEffectString(stat.Key, stat.Value))
		).ToList();

		// Measure the widest label, value, and effect separately
		float maxLabelWidth = statEntries.Max(entry => MiddleFont.MeasureString(entry.label).X);
		float maxValueWidth = statEntries.Max(entry => MiddleFont.MeasureString(entry.value).X);
		float maxEffectWidth = statEntries.Max(entry => MiddleFont.MeasureString(entry.effect).X);
		float spacing = 10f; // Space between columns

		// Total width of text block
		float totalWidth = maxLabelWidth + spacing + maxValueWidth + spacing + maxEffectWidth;
		float totalHeight = statEntries.Count * MiddleFont.LineSpacing;

		// Start position centered horizontally
		float startX = (Viewport.Width - totalWidth) / 2;
		float startY = (Viewport.Height - (statEntries.Count * MiddleFont.LineSpacing)) / 2;

		Rectangle backgroundRect = new Rectangle((int)startX - 10, (int)startY - 10, (int)totalWidth + 20, (int)totalHeight + 20);
		spriteBatch.Draw(SpriteBackground, backgroundRect, Color.Black * 0.5f);

		for (int i = 0; i < statEntries.Count; i++)
		{
			string label = statEntries[i].label;
			string value = statEntries[i].value;
			string effect = statEntries[i].effect;

			Vector2 labelPosition = new Vector2(startX, startY + i * MiddleFont.LineSpacing);
			Vector2 valuePosition = new Vector2(startX + maxLabelWidth + spacing + maxValueWidth - MiddleFont.MeasureString(value).X, startY + i * MiddleFont.LineSpacing);
			Vector2 effectPosition = new Vector2(startX + maxLabelWidth + spacing + maxValueWidth + spacing, startY + i * MiddleFont.LineSpacing);

			spriteBatch.DrawString(MiddleFont, label, labelPosition, Color.White);
			spriteBatch.DrawString(MiddleFont, value, valuePosition, Color.White);
			spriteBatch.DrawString(MiddleFont, effect, effectPosition, Color.LightCyan);
		}
		DrawEffects(spriteBatch);
	}

	private string GetEffectString(StatTypes statType, float value)
	{
		return statType switch
		{
			StatTypes.MAX_HP => value > 0 ? $"(+{(int)(value * 0.5)} Hp)" : $"({(int)(value * 0.5)} Hp)",
			StatTypes.DAMAGE => $"(+{(value * 10)}% dmg)",
			StatTypes.PROJECTILE_COUNT => $"(+{(int)Math.Max(value / 3, 0)} projektilu)",
			StatTypes.XP_GAIN => $"(+{value * 10}% xp gain)",
			StatTypes.ATTACK_SPEED => $"(+{value * 10}% rychlost utoku)",
			StatTypes.MOVEMENT_SPEED => $"(+{value * 10}% rychlost pohybu)",
			_ => "(N/A)"
		};
	}

	// Helper method to map StatTypes to display names
	private string GetStatName(StatTypes statType)
	{
		return statType switch
		{
			StatTypes.MAX_HP => "Biologie",
			StatTypes.DAMAGE => "Matematika",
			StatTypes.PROJECTILE_COUNT => "Fyzika",
			StatTypes.XP_GAIN => "Zsv",
			StatTypes.ATTACK_SPEED => "Cestina",
			StatTypes.MOVEMENT_SPEED => "Telocvik",
			_ => statType.ToString()
		};
	}
	public void DrawEffects(SpriteBatch spriteBatch)
	{
		// Get current mouse state for hover detection.
		MouseState mouseState = Mouse.GetState();

		// Starting position for the first effect on the left side.
		Vector2 startPosition = new Vector2(10, 10);
		Vector2 currentPosition = startPosition;
		int spacingBetweenEffects = 10;

		foreach (var effect in Effects)
		{
			effect.Position = currentPosition;
			effect.Draw(spriteBatch);
			currentPosition.Y += effect.GetRect().Height + spacingBetweenEffects;
		}
		foreach (var effect in Effects)
			if (effect.GetRect().Contains(mouseState.Position))
				DrawEffectTooltip(spriteBatch, effect);
	}
	private void DrawEffectTooltip(SpriteBatch spriteBatch, Effect effect)
	{
		// Prepare full tooltip texts.
		string nameText = effect.Name;
		string descriptionText = effect.Description;
		string levelText = $"Level: {effect.Level}";

		// Build a string from the effect stats.
		string statsText = FormatStats(effect.Stats);

		// Define padding values.
		int horizontalPadding = 10;
		int verticalPadding = 5;

		// Measure text sizes using your fonts.
		Vector2 nameSize = LargerFont.MeasureString(nameText);
		Vector2 descriptionSize = MiddleFont.MeasureString(descriptionText);
		Vector2 levelSize = MiddleFont.MeasureString(levelText);
		Vector2 statsSize = MiddleFont.MeasureString(statsText);

		// Calculate tooltip dimensions.
		float tooltipWidth = Math.Max(Math.Max(nameSize.X, descriptionSize.X), Math.Max(levelSize.X, statsSize.X)) + horizontalPadding * 2;
		float tooltipHeight = nameSize.Y + descriptionSize.Y + levelSize.Y + statsSize.Y + verticalPadding * 4;

		// Position the tooltip near the effect's rectangle.
		Vector2 tooltipPosition = new Vector2(effect.GetRect().Right + 5, Math.Min(effect.GetRect().Top, Viewport.Height - effect.GetRect().Height - 5));
		Rectangle tooltipRect = new Rectangle((int)tooltipPosition.X, (int)tooltipPosition.Y, (int)tooltipWidth, (int)tooltipHeight);

		// Draw the tooltip background.
		spriteBatch.Draw(SpriteForeground, tooltipRect, new Color(60, 60, 60, 200));

		// Draw the detailed text inside the tooltip.
		Vector2 textPos = tooltipPosition + new Vector2(horizontalPadding, verticalPadding);
		spriteBatch.DrawString(LargerFont, nameText, textPos + new Vector2(tooltipWidth / 2 - nameSize.X / 2, 0), effect.Positive ? Color.LightGreen : Color.OrangeRed);
		textPos.Y += nameSize.Y + verticalPadding;

		spriteBatch.DrawString(MiddleFont, levelText, textPos + new Vector2(tooltipWidth / 2 - levelSize.X / 2, 0), effect.Positive ? Color.LightGreen : Color.OrangeRed);
		textPos.Y += levelSize.Y + verticalPadding;

		spriteBatch.DrawString(MiddleFont, descriptionText, textPos, Color.LightGray);
		textPos.Y += descriptionSize.Y + verticalPadding;

		spriteBatch.DrawString(MiddleFont, statsText, textPos, Color.LightCyan);
	}
	private string FormatStats(Dictionary<StatTypes, int> stats)
	{
		if (stats == null || stats.Count == 0) return "";

		StringBuilder sb = new StringBuilder();
		foreach (var stat in stats)
		{
			string displayName = stat.Key switch
			{
				StatTypes.MAX_HP => "Biologie",
				StatTypes.DAMAGE => "Matematika",
				StatTypes.PROJECTILE_COUNT => "Fyzika",
				StatTypes.XP_GAIN => "Zsv",
				StatTypes.ATTACK_SPEED => "Cestina",
				StatTypes.MOVEMENT_SPEED => "Telocvik",
				_ => stat.Key.ToString()
			};

			string valueString = stat.Value.ToString();

			sb.AppendLine($"{displayName}: {valueString}");
		}
		return sb.ToString();
	}
}