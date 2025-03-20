using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TBoGV;

class TutorialRoom : RoomClassroom
{
    private bool Peaceful = true;
    private bool PlayedDialogueStart = false;
    private bool PlayedDialogueEnd = false;
    private Dialogue DialogueStart;
    private Dialogue DialogueEnd;
    private new List<Entity> Enemies;
    public TutorialRoom(Vector2 dimensions, Player p, bool noEnemies, List<Entity> enemies, Dialogue dialogueStart, Dialogue dialogueEnd)
        : base(dimensions, p, enemies)
    {
        Enemies = enemies;
        DialogueStart = dialogueStart;
        DialogueEnd = dialogueEnd;
        Peaceful = noEnemies;
        Dimensions = dimensions;
    }
    public TutorialRoom(Vector2 dimensions, Player p, bool noEnemies, Dialogue dialogueStart, Dialogue dialogueEnd)
        : this(dimensions, p, noEnemies, null, dialogueStart, dialogueEnd) { }

    protected override List<Enemy> validEnemies { get; set; } = [
        new EnemyTriangle(),
        new EnemySoldier()
    ];

    public override void Update(double dt)
    {
        base.Update(dt);
    }
    public override void Generate()
    {
        if (direction == null)
            direction = (Directions)Random.Shared.Next(4);

        GenerateBase(FloorTypes.BASIC, WallTypes.WHITE, DoorTypes.BASIC);
        GenerateDecor();
        if (!Peaceful)
            GenerateEnemies();
        IsGenerated = true;
    }

    protected override void GenerateEnemies()
    {
        if (Enemies == null)
            return;
        foreach (var entity in Enemies)
        {
            if (entity is not Enemy enemy)
                break;
            while (true)
            {
                Vector2 spawnPos = new Vector2(
                    Random.Shared.Next(50 * ((int)Dimensions.X - 3)) + 50,
                    Random.Shared.Next(50 * ((int)Dimensions.Y - 3)) + 50);

                if (Doors.Any(d => ((d.DoorTpPosition * 50) - spawnPos).Length() < 100))
                    continue;

                if (!ShouldCollideAt(new Rectangle(spawnPos.ToPoint(), enemy.Size.ToPoint())))
                {
                    enemy.Position = spawnPos;
                    AddEnemy(enemy);
                    break;
                }
            }
        }
    }

    public override void OnEntry()
    {
        if (PlayedDialogueStart || DialogueStart == null)
            return;

        ScreenManager.ScreenGame.OpenDialogue(DialogueStart);
        PlayedDialogueStart = true;
    }

    public override void OnExit()
    {
        if (PlayedDialogueEnd || DialogueEnd == null)
            return;

        ScreenManager.ScreenGame.OpenDialogue(DialogueEnd);
        PlayedDialogueEnd = true;
    }
}