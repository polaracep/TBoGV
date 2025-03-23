using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Audio;


namespace TBoGV;
class EnemyTriangle : EnemyMelee
{
    static Texture2D Spritesheet = TextureManager.GetTexture("triangleSpritesheet");

    public static SoundEffectInstance Sfx = SoundManager.GetSound("triangle").CreateInstance();
    static double SfxDuration = SoundManager.GetSound("triangle").Duration.TotalMilliseconds;
    protected double SfxTimeElapsed = SfxDuration;
    static float Scale;

    int frameWidth = 98;
    int frameHeight = 103;
    int frameCount = 32;
    int currentFrame = 0;
    double lastFrameChangeElapsed = 0;
    double frameSpeed = 3200 / 32;

    public EnemyTriangle(Vector2 position)
    {
        InitStats(Storyline.Difficulty);
        movingDuration = 3200;
        chillDuration = 0;
        Position = position;
        Scale = 50f / Math.Max(frameWidth, frameHeight);
        Size = new Vector2(frameWidth * Scale, frameHeight * Scale);

        AttackDelay();

    }
    public EnemyTriangle() : this(Vector2.Zero) { }

    public override void Draw(SpriteBatch spriteBatch)
    {
        Rectangle sourceRect = new Rectangle(currentFrame * frameWidth, 0, frameWidth, frameHeight);
        spriteBatch.Draw(Spritesheet, new Rectangle((int)Position.X, (int)Position.Y, (int)(Size.X), (int)(Size.Y)), sourceRect, Color.White);
		DrawHealthBar(spriteBatch);
	}
    public override Texture2D GetSprite()
    {
        return Spritesheet;
    }
    public override void Update(Vector2 playerPosition, double dt)
    {
        Sfx.Volume = Convert.ToSingle(Settings.SfxVolume.Value);
        base.Update(playerPosition, dt);
        lastFrameChangeElapsed += dt;
        SfxTimeElapsed += dt;
        UpdateSfx();
        UpdateAnimation();
    }
    protected override void UpdateMoving(double dt)
    {
        if (Sfx.State == SoundState.Stopped)
            Sfx.Resume();
        if ((phaseChangeElapsed > movingDuration && Moving) ||
            (phaseChangeElapsed > chillDuration && !Moving))
        {
            Moving = !Moving;
            phaseChangeElapsed = 0;
        }
    }
    private void UpdateSfx()
    {
        if (SfxTimeElapsed >= SfxDuration)
        {
            Sfx.Play();
            SfxTimeElapsed = 0;
        }
    }
    private void UpdateAnimation()
    {
        if (lastFrameChangeElapsed > frameSpeed)
        {
            currentFrame = (currentFrame + 1) % frameCount;
            lastFrameChangeElapsed = 0;
        }
        if (!Moving)
            currentFrame = 0;
    }

    public override void InitStats(int difficulty)
    {
        Hp = difficulty * 2f + 1f;
        MovementSpeed = 1.2f;
        AttackSpeed = 444;
        AttackDmg = 1;
        XpValue = 1 + difficulty / 2;
        Weight = EnemyWeight.EASY;
		base.InitStats(difficulty);
	}
}

