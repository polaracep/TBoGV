using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemOrezavatko : ItemContainerable
{
    static Texture2D Sprite;
    public ItemOrezavatko(Vector2 position)
    {
        Rarity = 2;
        Position = position;
        Size = new Vector2(50, 50);
        Name = "Ořezávátko";
        Description = "S ostrýma tužkama se lépe píše";
        Stats = new Dictionary<StatTypes, float>() { { StatTypes.MAX_HP, 1 }, { StatTypes.DAMAGE, 1 }, { StatTypes.ATTACK_SPEED, 1 }, { StatTypes.PROJECTILE_COUNT, 1 }, { StatTypes.XP_GAIN, 1 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("sharpener");
        ItemType = ItemTypes.BASIC;
    }
    public ItemOrezavatko() : this(Vector2.Zero) { }
    public override Texture2D GetSprite()
    {
        return Sprite;
    }
    public override ItemContainerable Clone()
    {
        return new ItemOrezavatko();
    }
}



