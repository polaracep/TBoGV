using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemLabcoat : ItemContainerable
{
    static Texture2D Sprite;
    public ItemLabcoat(Vector2 position)
    {
        Rarity = 2;
        Position = position;
        Size = new Vector2(50, 50);
        Name = "Laboratorní plášť";
        Description = "Měl by tě ochránit, ale nezakáže ti vypít kádinku.";
        Stats = new Dictionary<StatTypes, float>() { { StatTypes.MAX_HP, 2 }, { StatTypes.MOVEMENT_SPEED, -1 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("labCoat");
        ItemType = ItemTypes.BASIC;
    }
    public ItemLabcoat() : this(Vector2.Zero) { }
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
        return new ItemLabcoat();
    }
}



