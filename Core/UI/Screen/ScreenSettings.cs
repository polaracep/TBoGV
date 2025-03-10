using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using TBoGV;

public class ScreenSettings : Screen
{
    private SpriteFont LargerFont = FontManager.GetFont("Arial16");
    private Button escapeButton;
    private List<UIElement> settingElements = new List<UIElement>();
    public Screen LastScreen;
    private KeyboardState previousKeyboardState;
    public override void BeginRun(GraphicsDeviceManager graphics)
    {
        Viewport v = graphics.GraphicsDevice.Viewport;
        escapeButton = new Button("Zpět", LargerFont, () =>
        {
            Settings.Save();
            TBoGVGame.screenCurrent = LastScreen;
        });
        var list = Settings.SettingsList;

        // MemberInfo[] a = ;
        int hp = v.Height / (list.Count + 4); // Pad 2 top and 2 bottom

        foreach (var (setting, index) in list.Select((setting, index) => (setting, index)))
        {
            Type t = setting.Value.GetType();
            if (t == typeof(bool))
            {
                settingElements.Add(new Checkbox(setting.Name,
                    Vector2.Zero,
                    (bool)setting.Value,
                    x => setting.Value = x));
            }
            else if (t == typeof(double))
            {
                settingElements.Add(new Slider(0f, 1f,
                    (float)(double)setting.Value, v.Width / 10, 10,
                    setting.Name,
                    LargerFont,
                    x => setting.Value = x));
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

        int settingsCount = Settings.SettingsList.Count;

        int hp = viewport.Height / (settingsCount + 4); // Pad 2 top and 2 bottom

        _spriteBatch.Begin();

        // esc
        escapeButton.Position = new Vector2(viewport.Width / 10, viewport.Height / 10);
        escapeButton.Draw(_spriteBatch);

        // settings

        foreach (var (el, index) in settingElements.Select((el, index) => (el, index)))
        {
            el.Position = new Vector2(menuX - el.GetRect().Width / 2, (index * hp) + 100);
            ((IDraw)el).Draw(_spriteBatch);
        }

        _spriteBatch.End();
    }

    public override void Update(GameTime gameTime, GraphicsDeviceManager graphics)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Escape) && previousKeyboardState.IsKeyUp(Keys.Escape))
        {
            escapeButton.OnClick();
        }

        MouseState mouseState = Mouse.GetState();
        escapeButton.Update(mouseState);
        foreach (var s in settingElements)
            s.Update(mouseState);

        previousKeyboardState = keyboardState;
    }
}