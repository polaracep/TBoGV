using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemVysvedceni : ItemContainerable
{
    static Texture2D Sprite;
    public ItemVysvedceni(Vector2 position)
    {
        Rarity = 4;
        Position = position;
        Size = new Vector2(50, 50);
        Name = "Maturitní vysvědčení";
        Description = "To se hodí";
        Stats = new Dictionary<StatTypes, float>() {
            { StatTypes.ATTACK_SPEED, 3},
            { StatTypes.DAMAGE, 3},
            { StatTypes.MAX_HP, 3},
            { StatTypes.MOVEMENT_SPEED, 3},
            { StatTypes.PROJECTILE_COUNT, 3},
            { StatTypes.XP_GAIN, 3},
        };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("vyzo");
        ItemType = ItemTypes.BASIC;
    }
    public ItemVysvedceni() : this(Vector2.Zero) { }
    public override Texture2D GetSprite()
    {
        return Sprite;
    }
    public override ItemContainerable Clone()
    {
        return new ItemVysvedceni();
    }
}



