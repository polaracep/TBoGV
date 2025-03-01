using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TBoGV;

public class RoomStart : Room, IDraw
{
    public RoomStart(Vector2 dimensions, Player p) : base(dimensions, p, null) { }

    protected override List<Enemy> validEnemies { get; set; } = [
        new EnemyCat()
    ];

    public override void Generate()
    {
        base.GenerateBase(FloorTypes.BASIC, WallTypes.BASIC, DoorTypes.BASIC);
        this.AddDecoTile(this.Dimensions / 2, new TileHeal());
        this.GenerateEnemies(20);

        player.Position = this.GetTileWorldPos(Vector2.One);
    }

}