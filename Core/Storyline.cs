using System;
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
    /// Difficulty level in base game ranges from 0 to 7
    /// </summary>
    public static int Difficulty = 0;
    public static int FailedTimes = 0;
    public static void GenerateStoryline()
    {
        LevelList = [
#if DEBUG
            // new Level(p, [ new RoomLocker( p) , new RoomLocker(p), new RoomLocker(p), new RoomLocker(p), new RoomLocker(p)], new RoomStart(p), new RoomStart(p), 2),
            // new Level(p, [ new RoomToilet(p),  new RoomToilet(p),  new RoomToilet(p),  new RoomToilet(p)], new RoomStart(p), new RoomStart(p), 2),

#endif
            new Level(p, GenerateLevelRooms(5, 8), new RoomStart(p), new RoomClassroom(p), 3),
            new Level(p, GenerateLevelRooms(5, 8), new RoomStart(p), new RoomBossSvarta(p), 3),
            new Level(p, GenerateLevelRooms(5, 8), new RoomStart(p), new RoomClassroom(p), 6),
            new Level(p, GenerateLevelRooms(5, 8), new RoomStart(p), new RoomBossAles(p), 6),
            new Level(p, GenerateLevelRooms(6, 10), new RoomStart(p), new RoomClassroom(p), 6),
            new Level(p, GenerateLevelRooms(6, 10), new RoomStart(p), new RoomBossCat(p), 6),
            new Level(p, GenerateLevelRooms(6, 10), new RoomStart(p), new RoomClassroom(p), 6),
            new Level(p, GenerateLevelRooms(6, 10), new RoomStart(p), new RoomBossToilet(p), 6),
            new Level(p, GenerateLevelRooms(8, 12), new RoomStart(p), new RoomClassroom(p), 6),
            new Level(p, GenerateLevelRooms(8, 12), new RoomStart(p), new RoomBossRichard(p), 6),
            new Level(p, GenerateLevelRooms(8, 12), new RoomStart(p), new RoomClassroom(p), 6),
            new Level(p, GenerateLevelRooms(8, 12), new RoomStart(p), new RoomBossZeman(p), 6),
            new Level(p, GenerateLevelRooms(8, 12), new RoomStart(p), new RoomClassroom(p), 6),
            new Level(p, GenerateLevelRooms(10, 14), new RoomStart(p), new RoomBossAmogus(p), 6),
            new Level(p, GenerateLevelRooms(8, 12), new RoomStart(p), new RoomClassroom(p), 6),
            new Level(p, GenerateLevelRooms(10, 14), new RoomStart(p), new RoomBossMaturita(p), 6),
        ];
    }

    public static List<Room> GenerateLevelRooms(int minR, int maxR)
    {


        List<Room> rooms = new List<Room>();
        if (minR <= 0 || maxR <= 0)
            return rooms;
        Random rand = new Random();
        int roomCount = rand.Next(minR, maxR + 1);
        int classroomCount = rand.Next(roomCount / 2, roomCount - 1);
        int specialRoomCount = rand.Next(1, 3);
        int hallwayCount = roomCount - classroomCount - specialRoomCount;


        for (int i = 0; i < classroomCount; i++)
            rooms.Add(new RoomClassroom(p));

        for (int i = 0; i < hallwayCount; i++)
            rooms.Add(new RoomHallway(p));

        for (int i = 0; i < specialRoomCount; i++)
        {
            int roomType = rand.Next(3);
            if (roomType == 0)
                rooms.Add(new RoomShower(p));
            else if (roomType == 1)
                rooms.Add(new RoomLocker(p));
            else
                rooms.Add(new RoomToilet(p));
        }


        return rooms;
    }

    private static bool promoted = false;
    public static void NextLevel()
    {
        Player.Save(SaveType.AUTO);
        // wrap na zacatek
        if (CurrentLevelNumber == LevelList.Count)
            CurrentLevelNumber = 0;

        // reset failed times
        if (CurrentLevelNumber % 2 == 0 && !promoted)
        {
            FailedTimes = 0;
            promoted = true;
        }
        if (CurrentLevelNumber % 2 == 1)
            promoted = false;

        CurrentLevel = LevelList[CurrentLevelNumber];
        CurrentLevelNumber++;
        Difficulty = (int)Math.Floor((CurrentLevelNumber - 1) / (float)2) + 1;

    }
    public static void ResetLevel()
    {
        int f = FailedTimes;
        GenerateStoryline();
        CurrentLevel = LevelList[CurrentLevelNumber];
        CurrentLevel.Reset();
        FailedTimes = f;
        if (CurrentLevelNumber > 0)
            CurrentLevelNumber--;
    }
    public static void ResetStoryline()
    {
        GenerateStoryline();
        CurrentLevel = LevelList[0];
        CurrentLevel.Reset();
        CurrentLevelNumber++;
        Difficulty = 1;
        FailedTimes = 0;
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
            new EnemySoldier(Vector2.Zero),
            new EnemySoldier(Vector2.Zero),
        }),
        new RoomClassroom(new Vector2(7), p, new List<Entity> {
            new EnemySoldier(Vector2.Zero),
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