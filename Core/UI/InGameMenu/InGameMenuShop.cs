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
	private List<ButtonImage> Buttons = new List<ButtonImage>();

    private KeyboardState previousKeyboardState;

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

        ButtonReroll = new ButtonImage("  1¢", FontManager.GetFont("Arial12"), () =>
        {
            if (player.Coins < 1)
                return;
            ActiveShop.ClearCache();
            resetCount++;
            OpenShop();
            player.Coins--;
        }, TextureManager.GetTexture("reroll"), ImageOrientation.LEFT);

		ButtonReroll.SetTextColor(Color.Yellow);
		ButtonReroll.SetSize(new Vector2(160, ButtonReroll.GetRect().Height));
		OpenShop();
    }

    public static void ResetShop()
    {
        if (ActiveShop != null)
            ActiveShop.ClearCache();
        resetCount = 0;
    }
    public void OpenShop()
    {
		Buttons.Clear();
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
		foreach (var item in currentShopItems)
		{
			Buttons.Add(new ButtonImage(Convert.ToString(item.Price) + "c", MiddleFont, () =>
			{
				// "Purchase" the item: add it to the player's inventory.
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
					{
						itemToDrop.InitMovement();
						player.Drop(itemToDrop);
					}
					player.Coins -= itemClone.Price;
					ActiveShop.GetCachedItems().Remove(item);
				}

				CloseMenu.Invoke();
			}, item.Item.GetSprite(),ImageOrientation.TOP));
		}
		foreach (var b in Buttons)
		{
			b.SetTextColor(Color.Yellow);
			b.SetSize(new Vector2(100,100));
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

        if (ActiveShop is ShopSarka && resetCount < maxResetCount)
            ButtonReroll.Update(mouseState);
		foreach (var b in Buttons)
			b.Update(mouseState);
		
        previousKeyboardState = keyboardState;
    }

	public override void Draw(SpriteBatch spriteBatch)
	{
		base.Draw(spriteBatch);

		int totalWidth = 0;
		int boxHeight = 0;



		for (int i = 0; i < Buttons.Count; i++)
		{
			totalWidth += Buttons[i].GetRect().Width;
			boxHeight = Math.Max(boxHeight, Buttons[i].GetRect().Height);
		}
		totalWidth += (Buttons.Count - 1) * 20;
		int startX = (Viewport.Width - totalWidth) / 2;
		int posY = (Viewport.Height - boxHeight) / 2;

		for (int i = 0; i < Buttons.Count; i++)
		{
			Buttons[i].Position = new Vector2(startX, posY);
			Buttons[i].Draw(spriteBatch);
			startX += Buttons[i].GetRect().Width + 20;
		}


		// Reroll button placement
		if (ActiveShop is ShopSarka && resetCount < maxResetCount)
		{
			int buttonX = (Viewport.Width - ButtonReroll.GetRect().Width) / 2;
			int buttonY = posY + boxHeight + 20;
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