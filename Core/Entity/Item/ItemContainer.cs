﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;

namespace TBoGV;

public class ItemContainer : IDraw
{
    static Texture2D SpriteContainer;
    static Texture2D SpriteContainerBorder;
    public Vector2 Position { get; private set; }
    public static Vector2 Size;
    public bool Selected;
    public Item Item { get; set; }
    public ItemContainer()
    {
        SpriteContainer = TextureManager.GetTexture("container");
        SpriteContainerBorder = TextureManager.GetTexture("containerBorder");
        Size = new Vector2(50, 50);
        Selected = false;
    }
    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(SpriteContainer, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), Color.Beige);
        if (!IsEmpty())
            Item.Draw(spriteBatch);
        if (Selected)
            spriteBatch.Draw(SpriteContainerBorder, new Rectangle(Convert.ToInt32(Position.X), Convert.ToInt32(Position.Y), Convert.ToInt32(Size.X), Convert.ToInt32(Size.Y)), new Color(200, 30, 30, 180));
    }
    public void SetPosition(Vector2 postition)
    {
        Position = postition;
        if(!IsEmpty())
            Item.Position = Position;
    }
    public bool IsEmpty()
    {
        return Item == null;
    }
}

