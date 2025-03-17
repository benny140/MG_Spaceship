using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spaceship.Classes
{
    public class Asteroid
    {
        public Texture2D Texture { get; private set; } // Texture for the asteroid
        public Vector2 Position { get; private set; } // Current position of the asteroid
        public float Speed { get; private set; } // Speed of the asteroid (pixels per second)
        public int ScreenWidth { get; private set; } // Width of the screen (for boundary checks)
        public bool IsActive { get; private set; } // Flag to indicate if the asteroid is active

        // Method to get the bounding circle of the asteroid.
        public Circle Bounds =>
            new Circle(
                new Vector2(Position.X + Texture.Width / 2, Position.Y + Texture.Height / 2), // Center
                Texture.Width / 2 // Radius
            );

        public Asteroid(Texture2D texture, Vector2 startPosition, float speed, int screenWidth)
        {
            Texture = texture;
            Position = startPosition;
            Speed = speed;
            ScreenWidth = screenWidth;
            IsActive = true; // The asteroid starts as active
        }

        public void Update(GameTime gameTime)
        {
            // Only update if the asteroid is active
            if (!IsActive)
                return;

            // Calculate movement based on speed and elapsed time
            float delta = (float)gameTime.ElapsedGameTime.TotalSeconds;
            Position = new Vector2(Position.X - Speed * delta, Position.Y);

            // Deactivate the asteroid when it goes off-screen
            if (Position.X + Texture.Width < 0)
            {
                IsActive = false; // Mark the asteroid as inactive
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Only draw if the asteroid is active
            if (!IsActive)
                return;

            // Draw the asteroid at its current position
            spriteBatch.Draw(Texture, Position, Color.White);
        }
    }
}
