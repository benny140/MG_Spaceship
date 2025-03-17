using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spaceship.Classes
{
    public class AsteroidManager
    {
        public List<Asteroid> _asteroids;
        private readonly Texture2D _textureAsteroid;
        private readonly int _screenWidth;
        private readonly int _screenHeight;
        private readonly Random random = new();

        // Time tracking
        private float _totalElapsedTime = 0f;
        private float _nextSpawnTime = 0f;

        // Spawn parameters
        private readonly float _initialSpawnDelay = 2f; // Initial delay between spawns (in seconds)
        private readonly float _minSpawnDelay = 0.1f; // Minimum delay between spawns
        private readonly float _spawnDelayDecreaseRate = 0.1f; // How much the spawn delay decreases over time

        // Speed parameters
        private readonly int _initialMinSpeed = 100; // Initial minimum speed
        private readonly int _initialMaxSpeed = 200; // Initial maximum speed
        private readonly int _maxSpeedIncreaseRate = 10; // How much the maximum speed increases over time

        public AsteroidManager(Texture2D textureAsteroid, int screenWidth, int screenHeight)
        {
            _textureAsteroid = textureAsteroid;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _asteroids = [];
        }

        public void Update(GameTime gameTime, float _elapsedTime)
        {
            // Update the total elapsed time from the main game
            _totalElapsedTime = _elapsedTime;

            // Adjust spawn delay and speed over time
            float currentSpawnDelay = MathHelper.Max(
                _minSpawnDelay,
                _initialSpawnDelay - _totalElapsedTime * _spawnDelayDecreaseRate
            );
            int currentMinSpeed =
                _initialMinSpeed + (int)(_totalElapsedTime * _maxSpeedIncreaseRate);
            int currentMaxSpeed =
                _initialMaxSpeed + (int)(_totalElapsedTime * _maxSpeedIncreaseRate);

            // Spawn new asteroids
            if (_totalElapsedTime >= _nextSpawnTime && _asteroids.Count < 10) // Limit the number of asteroids
            {
                Vector2 startPosition = new(
                    _screenWidth,
                    Random.Shared.Next(0, _screenHeight - _textureAsteroid.Height)
                );
                float speed = Random.Shared.Next(currentMinSpeed, currentMaxSpeed);
                _asteroids.Add(new Asteroid(_textureAsteroid, startPosition, speed, _screenWidth));

                // Set the next spawn time
                _nextSpawnTime = _totalElapsedTime + currentSpawnDelay;
            }

            // Update all asteroids
            if (_totalElapsedTime > 0)
            {
                foreach (var asteroid in _asteroids)
                {
                    asteroid.Update(gameTime);
                }
            }

            // Remove inactive asteroids
            _asteroids.RemoveAll(asteroid => !asteroid.IsActive);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            // Sort asteroids by speed (slowest first)
            var sortedAsteroids = _asteroids.OrderBy(asteroid => asteroid.Speed).ToList();

            // Draw sorted asteroids
            foreach (var asteroid in sortedAsteroids)
            {
                asteroid.Draw(spriteBatch);
            }
        }

        public void Reset()
        {
            _asteroids.Clear();
            _nextSpawnTime = 0f;
        }
    }
}
