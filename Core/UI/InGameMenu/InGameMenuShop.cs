using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;
public class InGameMenuShop : InGameMenu
{
    private static Viewport Viewport;
    private static SpriteFont MiddleFont = FontManager.GetFont("Arial12");
    private ButtonImage ButtonReroll;

    // A helper class to hold a shop item and its price.

    // The three items currently offered in the shop.
    private List<ShopItem> currentShopItems = new List<ShopItem>();

    // Rectangles representing the clickable areas for the 3 boxes.
    private Rectangle[] boxBounds = new Rectangle[3];
    private int hoveredBox = -1;
    private MouseState previousMouseState;
    private KeyboardState previousKeyboardState;
    // private ShopState prevShopState = ShopState.SARKA;
    private static ShopBase ActiveShop = null;
    private Player player;
    private static int resetCount = 0;
    private const int maxResetCount = 1;
    public InGameMenuShop(Viewport viewport, Player player, ShopTypes type)
    {
        Viewport = viewport;
        this.player = player;
        ActiveShop = type switch
        {
            ShopTypes.SARKA => new ShopSarka(),
            ShopTypes.PERLOUN => new ShopPerloun(),
            _ => throw new Exception(),
        };

        SpriteBackground = TextureManager.GetTexture("blackSquare");

        ButtonReroll = new ButtonImage("$1", FontManager.GetFont("Arial12"), () =>
        {
            if (player.Coins < 1)
                return;
            ActiveShop.ClearCache();
            resetCount++;
            OpenShop();
            player.Coins--;
        }, TextureManager.GetTexture("reroll"), ImageOrientation.LEFT);

        OpenShop();
    }

    public static void ResetShop()
    {
        ActiveShop.ClearCache();
        resetCount = 0;
    }
    public void OpenShop()
    {
        // if there's something in the cache, use it
        if (ActiveShop.GetCachedItems().Count != 0)
            currentShopItems = ActiveShop.GetCachedItems();
        else
        {
            // fill the cache with n random items
            ActiveShop.ItemPool.OrderBy(x => Random.Shared.Next()).Take(ActiveShop.ItemCount).ToList()
                .ForEach(item => currentShopItems.Add(new ShopItem(item, item.GetCost())));

            // if expensive, make them more expensive
            if (player.Inventory.GetEffect().Contains(EffectTypes.EXPENSIVE))
                currentShopItems.ForEach(x => x.Price *= (int)(2 + Random.Shared.NextDouble() * 0.7));

            ActiveShop.SetCachedItems(currentShopItems);
        }
    }

    public override void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, double dt)
    {
        base.Update(viewport, player, mouseState, keyboardState, dt);

        // Not clearing the cache here
        if (keyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
        {
            CloseMenu.Invoke();
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

                    ShopItem itemClone = currentShopItems[i].Clone();
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
                        ActiveShop.GetCachedItems().Remove(currentShopItems[i]);
                    }
                    CloseMenu.Invoke();
                    return;
                }
            }
        }
        if (ActiveShop is ShopSarka && resetCount < maxResetCount)
            ButtonReroll.Update(mouseState);
        previousMouseState = mouseState;
        previousKeyboardState = keyboardState;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);

        // Layout the 3 boxes horizontally centered.
        int boxWidth = 100;    // Fixed width for each shop box.
        int boxHeight = 100;   // Fixed height for each shop box.
        int spacing = 20;      // Space between boxes.
        int totalWidth = ActiveShop.ItemCount * boxWidth + ActiveShop.ItemCount - 1 * spacing;
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

        // Reroll button placement
        int buttonX = (Viewport.Width - ButtonReroll.GetRect().Width) / 2;
        int buttonY = posY + boxHeight + 20;  // Positioned below the shop boxes
        if (ActiveShop is ShopSarka && resetCount < maxResetCount)
        {
            ButtonReroll.Position = new Vector2(buttonX, buttonY);
            ButtonReroll.Draw(spriteBatch);
        }
    }

}

public class ShopItem
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
public abstract class ShopBase
{
    public abstract List<ItemContainerable> ItemPool { get; set; }
    public static List<ShopItem> ItemCache { get; set; } = new List<ShopItem>();
    public abstract int ItemCount { get; protected set; }
    public void ClearCache()
    {
        ItemCache.Clear();
    }
    public List<ShopItem> GetCachedItems()
    {
        return ItemCache;
    }
    public void SetCachedItems(List<ShopItem> cache)
    {
        ItemCache = cache;
    }
}
public abstract class Shop<T> : ShopBase where T : Shop<T> { }
public class ShopSarka : Shop<ShopSarka>
{
    public override List<ItemContainerable> ItemPool { get; set; } = ItemDatabase.GetAllItems().Where(x => x is not ItemDoping).ToList();
    public override int ItemCount { get; protected set; } = 3;
}
public class ShopPerloun : Shop<ShopPerloun>
{
    public override int ItemCount { get; protected set; } = 1;
    public override List<ItemContainerable> ItemPool { get; set; } = [
        new ItemDoping(),
        ];
}
public enum ShopTypes : int
{
    SARKA,
    PERLOUN,
}