using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomEmpty : Room, IDraw
{
    public RoomEmpty(Vector2 dimensions, Vector2 pos, Player p) : base(dimensions, pos, p) { }
    public RoomEmpty(Vector2 pos, Directions dir, Player p) : base(Vector2.Zero, pos, p) { }
    public RoomEmpty(Vector2 dimensions, Player p) : base(dimensions, Vector2.One, p) { }
    //public RoomEmpty(Player p) :


    public override void GenerateRoom()
    {
        base.GenerateRoomBase();
        // this.AddDecorationTile(new TileHeal(), new Vector2(5, 5));
        this.GenerateEnemies();

        player.Position = this.GetTilePos(Vector2.One);
    }

    protected override void GenerateEnemies()
    {
        for (int i = 1; i <= 5; i++)
        {
            this.AddEnemy(new EnemyZdena(new Vector2(Tile.GetSize().X * i, Tile.GetSize().Y)));
        }
    }

}