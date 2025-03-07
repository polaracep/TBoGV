using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using TBoGV;

public class Level
{
    protected int Size;
    protected int RoomCount;
    protected Room[,] RoomMap;
    public Room ActiveRoom { get; private set; }
    protected Vector2 ActiveRoomCoords;
    protected Player Player;
    protected Vector2 StartRoomCoords;


    protected RoomStart StartRoom;

    public Level(Player player, List<Room> roomList, RoomStart startRoom, Room endRoom, uint maxSize, Vector2 startRoomPos)
    {
        if (startRoomPos.X > maxSize || startRoomPos.Y > maxSize)
            throw new ArgumentOutOfRangeException("The startPos is not in the level");
        if (startRoom == null || endRoom == null)
            throw new ArgumentNullException();
        StartRoom = startRoom;

        Size = (int)maxSize;
        RoomCount = roomList.Count + 2;

        Player = player;

        endRoom.IsEndRoom = true;
        LevelCreator lC = new LevelCreator(roomList, startRoom, endRoom, 7, startRoomPos);
        RoomMap = lC.GenerateLevel(out startRoomPos);
        ActiveRoom = RoomMap[(int)startRoomPos.X, (int)startRoomPos.Y];
        ActiveRoomCoords = startRoomPos;

        TileDoor.TileInteract += OnRoomChanged;
        LevelCreator.PrintMap(RoomMap);
    }

    public Level(Player player, List<Room> roomList, RoomStart roomStart, Room roomBoss, uint maxSize) : this(player, roomList, roomStart, roomBoss, maxSize, new Vector2(maxSize) / 2) { }
    private void OnRoomChanged(object sender, TileInteractEventArgs e)
    {
        // ten event projede proste tolikrat, kolik je levelu...
        // check, jestli se nachazime v tom levelu, ve kterym jsme ty dvere otevreli
        if (ActiveRoom != e.Place)
            return;

        if (ActiveRoom.Enemies.Count != 0)
            return;

        switch (e.Directions)
        {
            case Directions.LEFT:
                ActiveRoomCoords.X -= 1;
                break;
            case Directions.RIGHT:
                ActiveRoomCoords.X += 1;
                break;
            case Directions.UP:
                ActiveRoomCoords.Y -= 1;
                break;
            case Directions.DOWN:
                ActiveRoomCoords.Y += 1;
                break;
            default:
                break;
        }
        ActiveRoom.ClearProjectiles();
        ActiveRoom = RoomMap[(int)ActiveRoomCoords.X, (int)ActiveRoomCoords.Y];

        if (!ActiveRoom.IsGenerated)
            ActiveRoom.Generate();

        ActiveRoom.ClearProjectiles();
        Player.Position = e.OppositeDoor.DoorTpPosition * 50;
    }
    public void Reset()
    {
        ActiveRoom = StartRoom;
        ActiveRoomCoords = StartRoom.Position;
        Player.Position = ActiveRoomCoords + new Vector2(50);
        //foreach (var room in RoomMap)
        //{
        //	if (room != null)
        //	{
        //		room.Reset();
        //	}
        //}
    }
}

public class LevelCreator
{

    // Pro tvorbu candidateMap
    private List<RoomCandidate> Candidates = new List<RoomCandidate>();
    private List<Room> Rooms;
    private int Size;
    private int RoomCount;
    private Vector2 StartPos;
    private RoomStart StartRoom;
    private Room BossRoom;
    private bool bossPlaced;

    public LevelCreator(List<Room> rooms, RoomStart startRoom, Room bossRoom, int size, Vector2 startPos)
    {
        this.Size = size;
        this.Rooms = rooms;
        this.RoomCount = rooms.Count();
        if (bossRoom != null)
        {
            this.RoomCount++;
            this.BossRoom = bossRoom;
        }
        if (startRoom != null)
        {
            this.RoomCount++;
            this.StartRoom = startRoom;
        }
        // this.StartPos = startPos;
        this.StartPos = startPos;
    }

