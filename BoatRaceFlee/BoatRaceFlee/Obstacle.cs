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
        public Animation obstacleAnimation;
        Texture2D obstacleTexture;
        public Rectangle hitBox;
        bool active=true;
        public float angle;

        public void Initialize(Vector2 startposition, Vector2 startvel, Animation obstacleanimation)
        {
            position = startposition;
            velocity = startvel;
            obstacleAnimation = obstacleanimation;
            obstacleAnimation.Initialize(1, 1, position, angle, Color.White);
            hitBox = new Rectangle((int)position.X - obstacleAnimation.frameWidth / 2, (int)position.Y - obstacleAnimation.frameHeight / 2, obstacleAnimation.frameWidth, obstacleAnimation.frameHeight);

        }
        public Matrix obstacleTransformation;
        public void Update(GameTime gameTime)
        {
            position -= velocity* (float)gameTime.ElapsedGameTime.TotalSeconds;
           
            obstacleAnimation.position = position;

            obstacleAnimation.Update(gameTime);

            hitBox.Y = (int)position.Y - obstacleAnimation.frameHeight / 2;
            hitBox.X = (int)position.X - obstacleAnimation.frameWidth / 2;
            obstacleTransformation =
                    Matrix.CreateTranslation(new Vector3(-obstacleAnimation.origin, 0.0f)) *
                    Matrix.CreateScale(obstacleAnimation.scale) *
                    Matrix.CreateTranslation(new Vector3(obstacleAnimation.position, 0.0f));

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
