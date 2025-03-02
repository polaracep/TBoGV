using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomEmpty : Room, IDraw
{
    public RoomEmpty(Vector2 dimensions, Player p) : base(dimensions, p) { }
    public RoomEmpty(Player p, List<Entity> entityList) : base((5, 9, 11), p, entityList) { }
    public RoomEmpty(Vector2 dimensions, Player p, List<Entity> entityList) : base(dimensions, p, entityList) { }

    protected override List<Enemy> validEnemies { get; set; } = [
        new EnemyCat()
    ];

    public override void Generate()
    {
        base.GenerateBase(FloorTypes.BASIC, WallTypes.BASIC, DoorTypes.BASIC);
        // this.AddDecorationTile(new TileHeal(), new Vector2(5, 5));
        this.GenerateEnemies(20);
    }

}