using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;

namespace TBoGV;
public class InGameMenuDialogue : InGameMenu
{
    private static Viewport Viewport;
    private readonly SpriteFont titleFont = FontManager.GetFont("Arial24");
    private readonly SpriteFont mainTextFont = FontManager.GetFont("Arial16");
    private readonly SpriteFont buttonFont = FontManager.GetFont("Arial16");

    private string npcText = "prdis";
    private string npcName = "";
    private Texture2D npcSprite;
    private Dialogue dialogue;

    // Menu buttons.
    private List<Button> choiceButtons = new List<Button>();
    private Button nextButton;

    public InGameMenuDialogue(Viewport viewport, EntityPassive entity) : this(viewport, entity.Dialogue, entity.Name, entity.GetSprite()) { }
    public InGameMenuDialogue(Viewport viewport, Dialogue dialogue, string name, Texture2D sprite)
    {
        Viewport = viewport;
        SpriteBackground = TextureManager.GetTexture("blackSquare");

        this.dialogue = dialogue;
        npcName = name;
        npcSprite = sprite;

        nextButton = new Button("Dále", buttonFont, () => AdvanceDialogue());
        nextButton.SetSize(buttonFont.MeasureString("Dále") + new Vector2(50, 25));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        // Npc jmeno
        spriteBatch.DrawString(
            titleFont,
            npcName,
            new Vector2((Viewport.Width - titleFont.MeasureString(npcName).X) / 2, prcY(12)),
            Color.White
        );

        int size = 2;
        Vector2 spritePos = new Vector2((Viewport.Width - (size * npcSprite.Width)) / 2, prcY(20));
        Rectangle spriteRect = new Rectangle(
            (int)spritePos.X,
            (int)spritePos.Y,
            size * npcSprite.Width,
            size * npcSprite.Height);
        spriteBatch.Draw(npcSprite, spriteRect, Color.White);


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

        int buttonSpacing = 20;
        // jen pokud mame moznost odpovidat 
        if (dialogue.CurrentElement.Choices == null)
        {
            nextButton.Position = new Vector2(
                (Viewport.Width - nextButton.GetRect().Width) / 2,
                prcY(60) + nextButton.GetRect().Height + buttonSpacing);
            nextButton.Draw(spriteBatch);
        }
        else
        {
            // Calculate total height needed for the buttons.
            int totalButtonsHeight = choiceButtons.Sum(b => b.GetRect().Height) + (choiceButtons.Count - 1) * buttonSpacing;
            // Start drawing buttons at about mid-screen (adjust prcY value as needed).
            float startY = prcY(60);

            foreach (var button in choiceButtons)
            {
                int buttonWidth = button.GetRect().Width;
                float posX = (Viewport.Width - buttonWidth) / 2;
                button.Position = new Vector2(posX, startY);
                button.Draw(spriteBatch);
                startY += button.GetRect().Height + buttonSpacing;
            }
        }
    }

    public override void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, double dt)
    {
        base.Update(viewport, player, mouseState, keyboardState, dt);
        if (npcText != dialogue.CurrentElement.Text && dialogue.CurrentElement.Text != null)
            npcText = dialogue.CurrentElement.Text;

        if (dialogue.CurrentElement.Choices == null)
            nextButton.Update(mouseState);
        else
        {
            choiceButtons.ForEach(b => b.Update(mouseState));
            if (shouldAdvance)
                AdvanceDialogue();
        }
    }
    bool shouldAdvance = false;

    protected void AdvanceDialogue()
    {
        shouldAdvance = false;
        choiceButtons.Clear();
        if (!dialogue.Advance())
        {
            CloseMenu.Invoke();
            return;
        }

        var element = dialogue.CurrentElement;

        if (element.Choices != null)
        {
            Dictionary<string, string> choices = dialogue.CurrentElement.Choices;
            // choice buttons
            // Set a uniform size and text color for each button.
            foreach ((string t, string reference) in choices)
            {
                choiceButtons.Add(new Button(t, buttonFont, () =>
                {
                    dialogue.Respond(reference);
                    shouldAdvance = true;
                }));
                choiceButtons.Last().SetSize(buttonFont.MeasureString(t) + new Vector2(50, 25));
                choiceButtons.Last().SetTextColor(Color.White);
            }
            return;
        }
        if (element.Action != null)
        {
            element.Action.Invoke();
            AdvanceDialogue();
        }
    }
}