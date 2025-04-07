using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV;

class UI : IDraw
{
    List<Heart> hearts;
    static SpriteFont Font;
    static SpriteFont MiddleFont = FontManager.GetFont("Arial12");
    static SpriteFont LargeFont = FontManager.GetFont("Arial16");
    static SpriteFont LargestFont = FontManager.GetFont("Arial24");
    static Texture2D SpriteCoin;
    static Texture2D SpriteXpBar;
    protected Vector2 screenSize;

    private char[] questionLetters = ['A', 'B', 'C', 'D'];

    int Coins;
    float Xp;
    int MaxXp;
    const int MaxHeartsPerRow = 5;
    protected List<Effect> Effects = new List<Effect>();
    protected EnemyBoss Boss;

    /// <summary>
    /// used only for the question, answers ignored
    /// </summary>
    protected Question activeQuestion;
    protected double questionFadeElapsed = 0;
    protected int questionFadeMax = 1000;
    // abych te videl, musim te spatrit
    // abych se rozdal, musim si spatrit!
    protected float questionAlpha = 1f;

    public Viewport viewport { get; set; }

    public UI()
    {
        hearts = new List<Heart>();
        Font = FontManager.GetFont("font");
        SpriteCoin = TextureManager.GetTexture("coin");
        SpriteXpBar = TextureManager.GetTexture("whiteSquare");
        Coins = 0;
        Xp = 0;
        MaxXp = 10;
    }

