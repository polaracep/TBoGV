using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

public class Lobby : Place
{

    public Lobby(Player p)
    {
        this.player = p;
        this.Dimensions = new Vector2(13, 9);
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

        AddDecoTile(Dimensions / 2, new TileTreasure());

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
        this.Entities.Add(new EntitySarka(GetTileWorldPos(new Vector2(3, 3))));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        foreach (var e in Entities)
            spriteBatch.Draw(e.GetSprite(), e.Position, Color.White);

    }

    public override void Update(GameTime gameTime)
    {
    }

    public override void Reset()
    {
    }
}