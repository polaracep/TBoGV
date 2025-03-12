﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemSesitModry : ItemContainerable
{
    static Texture2D Sprite;
    public ItemSesitModry(Vector2 position)
    {
        Rarity = 2;
        Position = position;
        Size = new Vector2(50, 50);
        Name = "Sešit biologie";
        Description = "Bez linek. Ideální na nákresy.";
        Stats = new Dictionary<StatTypes, float>() { { StatTypes.MAX_HP, 3 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("sesitModry");
        ItemType = ItemTypes.BASIC;
    }
    public ItemSesitModry() : this(Vector2.Zero) { }
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
        return new ItemSesitModry();
    }
}



