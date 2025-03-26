using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace TBoGV;

class EnemyZdena : EnemyRanged
{
	static Texture2D Sprite;
	public EnemyZdena(Vector2 position)
	{
		InitStats(Storyline.Difficulty);
		Position = position;
		Sprite = TextureManager.GetTexture("korenovy_vezen");
		Size = new Vector2(Sprite.Width, Sprite.Height);
		AttackDelay();
	}
	public EnemyZdena() : this(Vector2.Zero) { }
	public override void Draw(SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color.White);
		DrawHealthBar(spriteBatch);
	}
	public override List<Projectile> Attack()
	{
		List<Projectile> projectiles = [];
		projectiles.Add(new ProjectileRoot(Position + Size / 2, Direction, AttackDmg));
		LastAttackElapsed = 0;
		return projectiles;
	}
	public override Texture2D GetSprite()
	{
		return Sprite;
	}

	private static List<SoundEffectInstance> Sfx = [
		SoundManager.GetSound("zdena1").CreateInstance(),
		SoundManager.GetSound("zdena2").CreateInstance(),
		SoundManager.GetSound("zdena3").CreateInstance(),
		SoundManager.GetSound("zaklineno").CreateInstance()];
	private static double ambientElapsed = 0;
	private static double ambientTime = 3000;
	private static bool CanPlaySfx()
	{
		return ambientElapsed > ambientTime;
	}
	private static void PlayAmbientSfx()
	{
		if (!CanPlaySfx())
			return;
		SoundEffectInstance sfx = Sfx[random.Next(Sfx.Count)];
		sfx.Volume = Convert.ToSingle(Settings.SfxVolume.Value);
		if (sfx.State != SoundState.Playing)
			sfx.Play();
		ambientElapsed = 0;
		ambientTime = random.Next(3000, 6000);
	}
	public static void UpdateSfx(double dt)
	{
		ambientElapsed += dt;
		if (CanPlaySfx())
			PlayAmbientSfx();
	}
	public static void StopSfx()
	{
		foreach (SoundEffectInstance sfx in Sfx)
			sfx.Stop();
	}
	public override void Move(Place place)
	{
		return;
	}

	public override void InitStats(int difficulty)
	{
		Hp = 2 + difficulty * 1.5f;
		MovementSpeed = 4;
		AttackSpeed = 2500 - (100 * difficulty);
		AttackDmg = 1;
		XpValue = 1 + difficulty / 2;
		Weight = EnemyWeight.MEDIUM;
		base.InitStats(difficulty);
	}
}
