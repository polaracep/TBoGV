using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TBoGV;

/// <summary>
/// Consisting of 16 Levels
/// </summary>
public static class Storyline
{
    public static List<Level> LevelList;
    public static int CurrentLevelNumber = 0;
    public static Level CurrentLevel = null;
    public static Player Player
    {
        get => p;
        set => p = value;
    }
    private static Player p;

    public static void GenerateStoryline()
    {
        LevelList = new List<Level>();
        // Test level
        LevelList.Add(
            new Level(Player, new List<Room> {
                /*new RoomClassroom(new Vector2(9, 9), p, new List<Entity> { }),
                new RoomClassroom(new Vector2(9, 9), p, new List<Entity> { }),
                new RoomClassroom(new Vector2(9, 9), p, new List<Entity> { }),
                new RoomHallway(new Vector2(7), p, new List<Entity> { }),
                new RoomHallway(new Vector2(7), p, new List<Entity> { }),
                new RoomHallway(new Vector2(7), p, new List<Entity> { }),
                */
                new RoomHallway(new Vector2(7), p, new List<Entity> { }),
            },
            new RoomStart(new Vector2(7, 7), p), // start room
            new RoomClassroom(new Vector2(11), p, new List<Entity> { new BossAles() }), // boss room
            7
            )
        );

        /* Level 1 */
        /*
        LevelList.Add(
            new Level(Player, new List<Room> {
                new RoomClassroom(new Vector2(9), p, new List<Entity> {
                    new EnemyZdena(Vector2.Zero),
                    new EnemyZdena(Vector2.Zero),
                    new EnemyZdena(Vector2.Zero),
                    new EntitySarka(Vector2.One),
                }),
                new RoomClassroom(new Vector2(7), p, new List<Entity> {
                    new EnemyVitek(Vector2.Zero),
                    new EnemyVitek(Vector2.Zero),
                    new EnemyVitek(Vector2.Zero),
                }),
                new RoomClassroom(new Vector2(11), p, new List<Entity> {
                    new EnemyZdena(Vector2.Zero),
                    new EnemyZdena(Vector2.Zero),
                    new EnemyZdena(Vector2.Zero),
                })
            },
            new RoomStart(new Vector2(7, 7), p),
        4));
        */
        /* Level 2 */
        /*
        LevelList.Add(
            new Level(Player, new List<Room> { new RoomEmpty(new Vector2(9), p) },
            new RoomStart(new Vector2(9, 9), p),
        4));
        */
    }

    public static void NextLevel()
    {
        if (CurrentLevelNumber == LevelList.Count)
            // Won the game
            CurrentLevelNumber = 0;

        CurrentLevel = LevelList[CurrentLevelNumber];
        CurrentLevelNumber++;
        Player.LevelChanged = true;
    }
}