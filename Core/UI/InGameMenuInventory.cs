//using Microsoft.Xna.Framework.Graphics;
//using Microsoft.Xna.Framework;
//using System.Collections.Generic;
//using Microsoft.Xna.Framework.Input;
//using System.Text;
//using System;

//namespace TBoGV;

//internal class InGameMenuInventory : InGameMenu, IDraw
//{
//	static Viewport Viewport;
//	List<ItemContainer> ItemContainers;
//	static SpriteFont Font;
//	static SpriteFont MiddleFont;
//	static SpriteFont LargerFont;
//	static Texture2D SpriteForeground;
//	static Texture2D SpriteToolTip;
//	Inventory Inventory;
//	ItemContainer? hoveredItem;

//	public InGameMenuInventory(Viewport viewport)
//	{
//		Viewport = viewport;
//		SpriteBackground = TextureManager.GetTexture("blackSquare");
//		SpriteForeground = TextureManager.GetTexture("whiteSquare");
//		SpriteToolTip = TextureManager.GetTexture("container");
//		Font = FontManager.GetFont("Arial8");
//		MiddleFont = FontManager.GetFont("Arial12");
//		LargerFont = FontManager.GetFont("Arial24");
//		Active = false;
//	}

//	public override void Update(Viewport viewport, Player player, MouseState mouseState)
//	{
//		base.Update(viewport, player, mouseState);
//		player.Inventory.Update(mouseState);
//		ItemContainers = player.Inventory.BasicItemContainers;
//		foreach (var item in player.Inventory.SpecialItemContainers)
//		{
//			ItemContainers.Add(item);
//		}
//		hoveredItem = null; // Reset hovered item

//		Vector2 menuCenter = new Vector2(viewport.Width / 2, viewport.Height / 2);
//		for (int i = 0; i < ItemContainers.Count; i++)
//		{
//			Vector2 containerPosition = menuCenter + new Vector2((i - ItemContainers.Count / 2) * 60, 0);
//			ItemContainers[i].SetPosition(containerPosition);
//			if (ItemContainers[i].Selected && !ItemContainers[i].IsEmpty())
//				hoveredItem = ItemContainers[i];
//			// Check if mouse is over an item
//			if (ItemContainers[i].GetRectangle().Contains(mouseState.Position) && !ItemContainers[i].IsEmpty())
//			{
//				hoveredItem = ItemContainers[i];
//			}
//		}
//		Inventory = player.Inventory;
//	}

//	public override void Draw(SpriteBatch spriteBatch)
//	{
//		base.Draw(spriteBatch);


//		// Call Inventory's Draw method instead of handling it here
//		Inventory.Draw(spriteBatch, menuCenter);

//		// Draw tooltip if an item is hovered

//	}
//}
