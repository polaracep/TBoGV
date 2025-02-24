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

public sealed class FloorTypes : StringEnum
{
    public static readonly FloorTypes BASIC = new FloorTypes("floorYellow");
    public static readonly FloorTypes LOBBY = new FloorTypes("floorLobby");
	public static readonly FloorTypes STAIRS = new FloorTypes("stairs");

	public FloorTypes(string v) : base(v) { }
}

public sealed class DoorTypes : StringEnum
{
    public static readonly DoorTypes BASIC = new DoorTypes("door");

    public DoorTypes(string v) : base(v) { }
}
public sealed class WallTypes : StringEnum
{
    public static readonly WallTypes BASIC = new WallTypes("wallBrick");
    public static readonly WallTypes WHITE = new WallTypes("wallWhite");
    public static readonly WallTypes WHITE_CORNER = new WallTypes("wallWhiteCorner");
    public static readonly WallTypes LOBBY = new WallTypes("wallLobby");
    public static readonly WallTypes LOBBY_CORNER = new WallTypes("wallLobbyCorner");

    public WallTypes(string v) : base(v) { }

}