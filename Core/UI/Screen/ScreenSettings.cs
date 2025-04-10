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
    private UIElement skibidiElement;
    public Screen LastScreen;
    private KeyboardState previousKeyboardState;
    private string text = "";
    private Keys[] lastPressedKeys;
    private bool WOWskibidi = false;

    public ScreenSettings(GraphicsDeviceManager graphics) : base(graphics)
    {
        Viewport v = graphics.GraphicsDevice.Viewport;
        escapeButton = new Button("Zpět", LargerFont, () =>
        {
            text = "";
            Settings.Save();
            TBoGVGame.screenCurrent = LastScreen;
        });
        var list = Settings.SettingsList;

        // MemberInfo[] a = ;
        int hp = v.Height / (list.Count + 4); // Pad 2 top and 2 bottom

        foreach (var (setting, index) in list.Select((setting, index) => (setting, index)))
        {
            if (setting == null)
                continue;
            if (setting.Value == null)
                continue;

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
                    Convert.ToSingle(setting.Value), v.Width / 10, 10,
                    setting.Name,
                    LargerFont,
                    x => setting.Value = x));
            }
        }

        skibidiElement = new Checkbox(Settings.Skibidi.Name,
            Vector2.Zero,
            (bool)Settings.Skibidi.Value,
            x =>
            {
                Settings.Skibidi.Value = x;
                if (x)
                    GameManager.Player.ActivateEasyMode();
                else
                    GameManager.Player.DeactivateEasyMode();

            });

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

        int lastIndex = 0;
        foreach (var (el, index) in settingElements.Select((el, index) => (el, index)))
        {
            el.Position = new Vector2(menuX - el.GetRect().Width / 2, (index * hp) + 100);
            ((IDraw)el).Draw(_spriteBatch);
            lastIndex = index;
        }

        if (WOWskibidi)
        {
            skibidiElement.Position = new Vector2(menuX - skibidiElement.GetRect().Width / 2, ((lastIndex + 1) * hp) + 100);
            ((IDraw)skibidiElement).Draw(_spriteBatch);
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

        var pressedNow = keyboardState.GetPressedKeys();
        foreach (var key in pressedNow)
        {
            if (!lastPressedKeys.Contains(key))
            {
                if (key == Keys.Back && text.Length != 0)
                    text = text.Remove(text.Length - 1);
                else
                    text += key;
            }
        }

        MouseState mouseState = Mouse.GetState();

        if (text == "SKIBIDI")
        {
            skibidiElement.Update(mouseState);
            WOWskibidi = true;
        }
        else
            WOWskibidi = false;

        escapeButton.Update(mouseState);
        foreach (var s in settingElements)
            s.Update(mouseState);

        previousKeyboardState = keyboardState;
        lastPressedKeys = pressedNow;
    }
}