    public void Update(Player player, List<Enemy> enemies, GraphicsDeviceManager graphics, double dt)
    {
        Coins = player.Coins;
        Xp = (int)player.Xp;
        MaxXp = player.XpForLevel();
        Effects = player.Inventory.Effects;
        screenSize = new Vector2(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
        viewport = graphics.GraphicsDevice.Viewport;

        if (player.MaxHp != hearts.Count)
        {
            hearts.Clear();
            for (int i = 0; i < player.MaxHp; i++)
                hearts.Add(new Heart());
        }

        Vector2 screenOffset = new Vector2(20, 20);
        for (int i = 0; i < hearts.Count; i++)
        {
            hearts[i].Broken = i >= player.Hp;
            int row = i / MaxHeartsPerRow;
            int col = i % MaxHeartsPerRow;
            hearts[i].Position = screenOffset + new Vector2((Heart.Size.X + 5) * col, (Heart.Size.Y + 3) * row);
        }
        bool isboss = false;
        foreach (var e in enemies)
        {
            if (e is EnemyBoss)
            {
                isboss = true;
                Boss = (EnemyBoss)e;
            }
        }
        if (!isboss)
            Boss = null;

        if (player.activeQuestion != null)
            activeQuestion = player.activeQuestion;

        if (player.questionUpdated)
        {
            questionFadeElapsed = 0;
            player.questionUpdated = false;
        }
        if (activeQuestion != null)
        {
            questionFadeElapsed += dt;
            float progress = MathHelper.Clamp((float)questionFadeElapsed / questionFadeMax, 0f, 1f);
            if (progress == 1f)
            {
                player.SetQuestion(null);
                activeQuestion = null;
                questionFadeElapsed = 0;
                return;
            }

            questionAlpha = 1f - progress;
        }


    }

    public void Draw(SpriteBatch spriteBatch)
    {
        for (int i = 0; i < hearts.Count; i++)
            hearts[i].Draw(spriteBatch);

        // XP Bar
        Vector2 xpBarPosition = new Vector2(screenSize.X / 2 - 100, 20);
        int xpBarWidth = 200;
        int xpBarHeight = 5;
        float xpPercentage = Math.Min((float)Xp / MaxXp, 1);
        Rectangle xpBarBackground = new Rectangle((int)xpBarPosition.X, (int)xpBarPosition.Y, xpBarWidth, xpBarHeight);
        Rectangle xpBarFill = new Rectangle((int)xpBarPosition.X, (int)xpBarPosition.Y, (int)(xpBarWidth * xpPercentage), xpBarHeight);

        spriteBatch.Draw(SpriteXpBar, xpBarBackground, Color.Gray); // Background
        spriteBatch.Draw(SpriteXpBar, xpBarFill, new Color(15, 209, 209)); // Filled XP

        string xpText = $"XP: {Xp}/{MaxXp}";
        Vector2 xpTextPosition = new Vector2(xpBarPosition.X + xpBarWidth / 2 - MiddleFont.MeasureString(xpText).X / 2, xpBarPosition.Y + xpBarHeight + 5);
        spriteBatch.DrawString(MiddleFont, xpText, xpTextPosition, Color.White);

        // Coin display below the hearts
        int heartRows = (int)Math.Ceiling((double)hearts.Count / MaxHeartsPerRow);
        Vector2 coinPosition = new Vector2(20, 35 + heartRows * (Heart.Size.Y + 3));
        string coinText = $"{Coins}";
        spriteBatch.Draw(SpriteCoin, new Rectangle((int)coinPosition.X, (int)coinPosition.Y, 30, 30), Color.White);
        spriteBatch.DrawString(Font, coinText, new Vector2((int)coinPosition.X + 40, (int)coinPosition.Y), Color.Yellow);
        Vector2 effectStartPosition = new Vector2(screenSize.X - 100, 30);
        for (int i = 0; i < Effects.Count; i++)
        {
            // Position each effect in the column, spacing them vertically.
            Effects[i].Position = effectStartPosition;
            Effects[i].IconDraw(spriteBatch);
            effectStartPosition += new Vector2(
                0,
                Effects[i].SpriteSize.X + 5
            );
        }
        string failedTimesText = Storyline.FailedTimes.ToString() + "/3 Propadnutí";
        string pololetiText = Storyline.CurrentLevelNumber % 2 == 1 ? "1. " : "2. ";
        pololetiText += "Pololetí";

        string yearText;
        switch ((Storyline.CurrentLevelNumber - 1) / 2)
        {
            case 0:
                yearText = "Prima";
                break;
            case 1:
                yearText = "Sekunda";
                break;
            case 2:
                yearText = "Tercie";
                break;
            case 3:
                yearText = "Kvarta";
                break;
            case 4:
                yearText = "Kvinta";
                break;
            case 5:
                yearText = "Sexta";
                break;
            case 6:
                yearText = "Septima";
                break;
            case 7:
                yearText = "Oktava";
                break;
            default:
                yearText = (Storyline.CurrentLevelNumber - 1) / 2 + 1 + ". Ročník";
                break;
        }
        if (Storyline.CurrentLevelNumber == 0)
        {
            pololetiText = "";
            yearText = "";
            failedTimesText = "";
        }
        spriteBatch.DrawString(MiddleFont, failedTimesText, new Vector2(30, screenSize.Y - MiddleFont.MeasureString(failedTimesText).Y - 30), Color.White);
        spriteBatch.DrawString(MiddleFont, yearText, new Vector2(30, screenSize.Y - MiddleFont.MeasureString(yearText).Y - MiddleFont.MeasureString(failedTimesText).Y - 30), Color.White);
        spriteBatch.DrawString(MiddleFont, pololetiText, new Vector2(30, screenSize.Y - MiddleFont.MeasureString(yearText).Y - MiddleFont.MeasureString(pololetiText).Y - MiddleFont.MeasureString(failedTimesText).Y - 30), Color.White);
        if (Boss != null)
        {
            Boss.DrawHealthBar(spriteBatch, screenSize);
        }
        if (activeQuestion != null)
        {
            string question = activeQuestion.QuestionText;
            List<string> answers = activeQuestion.Answers;

            int x0 = (int)((screenSize.X - LargestFont.MeasureString(question).X) / 2);
            int y0 = (int)(35 * screenSize.Y / 100);

            int w = (int)Math.Max(
                LargestFont.MeasureString(question).X,
                LargeFont.MeasureString(answers.MaxBy(x => x.Length)).X
            );

            int h = (10 + 5 * answers.Count) * viewport.Height / 100;

            spriteBatch.Draw(InGameMenu.SpriteBackground,
                new Rectangle(x0 - 20, y0 - 20, w + 40, h + 40),
                new Color(0, 0, 0, (int)(255 * 0.25)) * questionAlpha);

            spriteBatch.DrawString(LargestFont, question, new Vector2(x0, y0), Color.White * questionAlpha);
            for (int i = 0; i < answers.Count; i++)
            {
                string ans = questionLetters[i] + ") " + answers[i];
                spriteBatch.DrawString(LargeFont, ans, new Vector2((screenSize.X - LargeFont.MeasureString(ans).X) / 2, (45 + 5 * i) * viewport.Height / 100), Color.White * questionAlpha);
            }
        }
    }
}
