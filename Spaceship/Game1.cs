using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Spaceship.Classes;

namespace Spaceship;

public class Game1 : Game
{
    // Graphics
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;
    private readonly int screenHeight = 720;
    private readonly int screenWidth = 1280;

    // Assets
    private Texture2D textureAsteroid;
    private Texture2D textureShip;
    private Texture2D textureSpace;
    private SpriteFont fontSpace;
    private SpriteFont fontTimer;

    // Ship
    private Ship _ship;

    // Asteroids
    private AsteroidManager _asteroidManager;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = screenWidth; // Width of the screen
        _graphics.PreferredBackBufferHeight = screenHeight; // Height of the screen
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        textureAsteroid = Content.Load<Texture2D>("asteroid");
        textureShip = Content.Load<Texture2D>("ship");
        textureSpace = Content.Load<Texture2D>("space");
        fontSpace = Content.Load<SpriteFont>("spaceFont");
        fontTimer = Content.Load<SpriteFont>("timerFont");

        // Initialize the spaceship
        _ship = new Ship(
            textureShip,
            new Vector2(screenWidth / 2, screenHeight / 2),
            5f,
            screenWidth,
            screenHeight
        );

        // Initialize the asteroid manager
        _asteroidManager = new AsteroidManager(
            textureAsteroid,
            _graphics.PreferredBackBufferWidth,
            _graphics.PreferredBackBufferHeight
        );
    }

    protected override void Update(GameTime gameTime)
    {
        if (
            GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed
            || Keyboard.GetState().IsKeyDown(Keys.Escape)
        )
            Exit();

        // Update assets
        _ship.Update(gameTime);
        _asteroidManager.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();
        _spriteBatch.Draw(textureSpace, new Vector2(0, 0), Color.White);
        _ship.Draw(_spriteBatch);
        _asteroidManager.Draw(_spriteBatch);
        _spriteBatch.End();

        base.Draw(gameTime);
    }
}
