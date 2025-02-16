using System;
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
        Stats = new Dictionary<StatTypes, int>() { { StatTypes.DAMAGE, 2 }, { StatTypes.ATTACK_SPEED, 750 } };
        Effects = new List<EffectTypes>();
        Sprite = TextureManager.GetTexture("pencil");
        ItemType = ItemTypes.WEAPON;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), IsKnown ? Color.White : Color.Black);
    }
}



