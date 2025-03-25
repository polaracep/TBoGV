using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

public abstract class RoomBoss : Room
{
	public RoomBoss(Vector2 dimensions, Player p) : base(dimensions, p) { }

	// this won't be used, bosses have a different way of generating
	protected override List<Enemy> validEnemies { get; set; } = [];
	protected abstract EnemyBoss Boss { get; set; }

	public override void Generate()
	{
		GenerateBase();
		GenerateEnemies();
		IsGenerated = true;
	}

	protected override void GenerateBase()
	{
		ClearRoom();

		Floor.GenerateFilledRectangleWRotation(
			new Rectangle(0, 0, (int)Dimensions.X, (int)Dimensions.Y),
			new TileFloor(FloorTypes.LOBBY),
			new TileWall(WallTypes.HALLWAY_ORANGE),
			new TileWall(WallTypes.HALLWAY_ORANGE_CORNER)
		);

		GenerateDoors(DoorTypes.BASIC);

		GenerateDecor();

	}
	protected override void GenerateEnemies()
	{
		if (Boss == null)
			throw new NullReferenceException("Provide a boss.");

		Random rand = new Random();
		while (true)
		{
			Vector2 spawnPos = new Vector2(rand.Next((int)Dimensions.X - 2) + 1, rand.Next((int)Dimensions.Y - 2) + 1) * 50;

			if (Doors.Any(d => (d.DoorTpPosition - spawnPos).Length() < 250))
				continue;

			if (!ShouldCollideAt(new Rectangle(spawnPos.ToPoint(), Boss.Size.ToPoint())))
			{
				Boss.Position = spawnPos;
				AddEnemy(Boss);
				break;
			}
		}
	}
	protected static Texture2D SpriteIcon = TextureManager.GetTexture("bossIcon");
	public override void DrawMinimapIcon(SpriteBatch spriteBatch, Vector2 position, float scale = 2, bool active = false)
	{
		base.DrawMinimapIcon(spriteBatch, position, scale, active);
		int width = (int)(IconBaseSize.X * scale);
		int height = (int)(IconBaseSize.Y * scale);

		if (!IsGenerated)
			spriteBatch.Draw(SpriteIcon, position + (new Vector2(width, height) - new Vector2(SpriteIcon.Width, SpriteIcon.Height)) / 2, new Color(110, 110, 110));
		else
			spriteBatch.Draw(SpriteIcon, position + (new Vector2(width, height) - new Vector2(SpriteIcon.Width, SpriteIcon.Height)) / 2, Color.White);
	}
}

