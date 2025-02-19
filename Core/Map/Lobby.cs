using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

public class Lobby : Place
{
    private Vector2 Size = new Vector2(13);

    public Lobby()
    {
        GenerateLobby();
    }

    private void GenerateLobby()
    {
        Floor = new Tile[(int)Size.X, (int)Size.Y];
        Decorations = new Tile[(int)Size.X, (int)Size.Y];

        Floor.GenerateFilledRectangleWRotation(
            new Rectangle(0, 0, (int)Size.X, (int)Size.Y),
            new TileFloor(FloorTypes.LOBBY),
            new TileWall(WallTypes.LOBBY),
            new TileWall(WallTypes.LOBBY_CORNER)
        );

    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        int tS = (int)Tile.GetSize().X;
        for (int x = 0; x < Floor.GetLength(0); x++)
            for (int y = 0; y < Floor.GetLength(1); y++)
            {
                spriteBatch.Draw(Floor[x, y].Sprite, new Vector2(x * tS, y * tS), Color.White);
            }

        for (int x = 0; x < Decorations.GetLength(0); x++)
            for (int y = 0; y < Decorations.GetLength(1); y++)
            {
                spriteBatch.Draw(Decorations[x, y].Sprite, new Vector2(x * tS, y * tS), Color.White);
            }

        foreach (var e in Entities)
            spriteBatch.Draw(e.GetSprite(), e.Position, Color.White);

    }

    public override void Update(GameTime gameTime)
    {
    }
}