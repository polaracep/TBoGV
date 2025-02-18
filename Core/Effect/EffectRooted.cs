using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace TBoGV;

internal class EffectRooted : Effect
{
	protected static Texture2D Sprite = TextureManager.GetTexture("koren");
	public EffectRooted(int level)
	{
		Name = "Korenovy vezen";
		Description = "bouchani nikdo neslysel, \nOdskocit jsi si nemohl, takze te zahriva tepla moc";
		Positive = false;
		Stats = new Dictionary<StatTypes, int>() { };
		Effects = new List<EffectTypes> { EffectTypes.ROOTED};
		Level = 0;
		ChangeLevel(level);
	}
	public override void ChangeLevel(int delta)
	{
		Level += delta;
		UpdateSize();
	}

	public override Texture2D GetSprite()
	{
		return Sprite;
	}
	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);
		spriteBatch.Draw(Sprite, new Vector2(Border + Position.X, Border / 2 + Position.Y), Color.White);
	}
}