    public Room[,] GenerateLevel(out Vector2 startRoomPos)
    {
        // Candidate map je mapa levelu, ktera nema mistnosti, ale jen kandidaty na mistnosti
        RoomCandidate[,] candidateMap = PopulateCandidates();

        // Velikosti 
        (Vector2 offset, Vector2 bounds) trucSize = GetOffsetAndSize(candidateMap);

        Room[,] finalMap = new Room[(int)trucSize.bounds.X, (int)trucSize.bounds.Y];

        Random rand = new Random();
        startRoomPos = StartPos - trucSize.offset;
        int maxDist = candidateMap.Cast<RoomCandidate>().Max(x => x != null ? x.Distance : 0);
        // kazdy kandidat na mape
        foreach (var c in candidateMap)
        {
            if (c == null)
                continue;
            Room chosen;
            bool isEntry = c.DoorDirections.Contains(Directions.ENTRY);

            // check if entry
            if (isEntry)
                startRoomPos = c.Position - trucSize.offset;

            if (BossRoom != null && c.Distance == maxDist && !bossPlaced)
            {
                // boss room!
                chosen = BossRoom;

                bossPlaced = true;
            }
            else if (isEntry && StartRoom != null)
                // start
                chosen = StartRoom;
            else
            {
                // nahodny
                chosen = Rooms[rand.Next(Rooms.Count())];
                Rooms.Remove(chosen);
            }

            // Dvere handling
            foreach (Directions dir in c.DoorDirections)
            {
                chosen.Doors.Add(new TileDoor(DoorTypes.BASIC, dir, Vector2.Zero));
            }
            chosen.Position = c.Position - trucSize.offset;
            Vector2 newPos = new Vector2((int)c.Position.X - (int)trucSize.offset.X, (int)c.Position.Y - (int)trucSize.offset.Y);

            // assign nahodneho do mapy
            finalMap[(int)newPos.X, (int)newPos.Y] = chosen;

            // PrintMap(finalMap);

        }
        finalMap = LinkDoors(finalMap);
        return finalMap;
    }
    private Room[,] LinkDoors(Room[,] roomMap)
    {
        foreach (Room room in roomMap)
        {
            if (room == null)
                continue;

            foreach (TileDoor door in room.Doors)
            {
                Vector2 newRoomPos = new Vector2(-1);
                Directions lookingForDir = Directions.ENTRY;
                switch (door.Direction)
                {
                    case Directions.LEFT:
                        newRoomPos = room.Position - Vector2.UnitX;
                        lookingForDir = Directions.RIGHT;
                        break;
                    case Directions.RIGHT:
                        newRoomPos = room.Position + Vector2.UnitX;
                        lookingForDir = Directions.LEFT;
                        break;
                    case Directions.UP:
                        newRoomPos = room.Position - Vector2.UnitY;
                        lookingForDir = Directions.DOWN;
                        break;
                    case Directions.DOWN:
                        newRoomPos = room.Position + Vector2.UnitY;
                        lookingForDir = Directions.UP;
                        break;
                    case Directions.ENTRY:
                        continue;
                }
                Room linkRoom = roomMap[(int)newRoomPos.X, (int)newRoomPos.Y];
                door.OppositeDoor = linkRoom.Doors.Find(d => d.Direction == lookingForDir);
                if (linkRoom == BossRoom)
                    door.SetDoorType(DoorTypes.BOSS);
            }
        }
        return roomMap;
    }

