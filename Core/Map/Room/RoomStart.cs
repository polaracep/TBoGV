using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TBoGV;

public class RoomStart : Room, IDraw
{
    public RoomStart(Vector2 dim, Player p) : base(dim, p) { }
    public RoomStart(Player p) : base((7, 9, 12), p) { }

    protected override List<Enemy> validEnemies { get; set; } = [];

    public override void Generate()
    {
        base.GenerateBase(FloorTypes.BASIC, WallTypes.BASIC, DoorTypes.BASIC);
#if DEBUG
        this.AddDecoTile(this.Dimensions / 2, new TileHeal());
#endif
        this.GenerateEnemies(0);

        player.Position = this.GetTileWorldPos(Vector2.One);
    }


}