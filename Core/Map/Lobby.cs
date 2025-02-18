using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

public class Lobby : IDraw
{
    private Tile[,] LobbyFloor;
    private Tile[,] LobbyDecorations;
    private List<Entity> LobbyEntities;
    private Vector2 Size = new Vector2(13);

    public Lobby()
    {
        GenerateLobby();
    }

    private void GenerateLobby()
    {
        LobbyFloor = new Tile[(int)Size.X, (int)Size.Y];
        LobbyDecorations = new Tile[(int)Size.X, (int)Size.Y];

        LobbyFloor.GenerateFilledRectangleWRotation(
            new Rectangle(0, 0, (int)Size.X, (int)Size.Y),
            new TileFloor(FloorTypes.LOBBY),
            new TileWall(WallTypes.LOBBY),
            new TileWall(WallTypes.LOBBY_CORNER)
        );

    }

    public void Draw(SpriteBatch spriteBatch)
    {
        int tS = (int)Tile.GetSize().X;
        for (int x = 0; x < LobbyFloor.GetLength(0); x++)
            for (int y = 0; y < LobbyFloor.GetLength(1); y++)
            {
                spriteBatch.Draw(LobbyFloor[x, y].Sprite, new Vector2(x * tS, y * tS), Color.White);
            }

        for (int x = 0; x < LobbyDecorations.GetLength(0); x++)
            for (int y = 0; y < LobbyDecorations.GetLength(1); y++)
            {
                spriteBatch.Draw(LobbyDecorations[x, y].Sprite, new Vector2(x * tS, y * tS), Color.White);
            }

        foreach (var e in LobbyEntities)
            spriteBatch.Draw(e.GetSprite(), e.Position, Color.White);

    }
}