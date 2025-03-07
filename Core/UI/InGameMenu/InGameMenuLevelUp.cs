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
    private static SpriteFont SmallFont = FontManager.GetFont("Arial8");
    private static SpriteFont MiddleFont = FontManager.GetFont("Arial12");
    private static SpriteFont LargerFont = FontManager.GetFont("Arial16");
    private List<Button> buttons = new();
    private string chosenDescription = "";
    private readonly List<string> descriptions = new List<string>
    {
"Odložil jsi na chvíli telefon a proto jsi se soustředil",
"Dlouho jsi neposlouchal Sigma boy",
"Uvařil jsi neohlášený test",
"Dával jsi pozor alespoň 5 minut v kuse",
"Rozbil se ti endless scroll",
"Dneska ti nikdo neposlal reel",
"Přinesl jsi si věci do školy",
"Dneska se ti nezdály sny o neexistujících lego dílcích",
"Schovánek byl cooked víc než ty na TFY",
"Dostal jsi přidáno na obědě"
    };

    // Extra text to implicitly indicate that the option improves understanding
    private readonly string understandingHint = "Volbou se ti zlepší pochopení předmětu";

    private const int Padding = 10;

    public InGameMenuLevelUp(Viewport viewport, Player player)
    {
        Viewport = viewport;
        SpriteBackground = TextureManager.GetTexture("blackSquare");
        GenerateStatOptions(player);
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
            }));
        }
        foreach (var b in buttons)
            b.SetSize(new Vector2(110, b.GetRect().Height));

        chosenDescription = descriptions[random.Next(descriptions.Count)];
    }

    public override void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, double dt)
    {
        base.Update(viewport, player, mouseState, keyboardState, dt);

        foreach (var b in buttons)
            b.Update(mouseState);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
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
        int totalWidth = buttons.Count * buttons[0].GetRect().Width + 20 * (buttons.Count - 1);

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
}
