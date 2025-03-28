using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemSesitMatika : ItemContainerable
{
    static Texture2D Sprite;
    public ItemSesitMatika(Vector2 position)
    {
        Rarity = 2;
        Position = position;
        Size = new Vector2(50, 50);
        Name = "Sešit matematiky";
        Description = "Nemáte napočítáno, tak počítejte!";
        Stats = new Dictionary<StatTypes, float>() { { StatTypes.DAMAGE, 3 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("sesitCerveny");
        ItemType = ItemTypes.BASIC;
    }
    public ItemSesitMatika() : this(Vector2.Zero) { }
    public override Texture2D GetSprite()
    {
        return Sprite;
    }
    public override ItemContainerable Clone()
    {
        return new ItemSesitMatika();
    }
}



