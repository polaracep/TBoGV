﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace TBoGV;

public class TBoGVGame : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
	private Camera _camera;
	RoomEmpty r = new RoomEmpty();
    Player player;
    List<Projectile> projectiles;
    Enemy enemy;
    MouseState mouseState;
    KeyboardState keyboardState;
    public TBoGVGame()
    {
        _graphics = new GraphicsDeviceManager(this);
		

		Content.RootDirectory = "Content/Textures";
        IsMouseVisible = true;
        player = new Player(new Vector2(0, 0));

        enemy = new RangedEnemy(new Vector2(0, 100));
        projectiles = new List<Projectile>();

    }

    protected override void Initialize()
    {
        base.Initialize();
		_camera = new Camera(GraphicsDevice.Viewport);
	}

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        Player.Load(Content);
        RangedEnemy.Load(Content);
        Projectile.Load(Content);
        TileWall.Load(Content);
        TileFloor.Load(Content);

    }

    protected override void Update(GameTime gameTime)
    {
        // exit coded
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();
        mouseState = Mouse.GetState();
        keyboardState = Keyboard.GetState();
        player.Update(keyboardState, mouseState,_camera.Transform);
        foreach (Projectile projectile in player.Projectiles)
            projectile.Update();
        foreach (Projectile projectile in projectiles)
            projectile.Update();
        enemy.Update(player.Position + player.Size / 2);
        if (enemy.ReadyToAttack())
            projectiles.Add(enemy.Attack());
        
		_camera.Update(player.Position + player.Size / 2);
		base.Update(gameTime);
	}

    protected override void Draw(GameTime gameTime)
    {
        //TileWall wallTile = new TileWall();
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin(transformMatrix: _camera.Transform);
        r.Draw(_spriteBatch);
        //_spriteBatch.Draw(wallTile.getTexture(), new Vector2(0, 0), Color.White);
        player.Draw(_spriteBatch);
        enemy.Draw(_spriteBatch);
        foreach (Projectile projectile in player.Projectiles)
            projectile.Draw(_spriteBatch);
        foreach (Projectile projectile in projectiles)
            projectile.Draw(_spriteBatch);
        //enemy.Draw(_spriteBatch);

        _spriteBatch.End();

        // TODO: Add your drawing code here

        base.Draw(gameTime);
    }
}
