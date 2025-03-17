using System.Runtime.CompilerServices;
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

                // Calculate movement based on arrow keys
                if (keyboardState.IsKeyDown(Keys.Left))
                {
                    Position = new Vector2(Position.X - Speed, Position.Y);
                }
                if (keyboardState.IsKeyDown(Keys.Right))
                {
                    Position = new Vector2(Position.X + Speed, Position.Y);
                }
                if (keyboardState.IsKeyDown(Keys.Up))
                {
                    Position = new Vector2(Position.X, Position.Y - Speed);
                }
                if (keyboardState.IsKeyDown(Keys.Down))
                {
                    Position = new Vector2(Position.X, Position.Y + Speed);
                }

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
