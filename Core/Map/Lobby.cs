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


    private void GenerateLobby()
    {
        Floor = new Tile[(int)Dimensions.X, (int)Dimensions.Y];
        Decorations = new Tile[(int)Dimensions.X, (int)Dimensions.Y];

        Floor.GenerateFilledRectangleWRotation(
            new Rectangle(0, 0, (int)Dimensions.X, (int)Dimensions.Y),
            new TileFloor(FloorTypes.LOBBY),
            new TileWall(WallTypes.LOBBY),
            new TileWall(WallTypes.LOBBY_CORNER)
        );
        player.Position = this.GetTileWorldPos(Vector2.One);
        IsGenerated = true;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        if (!IsGenerated)
            GenerateLobby();

        int tS = (int)Tile.GetSize().X;
        Vector2 origin = new Vector2(25, 25);
        for (int x = 0; x < Floor.GetLength(0); x++)
            for (int y = 0; y < Floor.GetLength(1); y++)
            {
                Tile t = Floor[x, y];
                if (t != null)
                    spriteBatch.Draw(t.Sprite, new Vector2(x * tS, y * tS) + origin, null, Color.White, t.Rotation, origin, 1f, SpriteEffects.None, 0f);

                t = Decorations[x, y];
                if (t != null)
                    spriteBatch.Draw(t.Sprite, new Vector2(x * tS, y * tS) + origin, null, Color.White, t.Rotation, origin, 1f, SpriteEffects.None, 0f);
            }

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