﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Diagnostics.CodeAnalysis;

namespace TBoGV;

[DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicConstructors)]
class ItemPencil : ItemContainerable
{
    static Texture2D Sprite;
    public ItemPencil() : this(Vector2.Zero) { }
    public ItemPencil(Vector2 position)
    {
        Rarity = 1;
        Position = position;
        Size = new Vector2(50, 50);
        Name = "Tužka";
        Description = "Čerstvě naostřená";
        Stats = new Dictionary<StatTypes, float>() { { StatTypes.DAMAGE, 1 }, { StatTypes.ATTACK_SPEED, 1250 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("pencil");
        ItemType = ItemTypes.WEAPON;
    }
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
        return new ItemPencil();
    }
}



