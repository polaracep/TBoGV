using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
namespace TBoGV;

internal class InGameMenuEffect : InGameMenu
{
    static Viewport Viewport;
    List<ItemContainer> ItemContainers;
    static SpriteFont SmallFont;
    static SpriteFont MiddleFont;
    static SpriteFont LargerFont;
    static Texture2D SpriteForeground;
    static Texture2D SpriteToolTip;
    Inventory Inventory;
    ItemContainer? hoveredItem;
    public Dictionary<StatTypes, int> Stats { get; set; }
    public InGameMenuEffect(Viewport viewport)
    {
        Viewport = viewport;
        SpriteBackground = TextureManager.GetTexture("blackSquare");
        SpriteForeground = TextureManager.GetTexture("whiteSquare");
        SpriteToolTip = TextureManager.GetTexture("container");
        SmallFont = FontManager.GetFont("Arial8");
        MiddleFont = FontManager.GetFont("Arial12");
        LargerFont = FontManager.GetFont("Arial24");
        Active = false;
    }
    public override void Update(Viewport viewport, Player player, MouseState mouseState)
    {
        base.Update(viewport, player, mouseState);
        Stats = player.Inventory.SetStats(player.BaseStats);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        if (Stats == null || Stats.Count == 0) return;

        // Convert stats to a formatted list of (label, value)
        List<(string label, string value)> statEntries = Stats.Select(stat =>
            (GetStatName(stat.Key), stat.Value.ToString())
        ).ToList();

        // Measure the widest label and widest value separately
        float maxLabelWidth = statEntries.Max(entry => MiddleFont.MeasureString(entry.label).X);
        float maxValueWidth = statEntries.Max(entry => MiddleFont.MeasureString(entry.value).X);
        float spacing = 10f; // Space between label and value

        // Total width of text block
        float totalWidth = maxLabelWidth + spacing + maxValueWidth;

        // Start position centered horizontally
        float startX = (Viewport.Width - totalWidth) / 2;
        float startY = (Viewport.Height - (statEntries.Count * MiddleFont.LineSpacing)) / 2;



        for (int i = 0; i < statEntries.Count; i++)
        {
            string label = statEntries[i].label;
            string value = statEntries[i].value;

            Vector2 labelPosition = new Vector2(startX, startY + i * MiddleFont.LineSpacing);
            Vector2 valuePosition = new Vector2(startX + maxLabelWidth + spacing, startY + i * MiddleFont.LineSpacing);

            spriteBatch.DrawString(MiddleFont, label, labelPosition, Color.White);
            spriteBatch.DrawString(MiddleFont, value, valuePosition, Color.White);
        }


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
