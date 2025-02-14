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

    public Level(Player player, List<Room> roomList, uint maxSize, Vector2 roomStartPos)
    {
        if (roomStartPos.X > maxSize || roomStartPos.Y > maxSize)
            throw new ArgumentOutOfRangeException("The startPos is not in the level");
        this.Size = (int)maxSize;
        this.RoomCount = roomList.Count;
        this.Player = player;
        roomStartPos = new Vector2((int)(maxSize / 2), (int)(maxSize / 2));

        RoomStart start = new RoomStart(new Vector2(7, 7), roomStartPos, player);
        roomList = roomList.Prepend(start).ToList();

        LevelCreator lC = new LevelCreator(roomList, 7, roomStartPos, Player);
        this.RoomMap = lC.GenerateLevel(out roomStartPos);
        this.ActiveRoom = this.RoomMap[(int)roomStartPos.X, (int)roomStartPos.Y];
        this.ActiveRoomCoords = roomStartPos;

        TileDoor.TileInteract += OnRoomChanged;
        LevelCreator.PrintMap(this.RoomMap);

    }

    public Level(Player player, List<Room> roomList, uint maxSize) : this(player, roomList, maxSize, new Vector2(maxSize / 2, 0)) { }

    private void OnRoomChanged(object sender, TileInteractEventArgs e)
    {
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
        }
        ActiveRoom = RoomMap[(int)ActiveRoomCoords.X, (int)ActiveRoomCoords.Y];
        Console.WriteLine("DIR:" + e.Directions);
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
    private Player Player;


    public LevelCreator(List<Room> rooms, int size, Vector2 startPos, Player player)
    {
        this.Size = size;
        this.Rooms = rooms;
        this.RoomCount = rooms.Count();
        // this.StartPos = startPos;
        this.StartPos = startPos;
        this.Player = player;
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

    public Room[,] GenerateLevel(out Vector2 startRoomPos)
    {
        // Candidate map je mapa levelu, ktera nema mistnosti, ale jen kandidaty na mistnosti
        RoomCandidate[,] candidateMap = PopulateCandidates();

        // Velikosti 
        (Vector2 offset, Vector2 bounds) trucSize = GetOffsetAndSize(candidateMap);

        Room[,] finalMap = new Room[(int)trucSize.bounds.X, (int)trucSize.bounds.Y];

        Random rand = new Random();
        // kazdy kandidat
        foreach (var c in candidateMap)
        {
            if (c == null)
                continue;

            Room chosen = Rooms[rand.Next(Rooms.Count())];
            chosen.DoorDirections = c.DoorDirections;
            chosen.Position = c.Position - trucSize.offset;
            Vector2 newPos = new Vector2((int)c.Position.X - (int)trucSize.offset.X, (int)c.Position.Y - (int)trucSize.offset.Y);

            finalMap[(int)newPos.X, (int)newPos.Y] = chosen;
            PrintMap(finalMap);

        }
        startRoomPos = new Vector2(StartPos.X - trucSize.offset.X, StartPos.Y - trucSize.offset.Y);
        return finalMap;
    }

    private RoomCandidate[,] PopulateCandidates()
    {
        RoomCandidate[,] candidateMap = new RoomCandidate[Size, Size];
        int roomsPlaced = 0;

        RoomCandidate startRoom = new RoomCandidate(StartPos, Directions.ENTRY, null, Player);
        candidateMap[(int)StartPos.X, (int)StartPos.Y] = startRoom;
        roomsPlaced++;

        AddCandidate(new RoomCandidate(new Vector2(StartPos.X + 1, StartPos.Y), Directions.LEFT, startRoom, Player));
        AddCandidate(new RoomCandidate(new Vector2(StartPos.X - 1, StartPos.Y), Directions.RIGHT, startRoom, Player));
        AddCandidate(new RoomCandidate(new Vector2(StartPos.X, StartPos.Y + 1), Directions.UP, startRoom, Player));
        AddCandidate(new RoomCandidate(new Vector2(StartPos.X, StartPos.Y - 1), Directions.DOWN, startRoom, Player));

        while (roomsPlaced < RoomCount)
        {
            Random rand = new Random();
            RoomCandidate rCan = Candidates[rand.Next(Candidates.Count)];
            RoomCandidate rGet = candidateMap[(int)rCan.Position.X, (int)rCan.Position.Y];

            if (!IsValidCandidate(rCan))
                continue;

            if (rGet == null)
            {
                candidateMap[(int)rCan.Position.X, (int)rCan.Position.Y] = rCan;
                roomsPlaced++;
                AddCandidate(new RoomCandidate(new Vector2(rCan.Position.X + 1, rCan.Position.Y), Directions.LEFT, rCan, Player));
                AddCandidate(new RoomCandidate(new Vector2(rCan.Position.X - 1, rCan.Position.Y), Directions.RIGHT, rCan, Player));
                AddCandidate(new RoomCandidate(new Vector2(rCan.Position.X, rCan.Position.Y + 1), Directions.UP, rCan, Player));
                AddCandidate(new RoomCandidate(new Vector2(rCan.Position.X, rCan.Position.Y - 1), Directions.DOWN, rCan, Player));
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
            PrintMap(candidateMap);
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
        for (int y = 0; y < map.GetLength(1); y++)
        {
            for (int x = 0; x < map.GetLength(0); x++)
            {
                if (map[x, y] != null)
                    Console.Write((int)map[x, y].DoorDirections.Count + " ");
                else
                    Console.Write(". ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private void PrintMap(RoomCandidate[,] map)
    {
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
    }

    private class RoomCandidate
    {
        public Directions GeneratedFromDir;
        public RoomCandidate GeneratedFromRoom;
        public List<Directions> DoorDirections = new List<Directions>();
        public Vector2 Position;
        public RoomCandidate(Vector2 pos, Directions from, RoomCandidate gen, Player p)
        {
            Position = pos;
            GeneratedFromDir = from;
            DoorDirections.Add(from);
            GeneratedFromRoom = gen;
        }
    }
}