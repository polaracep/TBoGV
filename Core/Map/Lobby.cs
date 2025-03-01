using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

public class Lobby : Place
{

    public static Vector2 SpawnPos;
    public Lobby(Player p)
    {
        this.player = p;
        this.Dimensions = new Vector2(14, 6);
        SpawnPos = new Vector2(Dimensions.X - 2, Dimensions.Y / 2);
    }


    public override void Generate()
    {
        Floor = new Tile[(int)Dimensions.X, (int)Dimensions.Y];
        Decorations = new Tile[(int)Dimensions.X, (int)Dimensions.Y];

        Floor.GenerateFilledRectangleWRotation(
            new Rectangle(0, 0, (int)Dimensions.X, (int)Dimensions.Y),
            new TileFloor(FloorTypes.LOBBY),
            new TileWall(WallTypes.LOBBY),
            new TileWall(WallTypes.LOBBY_CORNER)
        );

        AddDecoTile(new Vector2(3, 2), new TileTreasure());

        // Spawnpos + o jedno doprava
        AddDecoTile(SpawnPos + Vector2.UnitX, new TileStart(MathHelper.PiOver2));

        // lol
        AddDecoTile(new Vector2(1, Dimensions.Y - 3), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G));
        AddDecoTile(new Vector2(1, Dimensions.Y - 2), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G));
        AddDecoTile(new Vector2(2, Dimensions.Y - 3), new TileDecoration(true, DecorationTypes.TABLE_CAFETERIA));
        AddDecoTile(new Vector2(2, Dimensions.Y - 2), new TileDecoration(true, DecorationTypes.TABLE_CAFETERIA));
        AddDecoTile(new Vector2(3, Dimensions.Y - 3), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G, SpriteEffects.FlipHorizontally));
        AddDecoTile(new Vector2(3, Dimensions.Y - 2), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G, SpriteEffects.FlipHorizontally));

        AddDecoTile(new Vector2(5, Dimensions.Y - 3), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G));
        AddDecoTile(new Vector2(5, Dimensions.Y - 2), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G));
        AddDecoTile(new Vector2(6, Dimensions.Y - 3), new TileDecoration(true, DecorationTypes.TABLE_CAFETERIA));
        AddDecoTile(new Vector2(6, Dimensions.Y - 2), new TileDecoration(true, DecorationTypes.TABLE_CAFETERIA));
        AddDecoTile(new Vector2(7, Dimensions.Y - 3), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G, SpriteEffects.FlipHorizontally));
        AddDecoTile(new Vector2(7, Dimensions.Y - 2), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G, SpriteEffects.FlipHorizontally));

        AddDecoTile(new Vector2(9, Dimensions.Y - 3), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G));
        AddDecoTile(new Vector2(9, Dimensions.Y - 2), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G));
        AddDecoTile(new Vector2(10, Dimensions.Y - 3), new TileDecoration(true, DecorationTypes.TABLE_CAFETERIA));
        AddDecoTile(new Vector2(10, Dimensions.Y - 2), new TileDecoration(true, DecorationTypes.TABLE_CAFETERIA));
        AddDecoTile(new Vector2(11, Dimensions.Y - 3), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G, SpriteEffects.FlipHorizontally));
        AddDecoTile(new Vector2(11, Dimensions.Y - 2), new TileDecoration(false, DecorationTypes.CHAIR_CAFETERIA_G, SpriteEffects.FlipHorizontally));
        player.Position = this.GetTileWorldPos(Vector2.One);
        GenerateEntities();
        IsGenerated = true;
    }

    private void GenerateEntities()
    {
        this.Entities.Add(new EntitySarka(GetTileWorldPos(new Vector2(1, 0))));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        foreach (var e in Entities)
            spriteBatch.Draw(e.GetSprite(), e.Position, Color.White);
        foreach (var i in Drops)
            i.Draw(spriteBatch);
    }

    public override void Update(double dt)
    {
    }

    public override void Reset()
    {
        Drops.Clear();
    }
}