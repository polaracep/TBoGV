using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV
{
    class InGameMenuShop : InGameMenu
    {
        private static Viewport Viewport;
        private static SpriteFont MiddleFont;
		private ButtonImage ButtonReroll;
        private List<ItemContainerable> SarkaItemPool = ItemDatabase.GetAllItems();

        private List<ItemContainerable> PerlounItemPool = new List<ItemContainerable>
            {
                new ItemDoping(), 
            };

        // A helper class to hold a shop item and its price.
        private class ShopItem
        {
            public ItemContainerable Item { get; set; }
            public int Price { get; set; }
            public ShopItem(ItemContainerable item, int price)
            {
                Item = item;
                Price = price;
            }
            public ShopItem Clone()
            {
                return new ShopItem(Item.Clone(), Price);
            }
        }

        // Pool of all available shop items.
        private List<ShopItem> shopItemsPool = new List<ShopItem>();
        // The three items currently offered in the shop.
        private List<ShopItem> currentShopItems = new List<ShopItem>();
        private static List<ShopItem> itemCache = new List<ShopItem>();
		private static List<ButtonImage> buttonShopItems = new List<ButtonImage>();

        private KeyboardState previousKeyboardState;
        private ShopState prevShopState = ShopState.SARKA;
        private int resetCount = 0;
        private int maxResetCount = 1;
        public InGameMenuShop(Viewport viewport, Player player)
        {
            Viewport = viewport;
            SpriteBackground = TextureManager.GetTexture("blackSquare");
            MiddleFont = FontManager.GetFont("Arial12");
            Active = false;
            for (int i = 0; i < SarkaItemPool.Count; i++)
            {
                if (SarkaItemPool[i] is ItemDoping)
                    SarkaItemPool.RemoveAt(i);
            }

            InitializeShopItems(ShopState.SARKA);
			ButtonReroll = new ButtonImage("  1¢", FontManager.GetFont("Arial24"),() => 
			{
				if (player.Coins < 1)
					return;
				ClearShop();
                resetCount++;
				OpenMenu(player, ShopState.SARKA); 
				player.Coins--;
			}, TextureManager.GetTexture("reroll"), ImageOrientation.LEFT);
			ButtonReroll.SetTextColor(Color.Yellow);
			ButtonReroll.SetSize(new Vector2(160, ButtonReroll.GetRect().Height));
        }

        // Fill the shop item pool with example items and their prices.
        private void InitializeShopItems(ShopState shopState)
        {
            shopItemsPool.Clear();
            itemCache.Clear();
            prevShopState = shopState;

            List<ItemContainerable> allItems;
            switch (shopState)
            {
                case ShopState.CLOSE:
                    allItems = SarkaItemPool;
                    break;
                case ShopState.SARKA:
                    allItems = SarkaItemPool;
                    break;
                case ShopState.PERLOUN:
                    allItems = PerlounItemPool;
                    break;
                default:
                    allItems = SarkaItemPool;
                    break;
            }
            // Now using a foreach loop to add items to the shopItemsPool
            foreach (var item in allItems)
            {
                // Add the item to the shopItemsPool with its cost from GetCost()
                shopItemsPool.Add(new ShopItem(item, item.GetCost()));
            }

        }
        public void ResetShop()
        {
            itemCache.Clear();
            resetCount = 0;
        }
        public void OpenMenu(Player player, ShopState shopState)
        {
            if(prevShopState != shopState)
                InitializeShopItems(shopState);

            switch (shopState)
            {
                case ShopState.CLOSE:
                    break;
                case ShopState.SARKA:
                    OpenSarkaMenu(player);
                    break;
                case ShopState.PERLOUN:
                    OpenPerlounMenu(player);
                    break;
                default:
                    break;
            }

        }
        public void OpenPerlounMenu(Player player)
        {
            if (itemCache.Count != 0)
                currentShopItems = itemCache;
            else
            {
                Random random = new Random();
                currentShopItems = shopItemsPool.OrderBy(x => random.Next()).Take(1).ToList();

                if (player.Inventory.GetEffect().Contains(EffectTypes.EXPENSIVE))
                {
                    for (int i = 0; i < currentShopItems.Count; i++)
                    {
                        double multiplier = 2 + random.NextDouble() * 0.7;
                        currentShopItems[i] = new ShopItem(currentShopItems[i].Item, (int)(currentShopItems[i].Price * multiplier));
                    }
                }

                itemCache = currentShopItems;
            }
			Active = true;
			ShopButtonInit(player);
		}
		public void OpenSarkaMenu(Player player)
        {
            if (itemCache.Count != 0)
            {
                currentShopItems = itemCache;
            }
            else
            {
                Random random = new Random();
                currentShopItems = shopItemsPool.OrderBy(x => random.Next()).Take(3).ToList();

                if (player.Inventory.GetEffect().Contains(EffectTypes.EXPENSIVE))
                {
                    for (int i = 0; i < currentShopItems.Count; i++)
                    {
                        double multiplier = 2 + random.NextDouble() * 0.7;
                        currentShopItems[i] = new ShopItem(currentShopItems[i].Item, (int)(currentShopItems[i].Price * multiplier));
                    }
                }

                itemCache = currentShopItems;
            }
            Active = true;
			ShopButtonInit(player);
        }
		public void ShopButtonInit(Player player)
		{
			buttonShopItems.Clear();
			foreach (var item in itemCache)
			{
				buttonShopItems.Add(new ButtonImage(Convert.ToString(item.Price) + "¢", FontManager.GetFont("Arial24"), () =>
				{
					if (player.Coins < item.Price)
						return;

					ShopItem itemClone = item.Clone();
					ItemContainerable itemToDrop = null;
					if (!player.Inventory.PickUpItem(itemClone.Item))
						itemToDrop = (player.Inventory.SwapItem(itemClone.Item));
					float hp = player.Hp;
					player.SetStats();
					if (player.MaxHp <= 0)
					{
						player.Inventory.AddEffect(new EffectCloseCall());
						player.Inventory.RemoveItem(itemClone.Item);
						if (itemToDrop != null)
							player.Inventory.PickUpItem(itemToDrop);
						player.Hp = hp;
						player.SetStats();
					}
					else
					{
						if (itemToDrop != null)
							player.Drop(itemToDrop);
						player.Coins -= itemClone.Price;
						itemCache.Remove(item);
					}
					Active = false; // Close shop after purchase.
				}, item.Item.GetSprite(), ImageOrientation.TOP));
			}
			foreach (var b in buttonShopItems)

			{
				b.SetSize(new Vector2(200, 200));
				b.SetTextColor(Color.Yellow);
			}
		}
        public void ClearShop()
        {
            itemCache.Clear();
        }

        public override void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, double dt)
        {
            base.Update(viewport, player, mouseState, keyboardState, dt);

            if (!Active)
                return;

            // Not clearing the cache here
            if (keyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
            {
                Active = false;
                return;
            }

			foreach (var b in buttonShopItems)
				b.Update(mouseState);

            if(prevShopState == ShopState.SARKA && resetCount < maxResetCount)
			    ButtonReroll.Update(mouseState);
            previousKeyboardState = keyboardState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Active)
                return;

            base.Draw(spriteBatch);

            int totalWidth = 0;
			int boxHeight = 0;



            for (int i = 0; i < buttonShopItems.Count; i++)
            {
				totalWidth += buttonShopItems[i].GetRect().Width;
				boxHeight = Math.Max(boxHeight, buttonShopItems[i].GetRect().Height); 
			}
			totalWidth += (buttonShopItems.Count - 1) * 20;
			int startX = (Viewport.Width - totalWidth) / 2;
			int posY = (Viewport.Height - boxHeight) / 2;

			for (int i = 0; i < buttonShopItems.Count; i++)
			{
				buttonShopItems[i].Position = new Vector2(startX, posY);
				buttonShopItems[i].Draw(spriteBatch);
				startX += buttonShopItems[i].GetRect().Width + 20;
			}


			// Reroll button placement
			if (prevShopState == ShopState.SARKA && resetCount< maxResetCount)
            {
				int buttonX = (Viewport.Width - ButtonReroll.GetRect().Width) / 2;
				int buttonY = posY + boxHeight + 20;
				ButtonReroll.Position = new Vector2(buttonX, buttonY);
                ButtonReroll.Draw(spriteBatch);
            }
        }
    }
}
