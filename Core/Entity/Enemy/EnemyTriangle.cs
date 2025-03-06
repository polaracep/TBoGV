﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using Microsoft.Xna.Framework.Audio;


namespace TBoGV;
class EnemyTriangle : EnemyMelee
{
    static Texture2D Spritesheet = TextureManager.GetTexture("triangleSpritesheet");
    //static SoundEffect vibeSfx = SoundManager.GetSound("vibe");
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
    }
    public override Texture2D GetSprite()
    {
        return Spritesheet;
    }
    public override void Update(Vector2 playerPosition, double dt)
    {
        base.Update(playerPosition, dt);
        lastFrameChangeElapsed += dt;
        UpdateAnimation();
    }
    protected override void UpdateMoving(double dt)
    {
        if ((phaseChangeElapsed > movingDuration && Moving) ||
            (phaseChangeElapsed > chillDuration && !Moving))
        {
            Moving = !Moving;
            phaseChangeElapsed = 0;
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

    protected override void InitStats(int difficulty)
    {
        Hp = 3 * difficulty;
        MovementSpeed = difficulty;
        AttackSpeed = 444;
        AttackDmg = 1;
        XpValue = difficulty / 4;
        Weight = EnemyWeight.EASY;
    }
}

