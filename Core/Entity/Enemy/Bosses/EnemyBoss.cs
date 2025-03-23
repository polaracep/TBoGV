using System.Collections.Generic;
using System;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public abstract class EnemyBoss : Enemy
{
	public enum BossPhases : int;
	protected string Name { get; set; }
	public override List<Item> Drop(int looting)
	{
		Random random = new Random();
		List<Item> droppedItems = new List<Item>();

		// Define min and max coins to drop
		int minCoins = 3;
		int maxCoins = 7 + looting;

		// Drop chance calculation
		int dropChance = 50 / (looting + 1);
		for (int i = 0; i < minCoins; i++)
		{
			droppedItems.Add(new Coin(Position + Size / 2));
		}
		int coinCount = random.Next(0, maxCoins - minCoins + 1);
		for (int i = 0; i < coinCount; i++)
		{
			if (random.Next(0, 100) >= dropChance)
				droppedItems.Add(new Coin(Position + Size / 2));
		}
		return droppedItems;
	}
	public virtual void DrawHealthBar(SpriteBatch spriteBatch, Vector2 screenSize)
	{
		int barWidth = 300;
		int barHeight = 10;
		SpriteFont font = FontManager.GetFont("Arial16");
		Vector2 textSize = font.MeasureString(Name);
		Vector2 textPosition = new Vector2((screenSize.X - textSize.X) / 2, 12*screenSize.Y/100f );
		Vector2 barPosition = new Vector2((screenSize.X - barWidth) / 2, textPosition.Y + textSize.Y + 5);

		spriteBatch.Draw(SpriteWhiteSquare, new Rectangle(barPosition.ToPoint(), new Point(barWidth, barHeight)), Color.Black);

		float healthPercent = (float)Hp / MaxHp;
		int healthBarWidth = (int)(barWidth * healthPercent);

		spriteBatch.Draw(SpriteWhiteSquare, new Rectangle(barPosition.ToPoint(), new Point(healthBarWidth, barHeight)), Color.Green);

		spriteBatch.Draw(SpriteWhiteSquare, new Rectangle(new Point(barPosition.ToPoint().X + healthBarWidth, barPosition.ToPoint().Y), new Point(barWidth - healthBarWidth, barHeight)), Color.Red);

		spriteBatch.DrawString(font, Name, textPosition, Color.White);
	}
    public override void InitStats(int difficulty)
    {
        base.InitStats(difficulty);
        XpValue = 50 + difficulty * 10;
    }
}

