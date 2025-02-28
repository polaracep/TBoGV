using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomEmpty : Room, IDraw
{
    public RoomEmpty(Vector2 dimensions, Player p) : base(dimensions, p) { }

    public override void Generate()
    {
        base.GenerateBase(FloorTypes.BASIC, WallTypes.BASIC, DoorTypes.BASIC);
        // this.AddDecorationTile(new TileHeal(), new Vector2(5, 5));
        this.GenerateEnemies();
    }

    protected override void GenerateEnemies()
    {
        for (int i = 1; i <= 5; i++)
        {
            this.AddEnemy(new EnemyZdena(new Vector2(Tile.GetSize().X * i, Tile.GetSize().Y)));
        }
    }

}