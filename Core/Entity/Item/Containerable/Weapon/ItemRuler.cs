﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemRuler : ItemContainerable
{
    static Texture2D Sprite;
    public ItemRuler(Vector2 position)
    {
        Rarity = 1;
        Position = position;
        Size = new Vector2(50, 50);
        Name = "Pravoúhlý trojúhelník";
        Description = "Nevypadá na to, ale je křehký.";
        Stats = new Dictionary<StatTypes, float>() { { StatTypes.DAMAGE, 1.5f }, { StatTypes.ATTACK_SPEED, 1400 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("pravyuhel");
        ItemType = ItemTypes.WEAPON;
    }
    public ItemRuler() : this(Vector2.Zero) { }
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
        return new ItemRuler();
    }
}



