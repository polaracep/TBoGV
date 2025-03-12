using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemSesitZeleny : ItemContainerable
{
    static Texture2D Sprite;
    public ItemSesitZeleny(Vector2 position)
    {
        Rarity = 1;
        Position = position;
        Size = new Vector2(50, 50);
        Name = "Čtverečkovaný sešit";
        Description = "Ideální na geometrické konstrukce";
        Stats = new Dictionary<StatTypes, float>() { { StatTypes.DAMAGE, 2 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("sesitZeleny");
        ItemType = ItemTypes.BASIC;
    }
    public ItemSesitZeleny() : this(Vector2.Zero) { }
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
    }
    public override Texture2D GetSprite()
    {
        return Sprite;
    }
    public override ItemContainerable Clone()
    {
        return new ItemSesitZeleny();
    }
}



