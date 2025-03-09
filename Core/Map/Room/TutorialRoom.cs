using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TBoGV;

class TutorialRoom : RoomClassroom
{
    private bool Peaceful = true;
    public TutorialRoom(Vector2 dimensions, Player p, bool noEnemies, List<Entity> enemies) : this(p, noEnemies, enemies)
    {
        Dimensions = dimensions;
    }
    public TutorialRoom(Player p, bool noEnemies, List<Entity> enemies) : base(p, enemies)
    {
        Peaceful = noEnemies;
    }

    protected override List<Enemy> validEnemies { get; set; } = [
        new EnemyTriangle(),
        new EnemySoldier()
    ];

    public override void Generate()
    {
        if (direction == null)
            direction = (Directions)Random.Shared.Next(4);

        GenerateBase(FloorTypes.BASIC, WallTypes.WHITE, DoorTypes.BASIC);
        GenerateDecor();
        if (!Peaceful)
            GenerateEnemies();
    }
}