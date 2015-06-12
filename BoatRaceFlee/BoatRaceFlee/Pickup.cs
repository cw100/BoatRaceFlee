using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace BoatRaceFlee
{
    class Pickup
    {

        public float velocityBuff;
        public Vector2 position;
        public Rectangle hitBox;
        public Vector2 velocity;
        public bool active;
        public float buffTime;
        public Animation pickupAnimation;


        public void LoadContent(ContentManager theContentManager, string textureName)
        {
            pickupAnimation = new Animation();
            pickupAnimation.LoadContent(theContentManager, textureName);
            hitBox = new Rectangle((int)position.X - pickupAnimation.frameWidth / 2, (int)position.Y - pickupAnimation.frameHeight / 2, pickupAnimation.frameWidth, pickupAnimation.frameHeight);

        }
        public void Initialize(Vector2 intvel, Vector2 intpos, float bufftime, float velocitybuff)

        {
            velocity = intvel;
            position = intpos;
            buffTime = bufftime;
            velocityBuff = velocitybuff;

            active = true;

            

            pickupAnimation.Initialize(1, 1, intpos, 0f, Color.White);
        }

        public void Update(GameTime gameTime)
        {
            if (active)
            {
            
            position -= velocity*(float)gameTime.ElapsedGameTime.TotalSeconds;
                hitBox.X = (int)position.X;
                hitBox.Y = (int)position.Y;
                pickupAnimation.position = position;
                pickupAnimation.Update(gameTime);
        }
    }
        public void Draw(SpriteBatch spriteBatch)
        {
            if(active)
            {
                pickupAnimation.Draw(spriteBatch);
            }
        }




    }
}
