using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV;

class UI : IDraw
{
    List<Heart> hearts;
    static SpriteFont Font;
    static SpriteFont MiddleFont = FontManager.GetFont("Arial12");
    static Texture2D SpriteCoin;
    static Texture2D SpriteXpBar;
    protected Vector2 screenSize;

    int Coins;
    float Xp;
    int MaxXp;
    const int MaxHeartsPerRow = 5;
    protected List<Effect> Effects = new List<Effect>();
    protected EnemyBoss Boss;
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

    public void Update(Player player, List<Enemy> enemies, GraphicsDeviceManager graphics)
    {
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

        Coins = player.Coins;
        Xp = (int)player.Xp;
        MaxXp = player.XpForLevel();
        Effects = player.Inventory.Effects;
        screenSize = new Vector2(graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height);
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

        string timeElapsedText = GameManager.GetPlaytime().ToString("HH:mm:ss.fff");

        string hotbarText = pololetiText + '\n' + yearText + '\n' + failedTimesText + '\n' + timeElapsedText;

        spriteBatch.DrawString(MiddleFont, hotbarText, new Vector2(30, screenSize.Y - MiddleFont.MeasureString(hotbarText).Y - 30), Color.White);
        // spriteBatch.DrawString(MiddleFont, failedTimesText, new Vector2(30, screenSize.Y - MiddleFont.MeasureString(failedTimesText).Y - 30), Color.White);
        // spriteBatch.DrawString(MiddleFont, yearText, new Vector2(30, screenSize.Y - MiddleFont.MeasureString(yearText).Y - MiddleFont.MeasureString(failedTimesText).Y - 30), Color.White);
        // spriteBatch.DrawString(MiddleFont, pololetiText, new Vector2(30, screenSize.Y - MiddleFont.MeasureString(yearText).Y - MiddleFont.MeasureString(pololetiText).Y - MiddleFont.MeasureString(failedTimesText).Y - 30), Color.White);
        if (Boss != null)
        {
            Boss.DrawHealthBar(spriteBatch, screenSize);
        }
    }
}
