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
        }

        // Pool of all available shop items.
        private List<ShopItem> shopItemsPool = new List<ShopItem>();
        // The three items currently offered in the shop.
        private List<ShopItem> currentShopItems = new List<ShopItem>();
        private static List<ShopItem> itemCache = new List<ShopItem>();

        // Rectangles representing the clickable areas for the 3 boxes.
        private Rectangle[] boxBounds = new Rectangle[3];
        private int hoveredBox = -1;
        private MouseState previousMouseState;
        private KeyboardState previousKeyboardState;

        public InGameMenuShop(Viewport viewport)
        {
            Viewport = viewport;
            SpriteBackground = TextureManager.GetTexture("blackSquare");
            MiddleFont = FontManager.GetFont("Arial12");
            Active = false;
            InitializeShopItems();
        }

        // Fill the shop item pool with example items and their prices.
        private void InitializeShopItems()
        {
			// List to hold all the items (add your actual items here)
			List<ItemContainerable> allItems = new List<ItemContainerable>
{
	new ItemCalculator(),
	new ItemDoping(),
	new ItemMonster(),
	new ItemFancyShoes(),
	new ItemFlipFlop(),
	new ItemTrackShoes(),
	new ItemMap(),
	new ItemMathProblem(),
	new ItemTeeth(),
	new ItemAdBlock(),
	new ItemExplosive(),
	new ItemDagger(),
	new ItemPencil(),
	new ItemBookBio(),
	new ItemBookCzech(),
	new ItemBookMath(),
	new ItemBookZsv(),
	new ItemBookPhysics(),
	new ItemCross(),
	new ItemBryle(),
	new ItemBook(),
	new ItemBookPE(),
	new ItemPen(),
	new ItemScissors(),
	new ItemRubbedBoots(),
	new ItemFixa(),
};

			// Now using a foreach loop to add items to the shopItemsPool
			foreach (var item in allItems)
			{
				// Add the item to the shopItemsPool with its cost from GetCost()
				shopItemsPool.Add(new ShopItem(item, item.GetCost()));
			}

		}

		// Opens the shop by randomly selecting 3 items from the pool.
		public void OpenMenu()
        {
            if (itemCache.Count != 0)
                currentShopItems = itemCache;
            else
            {
                Random random = new Random();
                currentShopItems = shopItemsPool.OrderBy(x => random.Next()).Take(3).ToList();
                itemCache = currentShopItems;
            }
            Active = true;
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

            hoveredBox = -1; // Reset hover state.

            Point mousePos = mouseState.Position;
            for (int i = 0; i < currentShopItems.Count; i++)
            {
                if (boxBounds[i].Contains(mousePos))
                {
                    hoveredBox = i;
                    // Check for a click (release after press).
                    if (previousMouseState.LeftButton == ButtonState.Pressed &&
                        mouseState.LeftButton == ButtonState.Released)
                    {
                        // "Purchase" the item: add it to the player's inventory.
                        if (player.Coins < currentShopItems[i].Price)
                            return;

                        player.Inventory.PickUpItem(currentShopItems[i].Item);
                        // Optionally, you could deduct money from the player here.

                        itemCache.Clear();
                        Active = false; // Close shop after purchase.
                        return;
                    }
                }
            }
            previousMouseState = mouseState;
            previousKeyboardState = keyboardState;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!Active)
                return;

            base.Draw(spriteBatch);

            // Layout the 3 boxes horizontally centered.
            int boxWidth = 100;    // Fixed width for each shop box.
            int boxHeight = 100;   // Fixed height for each shop box.
            int spacing = 20;      // Space between boxes.
            int totalWidth = 3 * boxWidth + 2 * spacing;
            int startX = (Viewport.Width - totalWidth) / 2;
            int posY = (Viewport.Height - boxHeight) / 2;

            for (int i = 0; i < currentShopItems.Count; i++)
            {
                int boxX = startX + i * (boxWidth + spacing);
                Rectangle boxRect = new Rectangle(boxX, posY, boxWidth, boxHeight);
                boxBounds[i] = boxRect;

                // Draw the box background; change color if hovered.
                Color boxColor = (hoveredBox == i) ? Color.Gray * 0.8f : Color.Black * 0.5f;
                spriteBatch.Draw(SpriteBackground, boxRect, boxColor);

                // Draw the item centered in the box.
                ShopItem shopItem = currentShopItems[i];
                Vector2 itemSize = shopItem.Item.Size;
                Vector2 itemPos = new Vector2(
                    boxRect.X + (boxWidth - itemSize.X) / 2,
                    boxRect.Y + (boxHeight - itemSize.Y) / 2 - 10  // Slightly upward to leave room for price
                );
                shopItem.Item.Position = itemPos;
                shopItem.Item.Draw(spriteBatch);

                // Draw the price text below the item.
                string priceText = $"${shopItem.Price}";
                Vector2 priceSize = MiddleFont.MeasureString(priceText);
                Vector2 pricePos = new Vector2(
                    boxRect.X + (boxWidth - priceSize.X) / 2,
                    boxRect.Y + boxHeight - priceSize.Y - 5
                );
                spriteBatch.DrawString(MiddleFont, priceText, pricePos, Color.Yellow);
            }
        }
    }
}
