using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace TBoGV;

public class ItemContainer : Entity, IDraw
{
    static Texture2D SpriteContainerBasic;
    static Texture2D SpriteContainerEffect;
    static Texture2D SpriteContainerArmor;
    static Texture2D SpriteContainerWeapon;
    static Texture2D SpriteContainerBorder;
	public ItemTypes ContainerType { get; set; }
	public bool Selected;
    public ItemContainerable Item { get; set; }
    public ItemContainer()
    {
        SpriteContainerBasic = TextureManager.GetTexture("containerBasic");
        SpriteContainerArmor = TextureManager.GetTexture("containerBoots");
        SpriteContainerEffect = TextureManager.GetTexture("containerEffect");
        SpriteContainerWeapon = TextureManager.GetTexture("containerWeapon");
        SpriteContainerBorder = TextureManager.GetTexture("containerBorder");
        Size = new Vector2(50, 50);
        Selected = false;
        ContainerType = ItemTypes.BASIC;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        Texture2D Sprite;
        switch(ContainerType)
        {
            case ItemTypes.BASIC:
                Sprite = SpriteContainerBasic;
                break;
            case ItemTypes.WEAPON:
                Sprite = SpriteContainerWeapon;
                break;
            case ItemTypes.ARMOR:
                Sprite = SpriteContainerArmor;
                break;
            case ItemTypes.EFFECT:
                Sprite = SpriteContainerEffect;
                break;
                default: Sprite = SpriteContainerBasic; break;
        }
        if (!IsEmpty())
            Sprite = SpriteContainerBasic;
        spriteBatch.Draw(Sprite, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color.Gray);
        if (!IsEmpty())
            Item.Draw(spriteBatch);
        if (Selected)
            spriteBatch.Draw(SpriteContainerBorder, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), new Color(200, 30, 30, 180));
    }
    public void SetPosition(Vector2 position)
    {
        Position = position;
        if(!IsEmpty())
            Item.Position = Position;
    }
    public bool IsEmpty()
    {
        return Item == null;
    }
	public override Texture2D GetSprite()
	{
		return null;
	}

}

