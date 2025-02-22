using Microsoft.Xna.Framework;
using TBoGV;

public class RoomBoss : Room
{
    public RoomBoss(Vector2 dimensions, Player p) : base(dimensions, p) { }

    public RoomBoss(Vector2 dimensions, Vector2 pos, Player p) : base(dimensions, pos, p) { }

    public override void GenerateRoom()
    {
        throw new System.NotImplementedException();
    }

    public override void Reset()
    {
        throw new System.NotImplementedException();
    }

    protected override void GenerateEnemies()
    {
        throw new System.NotImplementedException();
    }

}