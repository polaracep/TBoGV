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

    /// <summary>
    /// Difficulty level in base game ranges from 1 to 8
    /// </summary>
    public static int Difficulty;
    public static void GenerateStoryline()
    {
        LevelList = [
            new Level(p, [
                new RoomClassroom(p),new RoomShower(new Vector2(7),p)
            ], new RoomStart(p), new RoomEmpty(p), 3),
            new Level(p, [
                new RoomClassroom(p),
            ], new RoomStart(p), new RoomEmpty(p), 6),
        ];
    }

    public static void NextLevel()
    {
        // wrap na zacatek
        if (CurrentLevelNumber == LevelList.Count)
            CurrentLevelNumber = 0;

        CurrentLevel = LevelList[CurrentLevelNumber];
        CurrentLevelNumber++;
        Difficulty += CurrentLevelNumber % 2;
        Player.LevelChanged = true;
    }
    public static void End()
    {
        TBoGVGame.screenCurrent = ScreenManager.ScreenEnd;
    }


}


/* Level 1 */
/*
LevelList.Add(
    new Level(Player, new List<Room> {
        new RoomClassroom(new Vector2(9), p, new List<Entity> {
            new EnemyVitek(Vector2.Zero),
            new EnemyVitek(Vector2.Zero),
        }),
        new RoomClassroom(new Vector2(7), p, new List<Entity> {
            new EnemyVitek(Vector2.Zero),
            new EnemyCat(Vector2.Zero),
            new EnemyJirka(Vector2.Zero)
        }),
        new RoomClassroom(new Vector2(11), p, new List<Entity> {
            new EnemyZdena(Vector2.Zero),
            new EnemyPolhreich(Vector2.Zero),
        })
    },
    new RoomStart(new Vector2(7, 7), p),
    new RoomBossCat(p),
4));
*/
/* Level 2 */
/*
LevelList.Add(
    new Level(Player, new List<Room> { new RoomEmpty(new Vector2(9), p) },
    new RoomStart(new Vector2(9, 9), p),
4));
*/