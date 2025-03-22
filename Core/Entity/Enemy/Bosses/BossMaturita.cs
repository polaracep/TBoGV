using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

class BossMaturita : EnemyBoss
{
    public BossMaturita(Vector2 position) { }
    public BossMaturita() : this(Vector2.Zero) { }

    public override void Update(Vector2 playerPosition, double dt) { }

    public override bool ReadyToAttack()
    {
        return false;
    }

    public override void Move(Place place)
    {
    }

    public override bool IsDead()
    {
        return false;
    }

    public override void InitStats(int difficulty)
    {
    }

    public override List<Item> Drop(int looting)
    {
        return null;
    }

    public override Texture2D GetSprite()
    {
        return null;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
    }
    public override void DrawHealthBar(SpriteBatch spriteBatch, Vector2 screenSize)
    { }
}