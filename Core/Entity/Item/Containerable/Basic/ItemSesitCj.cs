﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemSesitCj : ItemContainerable
{
    static Texture2D Sprite;
    public ItemSesitCj(Vector2 position)
    {
        Rarity = 2;
        Position = position;
        Size = new Vector2(50, 50);
        Name = "Sešit češtiny";
        Description = "Ideálně ho nezahazuj.";
        Stats = new Dictionary<StatTypes, float>() { { StatTypes.ATTACK_SPEED, 3 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("sesitCerveny");
        ItemType = ItemTypes.BASIC;
    }
    public ItemSesitCj() : this(Vector2.Zero) { }
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
        return new ItemSesitCj();
    }
}



