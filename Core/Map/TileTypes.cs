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
    public static readonly FloorTypes TOILET = new FloorTypes("floorToilet");

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
    public static readonly WallTypes HALLWAY_ORANGE = new WallTypes("wallOrange");
    public static readonly WallTypes HALLWAY_ORANGE_CORNER = new WallTypes("wallOrangeCorner");
    public static readonly WallTypes HALLWAY_GREEN = new WallTypes("wallLightGreen");
    public static readonly WallTypes HALLWAY_GREEN_CORNER = new WallTypes("wallLightGreenCorner");
    public static readonly WallTypes HALLWAY_BLUE = new WallTypes("wallBlue");
    public static readonly WallTypes HALLWAY_BLUE_CORNER = new WallTypes("wallBlueCorner");
    public static readonly WallTypes SHOWER = new WallTypes("wallShower");
    public static readonly WallTypes SHOWER_CORNER = new WallTypes("wallShowerCorner");
    public static readonly WallTypes TOILET = new WallTypes("wallToilet");
    public static readonly WallTypes TOILET_CORNER = new WallTypes("wallToiletCorner");
    public static readonly WallTypes TOILET_DIVIDER = new WallTypes("wallDivider");
    public static readonly WallTypes TOILET_DIVIDER_END = new WallTypes("wallDividerEnd");
    public static readonly WallTypes TOILET_T = new WallTypes("wallToiletT");
    public static readonly WallTypes TOILET_DIVIDER_END_ROT = new WallTypes("wallDividerEndRot");

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
    public static readonly DecorationTypes PAINTING1 = new DecorationTypes("decoPainting1");
    public static readonly DecorationTypes PAINTING2 = new DecorationTypes("decoPainting2");
    public static readonly DecorationTypes PAINTING3 = new DecorationTypes("decoPainting3");
    public static readonly DecorationTypes PAINTING4 = new DecorationTypes("decoPainting4");
    public static readonly DecorationTypes PAINTING5 = new DecorationTypes("decoPainting5");
    public static readonly DecorationTypes PAINTING6 = new DecorationTypes("decoPainting6");
    public static readonly DecorationTypes PAINTING7 = new DecorationTypes("decoPainting7");
    public static readonly DecorationTypes PAINTING8 = new DecorationTypes("decoPainting8");
    public static readonly DecorationTypes PAINTING9 = new DecorationTypes("decoPainting9");
    public static readonly DecorationTypes PAINTING10 = new DecorationTypes("decoPainting10");
    public static readonly DecorationTypes PAINTING11 = new DecorationTypes("decoPainting11");
    public static readonly DecorationTypes PAINTING12 = new DecorationTypes("decoPainting12");
    public static readonly DecorationTypes PAINTING13 = new DecorationTypes("decoPainting13");
    public static readonly DecorationTypes PAINTING14 = new DecorationTypes("decoPainting14");
    public static readonly DecorationTypes PAINTING15 = new DecorationTypes("decoPainting15");
    public static readonly DecorationTypes PAINTING16 = new DecorationTypes("decoPainting16");
    public static readonly DecorationTypes PAINTING17 = new DecorationTypes("decoPainting17");
    public static readonly DecorationTypes PAINTING18 = new DecorationTypes("decoPainting18");
    public static readonly DecorationTypes CHAIR_CAFETERIA_R = new DecorationTypes("chairCafeteriaRed");
    public static readonly DecorationTypes CHAIR_CAFETERIA_G = new DecorationTypes("chairCafeteriaGreen");
    public static readonly DecorationTypes TABLE_CAFETERIA = new DecorationTypes("tableCafeteria");
    public static readonly DecorationTypes FRIDGE1 = new DecorationTypes("decoFridge1");
    public static readonly DecorationTypes FRIDGE2 = new DecorationTypes("decoFridge2");
    public static readonly DecorationTypes COFFEE_MACHINE = new DecorationTypes("coffeeMachine");
    public static readonly DecorationTypes SINK = new DecorationTypes("showerSink");
    public static readonly DecorationTypes BENCH = new DecorationTypes("bench");
    public static readonly DecorationTypes WINDOW = new DecorationTypes("window");
    public static readonly DecorationTypes TOILET = new DecorationTypes("toilet");
    public static readonly DecorationTypes URINAL = new DecorationTypes("urinal");
    public static readonly DecorationTypes EMPTY = new DecorationTypes("empty");
    public static readonly DecorationTypes MIRROR = new DecorationTypes("mirror");
    public static readonly DecorationTypes PUB_TABLE1 = new DecorationTypes("pub_table_beer");
    public static readonly DecorationTypes PUB_TABLE2 = new DecorationTypes("pub_table_beer_spilled");
    public static readonly DecorationTypes PUB_TABLE3 = new DecorationTypes("pub_table_bottles");
    public static readonly DecorationTypes PUB_TABLE4 = new DecorationTypes("pub_table_bottles1");
    public static readonly DecorationTypes PUB_TABLE5 = new DecorationTypes("pub_table_bottles2");
    public static readonly DecorationTypes PUB_TABLE6 = new DecorationTypes("pub_table_bottles3");
    public static readonly DecorationTypes PUB_TABLE7 = new DecorationTypes("pub_table_bottles4");
    public static readonly DecorationTypes PUB_SLOW_ZONE = new DecorationTypes("empty");
}