    private RoomCandidate[,] PopulateCandidates()
    {
        RoomCandidate[,] candidateMap = new RoomCandidate[Size, Size];
        int roomsPlaced = 0;

        RoomCandidate startRoom = new RoomCandidate(StartPos, Directions.ENTRY, null, 0);
        candidateMap[(int)StartPos.X, (int)StartPos.Y] = startRoom;
        roomsPlaced++;

        AddCandidate(new RoomCandidate(new Vector2(StartPos.X + 1, StartPos.Y), Directions.LEFT, startRoom, 1));
        AddCandidate(new RoomCandidate(new Vector2(StartPos.X - 1, StartPos.Y), Directions.RIGHT, startRoom, 1));
        AddCandidate(new RoomCandidate(new Vector2(StartPos.X, StartPos.Y + 1), Directions.UP, startRoom, 1));
        AddCandidate(new RoomCandidate(new Vector2(StartPos.X, StartPos.Y - 1), Directions.DOWN, startRoom, 1));

        while (roomsPlaced < RoomCount)
        {
            Random rand = new Random();
            // nahodny novy candidat
            RoomCandidate rCan = Candidates[rand.Next(Candidates.Count)];
            RoomCandidate rGet = candidateMap[(int)rCan.Position.X, (int)rCan.Position.Y];

            if (!IsValidCandidate(rCan))
                continue;

            if (rGet == null)
            {
                candidateMap[(int)rCan.Position.X, (int)rCan.Position.Y] = rCan;
                roomsPlaced++;
                AddCandidate(new RoomCandidate(new Vector2(rCan.Position.X + 1, rCan.Position.Y), Directions.LEFT, rCan, rCan.Distance + 1));
                AddCandidate(new RoomCandidate(new Vector2(rCan.Position.X - 1, rCan.Position.Y), Directions.RIGHT, rCan, rCan.Distance + 1));
                AddCandidate(new RoomCandidate(new Vector2(rCan.Position.X, rCan.Position.Y + 1), Directions.UP, rCan, rCan.Distance + 1));
                AddCandidate(new RoomCandidate(new Vector2(rCan.Position.X, rCan.Position.Y - 1), Directions.DOWN, rCan, rCan.Distance + 1));
            }
            else
            {
                if (!rGet.DoorDirections.Contains(rCan.GeneratedFromDir))
                    rGet.DoorDirections.Add(rCan.GeneratedFromDir);
            }
            Candidates.Remove(rCan);

            Directions d;
            if (rCan.GeneratedFromRoom != null)
            {
                switch (rCan.GeneratedFromDir)
                {
                    case Directions.LEFT:
                        d = Directions.RIGHT;
                        break;
                    case Directions.RIGHT:
                        d = Directions.LEFT;
                        break;
                    case Directions.UP:
                        d = Directions.DOWN;
                        break;
                    case Directions.DOWN:
                        d = Directions.UP;
                        break;
                    default:
                        d = Directions.ENTRY;
                        break;
                }
                if (!rCan.GeneratedFromRoom.DoorDirections.Contains(d))
                    rCan.GeneratedFromRoom.DoorDirections.Add(d);
            }
            // PrintMap(candidateMap);
        }

        Candidates.Clear();
        return candidateMap;
    }
    private void AddCandidate(RoomCandidate r)
    {
        if (IsValidCandidate(r))
            Candidates.Add(r);
    }
    private bool IsValidCandidate(RoomCandidate r)
    {
        return (r.Position.X < Size && r.Position.Y < Size && r.Position.X >= 0 && r.Position.Y >= 0) == true;
    }

    public static void PrintMap(Room[,] map)
    {
#if DEBUG
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (map[x, y] != null)
                    Console.Write((int)map[x, y].Doors.Count + " ");
                else
                    Console.Write(". ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
#endif
    }
    private void PrintMap(RoomCandidate[,] map)
    {
#if DEBUG
        for (int y = 0; y < Size; y++)
        {
            for (int x = 0; x < Size; x++)
            {
                if (map[x, y] != null)
                    Console.Write((int)map[x, y].DoorDirections.Count + " ");
                else
                    Console.Write(". ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
#endif
    }

    private (Vector2 offset, Vector2 bounds) GetOffsetAndSize(RoomCandidate[,] rooms)
    {
        int x = 0, y = 0;
        int _x = int.MaxValue, _y = int.MaxValue;
        foreach (RoomCandidate r in rooms)
        {
            if (r == null)
                continue;

            if (r.Position.X > x)
                x = (int)r.Position.X;
            if (r.Position.Y > y)
                y = (int)r.Position.Y;
            if (r.Position.X < _x)
                _x = (int)r.Position.X;
            if (r.Position.Y < _y)
                _y = (int)r.Position.Y;
        }
        return (new Vector2(_x, _y), new Vector2(x - _x + 1, y - _y + 1));
    }

    private class RoomCandidate
    {
        public Directions GeneratedFromDir;
        public RoomCandidate GeneratedFromRoom;
        public List<Directions> DoorDirections = new List<Directions>();
        public Vector2 Position;
        /// <summary>
        /// Distance from start room
        /// </summary>
        public int Distance = 0;
        public RoomCandidate(Vector2 pos, Directions from, RoomCandidate gen, int dist)
        {
            Position = pos;
            GeneratedFromDir = from;
            DoorDirections.Add(from);
            GeneratedFromRoom = gen;
            Distance = dist;
        }
    }
}