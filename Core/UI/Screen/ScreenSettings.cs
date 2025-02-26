using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBoGV;

class ScreenSettings : Screen
{
    private SpriteFont LargerFont = FontManager.GetFont("Arial16");
    private Button escapeButton;
    private FieldInfo[] settings;
    private List<IUIElement> settingElements;

    public override void BeginRun(GraphicsDeviceManager graphics)
    {
        escapeButton = new Button("Zpět", LargerFont, () =>
        {
            TBoGVGame.screenCurrent = new ScreenStart();
            TBoGVGame.screenCurrent.BeginRun(graphics);
        });

        Type type = typeof(Settings);
        // MemberInfo[] a = ;
        settings = type.GetFields(BindingFlags.Public | BindingFlags.Static).ToArray();
        int hp = graphics.GraphicsDevice.Viewport.Height / (settings.Length + 4); // Pad 2 top and 2 bottom

        foreach (var (setting, index) in settings.Select((setting, index) => (setting, index)))
        {
            if (setting.GetType() == typeof(bool))
            {
                settingElements.Add(new Checkbox(setting.Name,
                    Vector2.Zero,
                    (bool)setting.GetValue(null),
                    x => setting.SetValue(null, x)));
            }
        }
    }

    public override void Draw(SpriteBatch _spriteBatch, GraphicsDeviceManager graphics)
    {
        Viewport viewport = graphics.GraphicsDevice.Viewport;

        // bg
        _spriteBatch.Begin(blendState: BlendState.Opaque);
        _spriteBatch.Draw(TextureManager.GetTexture("gymvod"),
            new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
        _spriteBatch.End();

        int menuX = viewport.Width / 2;
        int menuY = viewport.Height / 2;

        int settingsCount = settings.Length;

        int hp = viewport.Height / (settingsCount + 4); // Pad 2 top and 2 bottom

        _spriteBatch.Begin();

        // esc
        escapeButton.Position = new Vector2(viewport.Width / 10, viewport.Height / 10);
        escapeButton.Draw(_spriteBatch);

        // settings

        foreach (var (setting, index) in settings.Select((setting, index) => (setting, index)))
        {
            new Vector2(menuX, (index * hp) + 100);
        }

        _spriteBatch.End();
    }

    public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
    {
        escapeButton.Update(Mouse.GetState());
    }
}