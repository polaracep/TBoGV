using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;

internal class InGameMenuLevelUp : InGameMenu
{
    private static Viewport Viewport;
    static SpriteFont SmallFont;
    static SpriteFont MiddleFont;
    static SpriteFont LargerFont;
    private List<StatTypes> statOptions = new();
    private Rectangle[] optionBounds = new Rectangle[3]; // Clickable areas for options
    private string chosenDescription = "";
    private int hoveredOption = -1; // Track which option is hovered
    private readonly List<string> descriptions = new List<string>
    {
        "odlozil jsi na chvili telefon a proto jsi se soustredil",
        "dlouho jsi neposlouchal sigma boy",
        "uvaril jsi neohlaseny test",
        "daval jsi pozor alespon 5 minut v kuse",
        "rozbil se ti endless scroll",
        "dneska ti nikdo neposlal reel",
        "prinesl jsi si veci do skoly",
        "dneska se ti nezdaly sny o neexistujicich lego dilcich",
        "schovanek byl cooked vic nez ty na TFY",
        "dostal jsi pridano na obede"
    };

    // Extra text to implicitly indicate that the option improves understanding
    private readonly string understandingHint = "volbou se ti zlepsi pochopeni predmetu";

    private const int Padding = 10;
    private MouseState previousMouseState; // Stores the previous mouse state


    public InGameMenuLevelUp(Viewport viewport)
    {
        Viewport = viewport;
        SpriteBackground = TextureManager.GetTexture("blackSquare");
        SmallFont = FontManager.GetFont("Arial8");
        MiddleFont = FontManager.GetFont("Arial12");
        LargerFont = FontManager.GetFont("Arial16");
        Active = false;
    }

    private void GenerateStatOptions()
    {
        Array statValues = Enum.GetValues(typeof(StatTypes));
        Random random = new Random();
        statOptions = statValues.Cast<StatTypes>()
                                .OrderBy(x => random.Next())
                                .Take(3)
                                .ToList();

        chosenDescription = descriptions[random.Next(descriptions.Count)];
    }

    public override void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, double dt)
    {
        base.Update(viewport, player, mouseState, keyboardState, dt);

        if (!Active)
            return;

        hoveredOption = -1; // Reset hover state

        Point mousePos = mouseState.Position;
        for (int i = 0; i < statOptions.Count; i++)
        {
            if (optionBounds[i].Contains(mousePos))
            {
                hoveredOption = i; // Mark hovered option

                if (previousMouseState.LeftButton == ButtonState.Pressed &&
                                    mouseState.LeftButton == ButtonState.Released)
                {
                    StatTypes selectedStat = statOptions[i];
                    if (player.LevelUpStats.ContainsKey(selectedStat))
                        player.LevelUpStats[selectedStat] += 1;
                    else
                        player.LevelUpStats[selectedStat] = 1;

                    Active = false; // Close menu after selection
                    return;
                }
            }
        }
        previousMouseState = mouseState;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!Active)
            return;

        base.Draw(spriteBatch);
        string headline = "Vyber";
        Vector2 hintSize = SmallFont.MeasureString(understandingHint);        
        Vector2 descSize = MiddleFont.MeasureString(chosenDescription);
        Vector2 headlineSize = LargerFont.MeasureString(headline);
        int menuHeight = (int)Math.Max(descSize.Y, Viewport.Height / 2);
        int menuWidth = (int)Math.Max(descSize.X + 30, Viewport.Width / 2);
        Rectangle menuBackground = new Rectangle(
            Viewport.Width / 4 - (menuWidth - Viewport.Width/2)/2, Viewport.Height / 4,
            menuWidth, menuHeight
        );
        // Draw full menu background
        spriteBatch.Draw(SpriteBackground, menuBackground, Color.Black * 0.7f);
        // Draw headline
        Vector2 headlinePos = new Vector2(Viewport.Width / 2 - headlineSize.X / 2, menuBackground.Y + 20);
        spriteBatch.DrawString(LargerFont, headline, headlinePos, Color.White);

        // Draw description text
        Vector2 descPos = new Vector2(Viewport.Width / 2 - descSize.X / 2, headlinePos.Y + headlineSize.Y + 10);
        spriteBatch.DrawString(MiddleFont, chosenDescription, descPos, Color.White);

        Vector2 hintPos = new Vector2(Viewport.Width / 2 - hintSize.X / 2, descPos.Y + descSize.Y + 10);
        spriteBatch.DrawString(SmallFont, understandingHint, hintPos, Color.LightGray);



        // Determine positions for the 3 horizontal option rectangles.
        int totalOptions = statOptions.Count;
        float optionsY = hintPos.Y + hintSize.Y + 30;

        List<Vector2> optionTextSizes = new List<Vector2>();
        List<Rectangle> backgrounds = new List<Rectangle>();
        int totalWidth = 0;

        for (int i = 0; i < totalOptions; i++)
        {
            string statName = GetStatName(statOptions[i]);
            Vector2 textSize = MiddleFont.MeasureString(statName);
            optionTextSizes.Add(textSize);

            Rectangle rect = new Rectangle(0, 0, (int)textSize.X + Padding * 2, (int)textSize.Y + Padding * 2);
            backgrounds.Add(rect);

            totalWidth += rect.Width;
        }

        int spacing = 20;
        totalWidth += spacing * (totalOptions - 1);
        float startX = Viewport.Width / 2 - totalWidth / 2;

        for (int i = 0; i < totalOptions; i++)
        {
            string statName = GetStatName(statOptions[i]);
            Rectangle bgRect = backgrounds[i];
            bgRect.X = (int)startX;
            bgRect.Y = (int)optionsY;
            optionBounds[i] = bgRect;

            // Change color if hovered
            Color backgroundColor = (hoveredOption == i) ? Color.Gray * 0.8f : Color.Black * 0.5f;
            spriteBatch.Draw(SpriteBackground, bgRect, backgroundColor);

            Vector2 textSize = optionTextSizes[i];
            Vector2 textPos = new Vector2(
                bgRect.X + Padding + (bgRect.Width - Padding * 2 - textSize.X) / 2,
                bgRect.Y + Padding + (bgRect.Height - Padding * 2 - textSize.Y) / 2
            );
            spriteBatch.DrawString(MiddleFont, statName, textPos, Color.White);

            startX += bgRect.Width + spacing;
        }
    }

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

    public void OpenMenu()
    {
        GenerateStatOptions();
        Active = true;
    }
}
