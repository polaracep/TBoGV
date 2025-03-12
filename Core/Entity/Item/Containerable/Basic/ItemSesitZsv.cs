using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace TBoGV;

public class ItemSesitZsv : ItemContainerable
{
    static Texture2D Sprite;
    public ItemSesitZsv(Vector2 position)
    {
        Rarity = 2;
        Position = position;
        Size = new Vector2(50, 50);
        Name = "Sešit zsv";
        Description = "Obsahuje určitě mnoho písmenek, ale ještě víc kreseb.\nMožná bys měl začít zapisovat.";
        Stats = new Dictionary<StatTypes, float>() { { StatTypes.DAMAGE, 3 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("sesitCerveny");
        ItemType = ItemTypes.BASIC;
    }
    public ItemSesitZsv() : this(Vector2.Zero) { }
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
        return new ItemSesitZsv();
    }
}



