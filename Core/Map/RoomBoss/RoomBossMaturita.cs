using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomBossMaturita : RoomBoss
{
	public Question currentQuestion = null;
	protected static Vector2 dimensions = new Vector2(11);
	protected override EnemyBoss Boss { get; set; } = new BossMaturita();

	public RoomBossMaturita(Player p) : base(dimensions, p) { }

	public override void Generate()
	{
		base.Generate();
		Reset();
	}

	public override void Reset()
	{
		ClearRoom();
		ClearAnswers();
		GenerateDecor();
	}

	protected void ClearAnswers()
	{
		for (int x = 0; x < dimensions.X; x++)
			for (int y = 0; y < dimensions.Y; y++)
			{
				//if (Decorations[x, y] is TileAnswer || Decorations[x, y] is TileTest)
				Decorations[x, y] = null;
			}
	}
	protected override void GenerateDecor()
	{
		currentQuestion = QuestionManager.GetNewQuestion("cestina");
		AddDecoTile(dimensions / 2, new TileTest(currentQuestion));

		while (!AddDecoTile(new Vector2(Random.Shared.Next(1, (int)dimensions.X - 1), Random.Shared.Next(1, (int)dimensions.Y - 1)), new TileAnswer(0))) ;
		while (!AddDecoTile(new Vector2(Random.Shared.Next(1, (int)dimensions.X - 1), Random.Shared.Next(1, (int)dimensions.Y - 1)), new TileAnswer(1))) ;
		while (!AddDecoTile(new Vector2(Random.Shared.Next(1, (int)dimensions.X - 1), Random.Shared.Next(1, (int)dimensions.Y - 1)), new TileAnswer(2))) ;
		while (!AddDecoTile(new Vector2(Random.Shared.Next(1, (int)dimensions.X - 1), Random.Shared.Next(1, (int)dimensions.Y - 1)), new TileAnswer(3))) ;

	}

	public void Answer(int questionIndex)
	{
		if (currentQuestion.CheckAnswer(currentQuestion.Answers[questionIndex]))
		{
			// Odpoved spravna
			Drops.Add(new ItemVysvedceni(dimensions * 25));
			GenerateExit();
		}
		else
		{
			Reset();
		}

	}
}

