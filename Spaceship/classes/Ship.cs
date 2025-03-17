using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Spaceship.Classes // Replace with your game's namespace
{
    public class Ship
    {
        // Properties
        public Texture2D Texture { get; set; } // The Ship texture
        public Vector2 Position { get; private set; } // The current position of the Ship
        public float Speed { get; set; } // Movement speed of the Ship
        public int ScreenWidth { get; set; } // Width of the screen
        public int ScreenHeight { get; set; } // Height of the screen

        // Method to get the bounding rectangle.
        public Rectangle Bounds =>
            new((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);

        // Constructor
        public Ship(
            Texture2D texture,
            Vector2 startPosition,
            float speed,
            int screenWidth,
            int screenHeight
        )
        {
            Texture = texture;
            Position = startPosition;
            Speed = speed;
            ScreenWidth = screenWidth;
            ScreenHeight = screenHeight;
        }

        // Update method to handle movement
        public void Update(GameTime gameTime, bool _gameStarted)
        {
            if (_gameStarted)
            {
                // Get the current keyboard state
                KeyboardState keyboardState = Keyboard.GetState();

                // Get the current game pad state (for player 1)
                GamePadState gamePadState = GamePad.GetState(PlayerIndex.One);

                // Calculate movement based on arrow keys or left thumbstick
                Vector2 movement = Vector2.Zero;

                // Get the elapsed time since the last frame
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

                // Keyboard input
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    movement.X -= Speed * deltaTime;
                }
                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    movement.X += Speed * deltaTime;
                }
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    movement.Y -= Speed * deltaTime;
                }
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    movement.Y += Speed * deltaTime;
                }

                // Gamepad input (left thumbstick)
                movement.X += gamePadState.ThumbSticks.Left.X * Speed;
                movement.Y -= gamePadState.ThumbSticks.Left.Y * Speed; // Y axis is inverted

                // Apply movement to the ship's position
                Position += movement;

                // Keep the Ship within the screen bounds
                Position = Vector2.Clamp(
                    Position,
                    Vector2.Zero,
                    new Vector2(ScreenWidth - Texture.Width, ScreenHeight - Texture.Height)
                );
            }
        }

        // Draw method to render the Ship
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
