﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

    internal class ItemPencil: ItemContainerable
    {
    static Texture2D Sprite;
    public ItemPencil(Vector2 position)
    {
        Position = position;
        Size = new Vector2(50, 50);
        Name = "Tuzka";
        Description = "cerstve naostrena";
        Stats = new Dictionary<StatTypes, int>() { { StatTypes.DAMAGE, 1 }, { StatTypes.ATTACK_SPEED, 500 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("tile");
        ItemType = ItemTypes.WEAPON;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color.White);
    }
}



