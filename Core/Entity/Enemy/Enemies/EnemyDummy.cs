using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TBoGV;
class EnemyDummy : Enemy
{
    public static Texture2D Sprite;
    public EnemyDummy(Vector2 position)
    {
        InitStats(Storyline.Difficulty);
        Position = position;
        Sprite = TextureManager.GetTexture("wiseman");

        Size = new Vector2(50);
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Sprite, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), Color.White);
    }

    public override Texture2D GetSprite() { return Sprite; }
    public override bool IsDead() { return Hp <= 0; }

    public override void Move(Place place) { }

    public override bool ReadyToAttack() { return false; }

    public override void Update(Vector2 playerPosition, double dt) { }

    protected override void InitStats(int difficulty)
    {
        Hp = 3 + (int)(1.5 * difficulty);
        MovementSpeed = 0;
        AttackSpeed = 1000000;
        AttackDmg = 0;
        XpValue = 0;
        Weight = EnemyWeight.EASY;
    }
}
