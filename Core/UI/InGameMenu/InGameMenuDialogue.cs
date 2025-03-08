using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Linq;

namespace TBoGV;
public class InGameMenuDialogue : InGameMenu
{
    private static Viewport Viewport;
    private readonly SpriteFont titleFont = FontManager.GetFont("Arial24");
    private readonly SpriteFont mainTextFont = FontManager.GetFont("Arial16");
    private readonly SpriteFont buttonFont = FontManager.GetFont("Arial16");

    private EntityPassive npc;
    private string npcText = "prdis";
    private DialogueElement currentElement = null;

    // Menu buttons.
    private readonly List<Button> buttons = new List<Button>();

    public InGameMenuDialogue(Viewport viewport, EntityPassive entity)
    {
        Viewport = viewport;
        npc = entity;

        SpriteBackground = TextureManager.GetTexture("blackSquare");

        // Create buttons with sample actions.
        buttons.Add(new Button("", buttonFont, null));
        buttons.Add(new Button("", buttonFont, null));
        buttons.Add(new Button("", buttonFont, null));
        // Set a uniform size and text color for each button.
        foreach (var button in buttons)
        {
            button.SetSize(new Vector2(400, 50));
            button.SetTextColor(Color.White);
        }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        // Npc jmeno
        spriteBatch.DrawString(
            titleFont,
            npc.Name,
            new Vector2((Viewport.Width - titleFont.MeasureString(npc.Name).X) / 2, prcY(12)),
            Color.White
        );

        // Hlava npccka
        Texture2D sprite = npc.GetSprite();

        int size = 2;
        Vector2 spritePos = new Vector2((Viewport.Width - (size * sprite.Width)) / 2, prcY(20));
        Rectangle spriteRect = new Rectangle(
            (int)spritePos.X,
            (int)spritePos.Y,
            size * sprite.Width,
            size * sprite.Height);
        spriteBatch.Draw(sprite, spriteRect, Color.White);


        // Draw the npc's text
        Vector2 mainTextSize = mainTextFont.MeasureString(npcText);
        Vector2 mainTextPos = new Vector2((Viewport.Width - mainTextSize.X) / 2, prcY(40));
        Rectangle textBlockRect = new Rectangle(
            (int)mainTextPos.X - 10,
            (int)mainTextPos.Y - 10,
            (int)mainTextSize.X + 20,
            (int)mainTextSize.Y + 20);
        spriteBatch.Draw(SpriteBackground, textBlockRect, new Color(0, 0, 0, 100));
        spriteBatch.DrawString(mainTextFont, npcText, mainTextPos, Color.White);

        // Moznosti
        int buttonSpacing = 20;
        // Calculate total height needed for the buttons.
        int totalButtonsHeight = buttons.Sum(b => b.GetRect().Height) + (buttons.Count - 1) * buttonSpacing;
        // Start drawing buttons at about mid-screen (adjust prcY value as needed).
        float startY = prcY(60);

        foreach (var button in buttons)
        {
            int buttonWidth = button.GetRect().Width;
            float posX = (Viewport.Width - buttonWidth) / 2;
            button.Position = new Vector2(posX, startY);
            button.Draw(spriteBatch);
            startY += button.GetRect().Height + buttonSpacing;
        }
    }
}