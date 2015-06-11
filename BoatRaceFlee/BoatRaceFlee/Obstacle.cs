using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace BoatRaceFlee
{
    class Obstacle
    {

        Vector2 position;
        Vector2 velocity;
        Animation obstacleAnimation;
        Rectangle hitBox;
        bool active
        public void LoadContent(ContentManager theContentManager, string textureName)
        {
            obstacleAnimation.LoadContent(theContentManager, textureName);
            hitBox = new Rectangle((int)position.X - obstacleAnimation.frameWidth / 2, (int)position.Y - obstacleAnimation.frameHeight / 2, obstacleAnimation.frameWidth, obstacleAnimation.frameHeight);

        }

        public void Initialize(Vector2 startposition, Vector2 startvel)
        {
            velocity = startvel;
            obstacleAnimation.Initialize(1, 1, startposition, 0f, Color.White);

        }
        public void Update(GameTime gameTime)
        {
            position += velocity;
            obstacleAnimation.position = position;
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {

                obstacleAnimation.Draw(spriteBatch);

            }
        }


    }
}
