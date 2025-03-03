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
    public static int Difficulty = 1;
	public static int FailedTimes = 0;
    public static void GenerateStoryline()
    {
        LevelList = [
            // L1
            new Level(p, [
                //new RoomClassroom(p),
                //new RoomClassroom(p),
                //new RoomClassroom(p),
                //new RoomHallway(p),
                //new RoomHallway(p),
                new RoomShower(p)
            ], new RoomStart(p), new RoomBossSvarta(p), 3),
            // L2
            new Level(p, [
                new RoomClassroom(p),
                new RoomClassroom(p),
                new RoomClassroom(p),
                new RoomClassroom(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomShower(p),
                new RoomShower(p),
            ], new RoomStart(p), new RoomBossAles(p), 6),
            // L3
            new Level(p, [
                new RoomClassroom(p),
                new RoomClassroom(p),
                new RoomClassroom(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomShower(p),
            ], new RoomStart(p), new RoomBossCat(p), 6),
            // L4
            new Level(p, [
                new RoomClassroom(p),
                new RoomClassroom(p),
                new RoomClassroom(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomShower(p),
            ], new RoomStart(p), new RoomBossToilet(p), 6),
            // L5
            new Level(p, [
                new RoomClassroom(p),
                new RoomClassroom(p),
                new RoomClassroom(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomShower(p),
            ], new RoomStart(p), new RoomBossRichard(p), 6),
            // L6
            new Level(p, [
                new RoomClassroom(p),
                new RoomClassroom(p),
                new RoomClassroom(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomShower(p),
            ], new RoomStart(p), new RoomBossToilet(p), 6),
            // L7
            new Level(p, [
                new RoomClassroom(p),
                new RoomClassroom(p),
                new RoomClassroom(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomShower(p),
            ], new RoomStart(p), new RoomBossAmogus(p), 6),
            // L8
            new Level(p, [
                new RoomClassroom(p),
                new RoomClassroom(p),
                new RoomClassroom(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomHallway(p),
                new RoomShower(p),
            ], new RoomStart(p), new RoomBossMaturita(p), 6),
        ];
    }

    public static void NextLevel()
    {
		Player.Save(SaveType.AUTO);
		// wrap na zacatek
		if (CurrentLevelNumber == LevelList.Count)
            CurrentLevelNumber = 0;

		CurrentLevel = LevelList[CurrentLevelNumber];
		CurrentLevelNumber++;
        Difficulty += (CurrentLevelNumber + 1) % 2;
        Player.LevelChanged = true;

    }
	public static void ResetLevel()
	{
		CurrentLevel.Reset();
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