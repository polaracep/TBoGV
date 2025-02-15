﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

internal class ItemCalculator : ItemContainerable
{
    static Texture2D Sprite;
    public ItemCalculator(Vector2 position)
    {
        Position = position;
        Size = new Vector2(50, 50);
        Name = "Kalkulacka";
        Description = "Rychla schovankovo opravovani testu";
        Stats = new Dictionary<StatTypes, int>() { { StatTypes.DAMAGE, 4 }};
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("koren");
        ItemType = ItemTypes.BASIC;
    }
    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color.White);
    }
}

