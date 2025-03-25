using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBoGV;

public class ScreenDeath : Screen
{
	private SpriteFont LargerFont = FontManager.GetFont("Arial16");
	private Viewport viewport;
	private Button backButton;

	public override void BeginRun(GraphicsDeviceManager graphics)
	{
		// Create a button to go back to the main menu.
		backButton = new Button("Zpátky na začátek", LargerFont, () =>
		{
			TBoGVGame.screenCurrent = ScreenManager.ScreenStart;
			Storyline.Player.Inventory.RemoveEffect(new EffectEndless());
		});
	}

	public override void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
	{
		viewport = graphics.GraphicsDevice.Viewport;

		// Start the sprite batch.
		_spriteBatch.Begin();

		_spriteBatch.Draw(TextureManager.GetTexture("gymvod"), new Rectangle(0, 0, viewport.Width, viewport.Height), Color.White);

		string loseText = "Byl jsi vyhozen ze školy!";
		Vector2 textSize = LargerFont.MeasureString(loseText);
		Vector2 textPosition = new Vector2((viewport.Width - textSize.X) / 2, viewport.Height / 9);
		_spriteBatch.DrawString(LargerFont, loseText, textPosition, Color.Gold);


		// Position and draw the back button centered in the lower third.
		backButton.Position = new Vector2((viewport.Width - backButton.GetRect().Width) / 2, viewport.Height * 4 / 5);
		backButton.Draw(_spriteBatch);

		_spriteBatch.End();
	}

	public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
	{
		// Update button state based on mouse input.
		MouseState mouseState = Mouse.GetState();
		backButton.Update(mouseState);
	}
}