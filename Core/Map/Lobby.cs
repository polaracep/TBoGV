using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

public class Lobby : IDraw
{
    private Tile[,] LobbyMap;
    private Tile[,] LobbyDecorations;
    private List<Entity> LobbyEntities;
    private Vector2 Size = new Vector2(13);

    public Lobby()
    {
        GenerateLobby();

    }

    private void GenerateLobby()
    {
        for (int x = 0; x < LobbyMap.GetLength(0); x++)
            for (int y = 0; y < LobbyMap.GetLength(1); y++)
            {
                LobbyMap[x, y] = new TileFloor(FloorTypes.LOBBY);
            }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        int tS = (int)Tile.GetSize().X;
        for (int x = 0; x < LobbyMap.GetLength(0); x++)
            for (int y = 0; y < LobbyMap.GetLength(1); y++)
            {
                spriteBatch.Draw(LobbyMap[x, y].Sprite, new Vector2(x * tS, y * tS), Color.White);
            }
        for (int x = 0; x < LobbyDecorations.GetLength(0); x++)
            for (int y = 0; y < LobbyDecorations.GetLength(1); y++)
            {
                spriteBatch.Draw(LobbyDecorations[x, y].Sprite, new Vector2(x * tS, y * tS), Color.White);
            }
    }
}