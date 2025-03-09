using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;
public class TutorialLevel
{
    private Room[,] RoomMap;
    public Room ActiveRoom { get; private set; }
    private Vector2 ActiveRoomCoords;
    private Player _player;

    public static string NpcName { get; private set; } = "Schovánek";
    // TODO: Schovanek
    public static Texture2D NpcSprite { get; private set; } = TextureManager.GetTexture("sarka");

    public TutorialLevel(Player player)
    {
        _player = player;
        TileDoor.TileInteract += OnRoomChanged;
        RoomMap = new Room[,]{
            {new TutorialRoom(new Vector2(13), _player, true, new DialogueIntro(), null)},
            {new TutorialRoom(new Vector2(9), _player, true, new DialogueIntro(), null)},
            {new TutorialRoom(new Vector2(13), _player, false, [new EnemyCat(), new EnemySoldier()], new DialogueIntro(), new DialogueIntro())},
        };

        // tvorba dveri
        RoomMap[0, 0].Doors.Add(new TileDoor(DoorTypes.BASIC, Directions.RIGHT));
        RoomMap[1, 0].Doors.Add(new TileDoor(DoorTypes.BASIC, Directions.LEFT));
        RoomMap[1, 0].Doors.Add(new TileDoor(DoorTypes.BASIC, Directions.RIGHT));
        RoomMap[2, 0].Doors.Add(new TileDoor(DoorTypes.BASIC, Directions.LEFT));

        // linkovani dveri
        RoomMap[0, 0].Doors[0].OppositeDoor = RoomMap[1, 0].Doors[0];
        RoomMap[1, 0].Doors[0].OppositeDoor = RoomMap[0, 0].Doors[0];
        RoomMap[1, 0].Doors[1].OppositeDoor = RoomMap[2, 0].Doors[0];
        RoomMap[2, 0].Doors[0].OppositeDoor = RoomMap[1, 0].Doors[1];

        RoomMap[2, 0].IsEndRoom = true;

        // generate
        RoomMap[0, 0].Generate();
        RoomMap[1, 0].Generate();
        RoomMap[2, 0].Generate();

        ActiveRoom = RoomMap[0, 0];
        ActiveRoomCoords = Vector2.Zero;
    }
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
        }
        ActiveRoom.ClearProjectiles();

        ActiveRoom.OnExit();
        ActiveRoom = RoomMap[(int)ActiveRoomCoords.X, (int)ActiveRoomCoords.Y];
        ActiveRoom.OnEntry();

        _player.Position = e.OppositeDoor.DoorTpPosition * 50;
    }
}