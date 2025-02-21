using Microsoft.Xna.Framework;
using TBoGV;

public static class RoomHelper
{
    public static void GenerateRectangleWRotation(this Tile[,] floor, Rectangle rect, Tile wall, Tile corner)
    {
        int x = rect.X;
        int y = rect.Y;
        for (int i = 0; i < rect.Width; i++)
        {
            Tile v = (Tile)wall.Clone();
            v.Rotation = 0;
            floor[i + x, y] = v;
            Tile t = (Tile)wall.Clone();
            t.Rotation = MathHelper.Pi;
            floor[i + x, y + rect.Height - 1] = t;
        }

        for (int i = 0; i < rect.Height; i++)
        {
            Tile v = (Tile)wall.Clone();
            v.Rotation = -MathHelper.PiOver2;
            floor[x, i + y] = v;
            Tile t = (Tile)wall.Clone();
            t.Rotation = MathHelper.PiOver2;
            floor[x + rect.Width - 1, i + y] = t;
        }

        Tile _a = (Tile)corner.Clone();
        _a.Rotation = -MathHelper.PiOver2;
        floor[x, y] = _a;
        Tile _b = (Tile)corner.Clone();
        _b.Rotation = 0;
        floor[x + rect.Width - 1, y] = _b;
        Tile _c = (Tile)corner.Clone();
        _c.Rotation = MathHelper.Pi;
        floor[x, y + rect.Height - 1] = _c;
        Tile _d = (Tile)corner.Clone();
        _d.Rotation = MathHelper.PiOver2;
        floor[x + rect.Width - 1, y + rect.Height - 1] = _d;

    }
    public static void GenerateRectangle(this Tile[,] floor, Rectangle rect, Tile wall, Tile corner)
    {
        int x = rect.X;
        int y = rect.Y;
        for (int i = 0; i < rect.Width; i++)
        {
            floor[i + x, y] = (Tile)wall.Clone();
            floor[i + x, y + rect.Height - 1] = (Tile)wall.Clone();
        }

        for (int i = 0; i < rect.Height; i++)
        {
            floor[x, i + y] = (Tile)wall.Clone();
            floor[x + rect.Height - 1, i + y] = (Tile)wall.Clone();
        }

        floor[x, y] = (Tile)corner.Clone();
        floor[x + rect.Width - 1, y] = (Tile)corner.Clone();
        floor[x, y + rect.Height - 1] = (Tile)corner.Clone();
        floor[x + rect.Width - 1, y + rect.Height - 1] = (Tile)corner.Clone();
    }
    public static void GenerateFilledRectangle(this Tile[,] floor, Rectangle rect, Tile fill, Tile wall)
    {
        GenerateRectangle(floor, rect, wall, wall);

        for (int i = 1; i < rect.Width - 1; i++)
            for (int j = 1; j < rect.Height - 1; j++)
                floor[i + rect.X, j + rect.Y] = (Tile)fill.Clone();
    }
    public static void GenerateFilledRectangleWRotation(this Tile[,] floor, Rectangle rect, Tile fill, Tile wall, Tile corner)
    {
        int x = rect.X;
        int y = rect.Y;

        for (int i = 1; i < rect.Width - 1; i++)
            for (int j = 1; j < rect.Height - 1; j++)
                floor[x + i, y + j] = (Tile)fill.Clone();

        GenerateRectangleWRotation(floor, rect, wall, corner);
    }
}