using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TBoGV;

class TutorialRoom : RoomClassroom
{
    private bool Peaceful = true;
    private bool PlayedDialogueStart = false;
    private bool PlayedDialogueEnd = false;
    private Dialogue DialogueStart;
    private Dialogue DialogueEnd;
    public TutorialRoom(Vector2 dimensions, Player p, bool noEnemies, List<Entity> enemies, Dialogue dialogueStart, Dialogue dialogueEnd)
        : base(dimensions, p, enemies)
    {
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

    public override void Generate()
    {
        if (direction == null)
            direction = (Directions)Random.Shared.Next(4);

        GenerateBase(FloorTypes.BASIC, WallTypes.WHITE, DoorTypes.BASIC);
        GenerateDecor();
        if (!Peaceful)
            GenerateEnemies();
    }

    public override void OnEntry()
    {
        if (PlayedDialogueStart || DialogueStart == null)
            return;

        ScreenManager.ScreenGame.OpenDialogue(DialogueStart, TutorialLevel.NpcName, TutorialLevel.NpcSprite);
        PlayedDialogueStart = true;
    }

    public override void OnExit()
    {
        if (PlayedDialogueEnd || DialogueEnd == null)
            return;

        ScreenManager.ScreenGame.OpenDialogue(DialogueEnd, TutorialLevel.NpcName, TutorialLevel.NpcSprite);
        PlayedDialogueEnd = true;
    }
}