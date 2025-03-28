using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemSesitFyzika : ItemContainerable
{
    static Texture2D Sprite;
    public ItemSesitFyzika(Vector2 position)
    {
        Rarity = 2;
        Position = position;
        Size = new Vector2(50, 50);
        Name = "Sešit fyziky";
        Description = "Prochy je bude kontrolovat, tak si ho doplň.";
        Stats = new Dictionary<StatTypes, float>() { { StatTypes.PROJECTILE_COUNT, 2 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("sesitCerveny");
        ItemType = ItemTypes.BASIC;
    }
    public ItemSesitFyzika() : this(Vector2.Zero) { }
    public override Texture2D GetSprite()
    {
        return Sprite;
    }
    public override ItemContainerable Clone()
    {
        return new ItemSesitFyzika();
    }
}



