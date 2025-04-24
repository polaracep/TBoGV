using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;
class InGameMenuPauseNoOverlay : InGameMenu
{
	private static Viewport Viewport;

	public InGameMenuPauseNoOverlay(Viewport viewport)
	{
		Viewport = viewport;
	}


	public override void Update(Viewport viewport, Player player, MouseState mouseState, KeyboardState keyboardState, double dt)
	{
		base.Update(viewport, player, mouseState, keyboardState, dt);
	}

	public override void Draw(SpriteBatch spriteBatch)
	{
	}

}
