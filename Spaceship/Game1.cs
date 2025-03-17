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
    private readonly int screenHeight = 1080;
    private readonly int screenWidth = 1920;

    // Timer variables
    private float _elapsedTime;
    private SpriteFont fontTimer;

    // Ship
    private Texture2D textureShip;
    private Ship _ship;
    private readonly float shipSpeed = 250f;

    // Asteroids
    private Texture2D textureAsteroid;
    private AsteroidManager _asteroidManager;

    // Other Assets
    private Texture2D textureSpace;
    private SpriteFont fontSpace;

    // Game state
    private bool _gameStarted;
    private bool _reset;

    // Rumble variables
    private float _rumbleTimeRemaining; // Time left for rumble
    private const float RumbleDuration = 0.5f; // Duration of rumble in seconds
    private const float RumbleIntensity = 1.0f; // Intensity of rumble (0.0f to 1.0f)

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
        _gameStarted = false; // Game starts in the "not started" state
        _rumbleTimeRemaining = 0f; // Initialize rumble timer

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
            shipSpeed,
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

        // Check if the Enter key is pressed to start the game
        if (!_gameStarted && Keyboard.GetState().IsKeyDown(Keys.Enter))
        {
            _gameStarted = true; // Start the game
            _elapsedTime = 0; // Reset the timer
            _reset = true;
        }

        // Reset the asteriods when the game has just started
        if (_reset)
        {
            _asteroidManager.Reset();
            _reset = false;
        }

        // Only update the timer if the game has started
        if (_gameStarted)
        {
            _elapsedTime += (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Check for collision between ship and asteroid
            foreach (var asteroid in _asteroidManager._asteroids)
            {
                if (Circle.Intersects(asteroid.Bounds, _ship.Bounds))
                {
                    _gameStarted = false;
                    TriggerRumble(); // Trigger rumble on collision
                }
            }
        }

        // Update assets
        _ship.Update(gameTime, _gameStarted);
        _asteroidManager.Update(gameTime, _elapsedTime * (_gameStarted ? 1 : 0));

        // Update rumble timer
        if (_rumbleTimeRemaining > 0)
        {
            _rumbleTimeRemaining -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (_rumbleTimeRemaining <= 0)
            {
                // Stop rumble when time is up
                GamePad.SetVibration(PlayerIndex.One, 0f, 0f);
            }
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        _spriteBatch.Begin();

        // Draw textures
        _spriteBatch.Draw(textureSpace, new Vector2(0, 0), Color.White);
        _ship.Draw(_spriteBatch);
        _asteroidManager.Draw(_spriteBatch);

        // Draw the timer
        string timerText = $"Time: {_elapsedTime:0}";
        _spriteBatch.DrawString(fontTimer, timerText, new Vector2(10, 10), Color.White);

        if (!_gameStarted)
        {
            // Display "Press Enter to Start" message in the center of the screen
            string startMessage = "Press Enter to Start";
            Vector2 textSize = fontSpace.MeasureString(startMessage);
            Vector2 textPosition = new(
                (GraphicsDevice.Viewport.Width - textSize.X) / 2,
                (GraphicsDevice.Viewport.Height - textSize.Y) / 2
            );
            _spriteBatch.DrawString(fontSpace, startMessage, textPosition, Color.White);
        }

        _spriteBatch.End();

        base.Draw(gameTime);
    }

    // Method to trigger rumble
    private void TriggerRumble()
    {
        _rumbleTimeRemaining = RumbleDuration; // Set rumble duration
        GamePad.SetVibration(PlayerIndex.One, RumbleIntensity, RumbleIntensity); // Start rumble
    }
}
