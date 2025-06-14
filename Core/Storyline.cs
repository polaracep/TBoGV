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
    public static bool Endless = false;
    public static Player Player
    {
        get => p;
        set => p = value;
    }
    private static Player p;

    /// <summary>
    /// Difficulty level in base game ranges from idk to idk
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
        int specialRoomCount = rand.Next(0, 2) + roomCount / 5;
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
    public static Level GenerateEndlessLevel()
    {
        var rooms = GenerateLevelRooms(Math.Max(5, CurrentLevelNumber - 4), Math.Max(8, CurrentLevelNumber - 2));
        List<Room> bossRooms = [new RoomBossAles(p), new RoomBossToilet(p), new RoomBossSvarta(p), new RoomBossCat(p), new RoomBossRichard(p), new RoomBossZeman(p), new RoomBossAmogus(p)];
        if (CurrentLevelNumber % 2 == 0)
            bossRooms = [new RoomClassroom(p)];
        Level level = new Level(p, rooms, new RoomStart(p), bossRooms[Random.Shared.Next(bossRooms.Count)], (uint)Math.Max(6, (int)Math.Sqrt(CurrentLevelNumber - 2) + 1));
        return level;
    }

    private static bool promoted = false;
    public static void NextLevel()
    {
        Player.Save(SaveType.AUTO);
        if (Endless)
        {
            NextLevelEndless();
            return;
        }
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
    public static void NextLevelEndless()
    {

        // reset failed times
        if (CurrentLevelNumber % 2 == 0 && !promoted)
        {
            FailedTimes = 0;
            promoted = true;
        }
        if (CurrentLevelNumber % 2 == 1)
            promoted = false;

        CurrentLevel = GenerateEndlessLevel();
        CurrentLevelNumber++;
        Difficulty = (int)Math.Floor((CurrentLevelNumber - 1) / (float)2) + 1;
        Player.Save(SaveType.AUTO);
    }
    public static void ResetLevel()
    {
        if (Endless)
        {
            ResetLevelEndless();
            return;
        }
        int f = FailedTimes;
        GenerateStoryline();
        if (CurrentLevelNumber >= LevelList.Count)
            CurrentLevelNumber = 0;
        CurrentLevel = LevelList[CurrentLevelNumber];
        CurrentLevel.Reset();
        FailedTimes = f;
    }
    public static void ResetLevelEndless()
    {
        int f = FailedTimes;
        CurrentLevel = GenerateEndlessLevel();
        CurrentLevel.Reset();
        FailedTimes = f;
        Player.Inventory.AddEffect(new EffectEndless());
    }
    public static void ResetStoryline()
    {
        GenerateStoryline();
        CurrentLevel = LevelList[0];
        CurrentLevel.Reset();
        CurrentLevelNumber = 0;
        Difficulty = 0;
        FailedTimes = 0;
        Endless = false;
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