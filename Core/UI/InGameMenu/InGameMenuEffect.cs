using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
namespace TBoGV;

internal class InGameMenuEffect : InGameMenu
{
    static Viewport Viewport;
    static SpriteFont MiddleFont;
    public Dictionary<StatTypes, int> Stats { get; set; }
    public InGameMenuEffect(Viewport viewport)
    {
        Viewport = viewport;
        SpriteBackground = TextureManager.GetTexture("blackSquare");
        MiddleFont = FontManager.GetFont("Arial12");

        Active = false;
    }
    public override void Update(Viewport viewport, Player player, MouseState mouseState)
    {
        base.Update(viewport, player, mouseState);

        Stats = player.Inventory.SetStats(player.LevelUpStats);
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
    }

    private string GetEffectString(StatTypes statType, float value)
    {
        return statType switch
        {
            StatTypes.MAX_HP => value > 0 ? $"(+{(int)(value * 0.5)} Hp)" : $"({(int)(value * 0.5)} Hp)",
            StatTypes.DAMAGE => $"(+{(value * 10)}% dmg)",
            StatTypes.PROJECTILE_COUNT => $"(+{(int)Math.Max(value / 3,0)} projektilu)",
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
}