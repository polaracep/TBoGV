using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TBoGV;

internal class EffectCooked : Effect
{
	protected static Texture2D Sprite = TextureManager.GetTexture("cooked");
	private List<StatTypes> allStats = new List<StatTypes>() 
	{
		StatTypes.DAMAGE, 
		StatTypes.MOVEMENT_SPEED, 
		StatTypes.MAX_HP, 
		StatTypes.XP_GAIN, 
		StatTypes.PROJECTILE_COUNT,
		StatTypes.ATTACK_SPEED
	};
	public EffectCooked(int level) 
	{
		Name = "Cooked";
		Description = "nandmerna konzumace brainrotu, \nnebo jsi se pokousel pochopit TFY";
		Positive = false;
		Stats = new Dictionary<StatTypes, int>();
		Level = 0;
		ChangeLevel(level);
	}
	public override void ChangeLevel(int delta)
	{
		Level += delta;
		Random rnd = new Random();
		for (int i = 0; i < delta; i++)
		{
			StatTypes stat = allStats[rnd.Next(0, allStats.Count)];
			if(Stats.ContainsKey(stat))
				Stats[stat]--;
			else 
				Stats[stat] = -1;
		}
		UpdateSize();
	}

	public override Texture2D GetSprite()
	{
		return Sprite;
	}
	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		spriteBatch.Draw(Sprite, new Vector2(Border+Position.X, Border / 2 + Position.Y), Color.White);
	}
}

