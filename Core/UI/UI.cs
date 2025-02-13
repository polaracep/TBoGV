﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV.Core
{
    internal class UI : IDraw
    {
        List<Heart> hearths;
        ItemContainer itemContainer;
        static SpriteFont Font;
        static Texture2D SpriteCoin;
        static Texture2D SpriteXpBar;

        int Coins;
        int Xp;
        int MaxXp;
        const int MaxHeartsPerRow = 5;

        public UI()
        {
            hearths = new List<Heart>();
            Font = FontManager.GetFont("font");
            SpriteCoin = TextureManager.GetTexture("coin");
            SpriteXpBar = TextureManager.GetTexture("whiteSquare");
            Coins = 0;
            Xp = 0;
            MaxXp = 100;
        }

        public void Update(Player player, GraphicsDeviceManager graphics)
        {
            if (player.MaxHp != hearths.Count)
            {
                hearths.Clear();
                for (int i = 0; i < player.MaxHp; i++)
                    hearths.Add(new Heart());
            }

            Vector2 screenOffset = new Vector2(20, 20);
            for (int i = 0; i < hearths.Count; i++)
            {
                hearths[i].Broken = i >= player.Hp;
                int row = i / MaxHeartsPerRow;
                int col = i % MaxHeartsPerRow;
                hearths[i].Position = screenOffset + new Vector2((Heart.Size.X + 5) * col, (Heart.Size.Y + 3) * row);
            }

            Coins = player.Coins;
            Xp = player.Xp;
            MaxXp = player.XpForLevel();

            Vector2 screenSize = new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth, GraphicsDeviceManager.DefaultBackBufferHeight);
            itemContainer = player.ItemContainers[player.selectedItemIndex];
            itemContainer.SetPosition( new Vector2(screenSize.X - itemContainer.Size.X - 20, 20)); 

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < hearths.Count; i++)
                hearths[i].Draw(spriteBatch);

            // XP Bar
            Vector2 xpBarPosition = new Vector2(GraphicsDeviceManager.DefaultBackBufferWidth / 2 - 100, 20);
            int xpBarWidth = 200;
            int xpBarHeight = 5;
            float xpPercentage = Math.Min((float)Xp / MaxXp, 1);
            Rectangle xpBarBackground = new Rectangle((int)xpBarPosition.X, (int)xpBarPosition.Y, xpBarWidth, xpBarHeight);
            Rectangle xpBarFill = new Rectangle((int)xpBarPosition.X, (int)xpBarPosition.Y, (int)(xpBarWidth * xpPercentage), xpBarHeight);

            spriteBatch.Draw(SpriteXpBar, xpBarBackground, Color.Gray); // Background
            spriteBatch.Draw(SpriteXpBar, xpBarFill, new Color(15, 209, 209)); // Filled XP

            string xpText = $"XP: {Xp}/{MaxXp}";
            Vector2 xpTextPosition = new Vector2(xpBarPosition.X + xpBarWidth / 2 - Font.MeasureString(xpText).X / 2, xpBarPosition.Y + xpBarHeight + 5);
            spriteBatch.DrawString(Font, xpText, xpTextPosition, Color.White);

            // Coin display below the hearts
            int heartRows = (int)Math.Ceiling((double)hearths.Count / MaxHeartsPerRow);
            Vector2 coinPosition = new Vector2(20, 35 + heartRows * (Heart.Size.Y + 3));
            string coinText = $"{Coins}";
            spriteBatch.Draw(SpriteCoin, new Rectangle((int)coinPosition.X, (int)coinPosition.Y, 30, 30), Color.White);
            itemContainer.Draw(spriteBatch);
            spriteBatch.DrawString(Font, coinText, new Vector2((int)coinPosition.X + 30, (int)coinPosition.Y), Color.Yellow);
        }
    }
}
