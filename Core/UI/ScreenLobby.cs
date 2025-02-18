using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TBoGV;

class ScreenLobby : Screen
{
    private Player player;
    private Camera _camera;
    private Lobby lobby;
    private UI ui;
    public override void BeginRun(GraphicsDeviceManager graphics)
    {
        this.lobby = new Lobby();
    }

    public override void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
    {
        _spriteBatch.Begin(transformMatrix: _camera.Transform);
        lobby.Draw(_spriteBatch);
        player.Draw(_spriteBatch);
        _spriteBatch.End();

        _spriteBatch.Begin();
        ui.Draw(_spriteBatch);
        player.Inventory.Draw(_spriteBatch);
        _spriteBatch.End();
    }

    public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
    {
        throw new System.NotImplementedException();
    }
}