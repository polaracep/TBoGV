using System.Collections.Generic;
using System;

namespace TBoGV;

public abstract class EnemyBoss : Enemy
{
	public enum BossPhases : int;
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
}

