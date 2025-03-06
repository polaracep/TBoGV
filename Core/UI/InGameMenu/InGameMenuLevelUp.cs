using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;

class InGameMenuLevelUp : InGameMenu
{
    private static Viewport Viewport;
    static SpriteFont SmallFont;
    static SpriteFont MiddleFont;
    static SpriteFont LargerFont;

    private List<Button> buttons = new();

    private string chosenDescription = "";
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

    public InGameMenuLevelUp(Viewport viewport)
    {
        Viewport = viewport;
        SpriteBackground = TextureManager.GetTexture("blackSquare");
        SmallFont = FontManager.GetFont("Arial8");
        MiddleFont = FontManager.GetFont("Arial12");
        LargerFont = FontManager.GetFont("Arial16");
        Active = false;
    }

    private void GenerateStatOptions(Player player)
    {
        buttons.Clear();
        Array statValues = Enum.GetValues(typeof(StatTypes));
        Random random = new Random();
        var statOptions = statValues.Cast<StatTypes>()
                                .OrderBy(x => random.Next())
                                .Take(3)
                                .ToList();
        foreach (var statOption in statOptions)
        {
            buttons.Add(new Button(GetStatName(statOption), MiddleFont, () =>
            {
                if (player.LevelUpStats.ContainsKey(statOption))
                    player.LevelUpStats[statOption] += 1;
                else
                    player.LevelUpStats[statOption] = 1;
                Active = false; // Close menu after selection
            }));
        }
        foreach (var b in buttons)
            b.SetSize(new Vector2(110,b.GetRect().Height));

        chosenDescription = descriptions[random.Next(descriptions.Count)];
    }

    public override void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, double dt)
    {
        base.Update(viewport, player, mouseState, keyboardState, dt);
        if (!Active)
            return;

        foreach (var b in buttons)
            b.Update(mouseState);
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
        int menuHeight = (int)(descSize.Y + hintSize.Y + headlineSize.Y + buttons[0].GetRect().Height + 80);
        int menuWidth = (int)Math.Max(descSize.X + 30, buttons.Count * buttons[0].GetRect().Width + 20 * (buttons.Count - 1) + 40);
        Rectangle menuBackground = new Rectangle(
            Viewport.Width / 4 - (menuWidth - Viewport.Width / 2) / 2, Viewport.Height / 4,
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


        float startY = hintPos.Y + hintSize.Y + 30;
        int totalWidth = buttons.Count * buttons[0].GetRect().Width + 20 * (buttons.Count-1);
        
        float startX = Viewport.Width / 2 - totalWidth / 2;

        for (int i = 0; i < buttons.Count; i++)
        {
            buttons[i].Position = new Vector2(startX, startY);
            buttons[i].Draw(spriteBatch);
            startX += buttons[i].GetRect().Width + 20;
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
            StatTypes.ATTACK_SPEED => "Čeština",
            StatTypes.MOVEMENT_SPEED => "Tělocvik",
            _ => statType.ToString()
        };
    }

    public void OpenMenu(Player player)
    {
        GenerateStatOptions(player);
        Active = true;
    }
}
