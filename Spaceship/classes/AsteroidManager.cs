using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Spaceship.Classes
{
    public class AsteroidManager
    {
        private List<Asteroid> _asteroids;
        private Texture2D _textureAsteroid;
        private int _screenWidth;
        private int _screenHeight;
        Random random = new Random();

        // Time tracking
        private float _totalElapsedTime = 0f;
        private float _nextSpawnTime = 0f;

        // Spawn parameters
        private float _initialSpawnDelay = 2f; // Initial delay between spawns (in seconds)
        private float _minSpawnDelay = 0.5f; // Minimum delay between spawns
        private float _spawnDelayDecreaseRate = 0.1f; // How much the spawn delay decreases over time

        // Speed parameters
        private int _initialMinSpeed = 100; // Initial minimum speed
        private int _initialMaxSpeed = 200; // Initial maximum speed
        private int _maxSpeedIncreaseRate = 10; // How much the maximum speed increases over time

        public AsteroidManager(Texture2D textureAsteroid, int screenWidth, int screenHeight)
        {
            _textureAsteroid = textureAsteroid;
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
            _asteroids = new List<Asteroid>();
        }

        public void Update(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            _totalElapsedTime += deltaTime;

            // Adjust spawn delay and speed over time
            float currentSpawnDelay = MathHelper.Max(
                _minSpawnDelay,
                _initialSpawnDelay - _totalElapsedTime * _spawnDelayDecreaseRate
            );
            int currentMaxSpeed =
                _initialMaxSpeed + (int)(_totalElapsedTime * _maxSpeedIncreaseRate);

            // Spawn new asteroids
            if (_totalElapsedTime >= _nextSpawnTime && _asteroids.Count < 10) // Limit the number of asteroids
            {
                Vector2 startPosition = new Vector2(
                    _screenWidth,
                    Random.Shared.Next(0, _screenHeight - _textureAsteroid.Height)
                );
                float speed = Random.Shared.Next(_initialMinSpeed, currentMaxSpeed);
                _asteroids.Add(new Asteroid(_textureAsteroid, startPosition, speed, _screenWidth));

                // Set the next spawn time
                _nextSpawnTime = _totalElapsedTime + currentSpawnDelay;
            }

            // Update all asteroids
            foreach (var asteroid in _asteroids)
            {
                asteroid.Update(gameTime);
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
    }
}
