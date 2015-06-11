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
        Rectangle hitBox;
        bool active=true;
        
        public void Initialize(Vector2 startposition, Vector2 startvel, Animation obstacleanimation)
        {
            position = startposition;
            velocity = startvel;
            obstacleAnimation = obstacleanimation;
            obstacleAnimation.Initialize(1, 1, position, 0, Color.White);
        }
        public Matrix obstacleTransformation;
        public void Update(GameTime gameTime)
        {
            position -= velocity* (float)gameTime.ElapsedGameTime.TotalSeconds;
           
            obstacleAnimation.position = position;

            obstacleAnimation.Update(gameTime);

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
