using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TBoGV;

public class RoomHallway : Room
{
    public RoomHallway(Vector2 dimensions, Player p) : base(dimensions, p) { }
    public RoomHallway(Player p) : base((7, 9, 17), p, null) { }
    public RoomHallway(Vector2 dimensions, Player p, List<Entity> entityList) : base(dimensions, p, entityList) { }

    protected override List<Enemy> validEnemies { get; set; } = [
        new EnemyJirka(),
        new EnemyCat()
    ];

    public override void Generate()
    {
        GenerateBase();
        GenerateEnemies();
        IsGenerated = true;
    }
    protected override void GenerateBase()
    {
        ClearRoom();

        Floor.GenerateFilledRectangleWRotation(
            new Rectangle(0, 0, (int)Dimensions.X, (int)Dimensions.Y),
            new TileFloor(FloorTypes.LOBBY),
            new TileWall(WallTypes.LOBBY),
            new TileWall(WallTypes.LOBBY_CORNER)
        );

        GenerateDoors(DoorTypes.BASIC);

        GenerateDecor();

    }
    protected override void GenerateEnemies()
    {
        GenerateEnemies(Storyline.Difficulty);
    }

    protected override void GenerateDecor()
    {

    }
}