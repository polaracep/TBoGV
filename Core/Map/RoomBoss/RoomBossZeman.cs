using System;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomBossZeman : RoomBoss
{
	protected static Vector2 dimensions = new Vector2(18);
	public RoomBossZeman(Player p) : base(dimensions, p) { }

	protected override EnemyBoss Boss { get; set; } = new BossZeman();
    protected override void GenerateDecor()
    {
        Random rand = new Random();
        DecorationTypes[] tableTypes =
        {
            DecorationTypes.PUB_TABLE1,
            DecorationTypes.PUB_TABLE2,
            DecorationTypes.PUB_TABLE3,
            DecorationTypes.PUB_TABLE4,
            DecorationTypes.PUB_TABLE5,
            DecorationTypes.PUB_TABLE6,
            DecorationTypes.PUB_TABLE7
        };

        int minX = 5, maxX = 12;
        int minY = 6, maxY = 10;

        for (int x = minX; x <= maxX; x++) 
        {
            for (int y = minY; y <= maxY; y += 2)
            {
                DecorationTypes tableType = tableTypes[rand.Next(tableTypes.Length)];
                AddDecoTile(new Vector2(x, y), new TileDecoration(true, tableType));
            }
        }

        // Adding SLOWZONE at the corners of the whole row structure
        AddDecoTile(new Vector2(minX - 1, minY - 1), new TileDecoration(false, DecorationTypes.PUB_SLOW_ZONE)); // Top-left corner
        AddDecoTile(new Vector2(minX, minY +1), new TileDecoration(false, DecorationTypes.PUB_SLOW_ZONE));
        AddDecoTile(new Vector2(minX, minY +3), new TileDecoration(false, DecorationTypes.PUB_SLOW_ZONE)); 
        AddDecoTile(new Vector2(maxX + 1, minY - 1), new TileDecoration(false, DecorationTypes.PUB_SLOW_ZONE)); // Top-right corner
        AddDecoTile(new Vector2(maxX , minY + 1), new TileDecoration(false, DecorationTypes.PUB_SLOW_ZONE));
        AddDecoTile(new Vector2(maxX, minY + 3), new TileDecoration(false, DecorationTypes.PUB_SLOW_ZONE));
        AddDecoTile(new Vector2(minX - 1, maxY + 1), new TileDecoration(false, DecorationTypes.PUB_SLOW_ZONE)); // Bottom-left corner
        AddDecoTile(new Vector2(maxX + 1, maxY + 1), new TileDecoration(false, DecorationTypes.PUB_SLOW_ZONE)); // Bottom-right corner
    }
    public bool IsSlowZone(Rectangle rect)
    {
        //experimental
        Vector2 tileSize = Tile.GetSize();

        // Calculate tile indices for the rectangle bounds
        int startX = Math.Max(0, rect.Left / (int)tileSize.X);
        int startY = Math.Max(0, rect.Top / (int)tileSize.Y);
        int endX = Math.Min((int)Dimensions.X - 1, rect.Right / (int)tileSize.X);
        int endY = Math.Min((int)Dimensions.Y - 1, rect.Bottom / (int)tileSize.Y);

        // Iterate through all tiles the rectangle covers
        for (int x = startX; x <= endX; x++)
        {
            for (int y = startY; y <= endY; y++)
            {
                Vector2 tileCoords = new Vector2(x, y);
                if (IsSlowZoneAtGrid(tileCoords))
                    return true; 
            }
        }
        return false;
    }
    public bool IsSlowZoneAtGrid(Vector2 Coords)
    {
        Tile td = Decorations[(int)Coords.X, (int)Coords.Y];
        if (td != null)
            return td.Sprite.Name == "Textures/empty";
        return false;
    }
}

