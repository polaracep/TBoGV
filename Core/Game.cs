﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TBoGV;

public class TBoGVGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    Screen screenGame, screenCurrent;
    public TBoGVGame()
    {
        _graphics = new GraphicsDeviceManager(this);

        screenCurrent = screenGame = new ScreenGame(_graphics);

        Content.RootDirectory = "Content/Textures";
        IsMouseVisible = true;

    }

    protected override void Initialize()
    {
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);
        TextureManager.Load(Content);
    }

    // Run after LoadContent
    protected override void BeginRun()
    {

        screenCurrent.BeginRun();
        base.BeginRun();
    }

    protected override void Update(GameTime gameTime)
    {
        // exit coded
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            Exit();
        screenCurrent.Update();
        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);
        screenCurrent.Draw(_spriteBatch);
        base.Draw(gameTime);
    }
}
