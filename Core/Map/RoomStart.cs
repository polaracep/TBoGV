using Microsoft.Xna.Framework;
using TBoGV;

public class RoomStart : Room, IDraw
{
    public RoomStart(Vector2 dimensions, Vector2 pos, Player p) : base(dimensions, pos, p) { }
    public RoomStart(Vector2 dimensions, Player p) : base(dimensions, Vector2.Zero, p) { }

    public override void GenerateRoom()
    {
        base.GenerateRoomBase();
        this.AddDecorationTile(new TileHeal(), this.Dimensions / 2);
        this.GenerateEnemies();

        player.Position = this.GetTilePos(Vector2.One);
    }

    protected override void GenerateEnemies() { }

}