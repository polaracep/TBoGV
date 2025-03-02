namespace TBoGV;

/// <summary>
/// Base class for "enum-like" types. 
/// Field example: 
/// <code>public static readonly FloorTypes BASIC = new FloorTypes("floorYellow");</code>
/// </summary>
public class StringEnum
{
    public string Value { get; }

    protected StringEnum(string value) { Value = value; }
    public sealed override string ToString() => Value;
}

/* ===== Tile types ===== */

public sealed class FloorTypes : StringEnum
{
    public static readonly FloorTypes BASIC = new FloorTypes("floorYellow");
    public static readonly FloorTypes LOBBY = new FloorTypes("floorLobby");
    public static readonly FloorTypes STAIRS = new FloorTypes("stairs");
    public static readonly FloorTypes HALLWAY = new FloorTypes("floorHallway");
    public static readonly FloorTypes SHOWER = new FloorTypes("showerFloor");

    public FloorTypes(string v) : base(v) { }
}
public sealed class DoorTypes : StringEnum
{
    public static readonly DoorTypes BASIC = new DoorTypes("door");
    public static readonly DoorTypes BOSS = new DoorTypes("doorBoss");

    public DoorTypes(string v) : base(v) { }
}
public sealed class WallTypes : StringEnum
{
    public static readonly WallTypes BASIC = new WallTypes("wallBrick");
    public static readonly WallTypes WHITE = new WallTypes("wallWhite");
    public static readonly WallTypes WHITE_CORNER = new WallTypes("wallWhiteCorner");
    public static readonly WallTypes LOBBY = new WallTypes("wallLobby");
    public static readonly WallTypes LOBBY_CORNER = new WallTypes("wallLobbyCorner");
    public static readonly WallTypes HALLWAY = new WallTypes("wallHallway");
    public static readonly WallTypes HALLWAY_CORNER = new WallTypes("wallHallwayCorner");

    public WallTypes(string v) : base(v) { }

}

public sealed class DecorationTypes : StringEnum
{
    public DecorationTypes(string v) : base(v) { }
    public static readonly DecorationTypes CHAIR_CLASSROOM = new DecorationTypes("chairClassroom");
    public static readonly DecorationTypes DESK = new DecorationTypes("tableClassroom");
    public static readonly DecorationTypes KATEDRA = new DecorationTypes("tableComputerClassroom");
    public static readonly DecorationTypes BLACKBOARD = new DecorationTypes("blackboard");
    public static readonly DecorationTypes INFO1 = new DecorationTypes("decoInfo1");
    public static readonly DecorationTypes INFO2 = new DecorationTypes("decoInfo2");
    public static readonly DecorationTypes INFO3 = new DecorationTypes("decoInfo3");
    public static readonly DecorationTypes INFO4 = new DecorationTypes("decoInfo4");
    public static readonly DecorationTypes CHAIR_CAFETERIA_R = new DecorationTypes("chairCafeteriaRed");
    public static readonly DecorationTypes CHAIR_CAFETERIA_G = new DecorationTypes("chairCafeteriaGreen");
    public static readonly DecorationTypes TABLE_CAFETERIA = new DecorationTypes("tableCafeteria");
    public static readonly DecorationTypes FRIDGE1 = new DecorationTypes("decoFridge1");
    public static readonly DecorationTypes FRIDGE2 = new DecorationTypes("decoFridge2");
    public static readonly DecorationTypes COFFEE_MACHINE = new DecorationTypes("coffeeMachine");
    public static readonly DecorationTypes SINK = new DecorationTypes("showerSink");

}