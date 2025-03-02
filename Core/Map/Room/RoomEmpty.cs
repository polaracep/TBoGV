using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class RoomEmpty : Room, IDraw
{
    public RoomEmpty(Vector2 dimensions, Player p) : base(dimensions, p) { }
    public RoomEmpty(Player p, List<Entity> entityList) : base((5, 9, 11), p, entityList) { }
    public RoomEmpty(Player p) : base((5, 9, 11), p, null) { }
    public RoomEmpty(Vector2 dimensions, Player p, List<Entity> entityList) : base(dimensions, p, entityList) { }

    protected override List<Enemy> validEnemies { get; set; } = [
        new EnemyLol(),
        new EnemyPolhreich()
    ];

    public override void Generate()
    {
        base.GenerateBase(FloorTypes.BASIC, WallTypes.BASIC, DoorTypes.BASIC);
        this.GenerateEnemies(20);
    }

}