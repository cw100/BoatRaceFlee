using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace BoatRaceFlee
{
    class Player
    {
        public float mass;
        float playerSpeed;
        public Vector2 position;
        public Vector2 velocity;

       
        int health;
       public Animation playerAnimation;
        bool active;
        public Rectangle hitBox;

        public PlayerIndex playerNumber;

        public void LoadContent(ContentManager theContentManager, string textureName)
        {
            playerAnimation.LoadContent(theContentManager, textureName );
            hitBox = new Rectangle((int)position.X- playerAnimation.frameWidth/2, (int)position.Y - playerAnimation.frameHeight/2, playerAnimation.frameWidth, playerAnimation.frameHeight);

        }

        public void Initialize(Vector2 startposition,PlayerIndex playernumber, float playerspeed)
        {
            mass = 100;

            position = startposition;
            playerSpeed = playerspeed;
            playerNumber = playernumber;
            active = true;
            playerAnimation = new Animation();
            playerAnimation.Initialize(1, 1, startposition, 0f, Color.White);
            }
        public void ScreenCollision()
        {
            position.X = MathHelper.Clamp(position.X, 0 + (playerAnimation.frameWidth / 2), 1920 - (playerAnimation.frameWidth / 2));

            position.Y = MathHelper.Clamp(position.Y, 0 + (playerAnimation.frameHeight / 2), 1080 - (playerAnimation.frameHeight / 2));

        }
        public void ControllerMove(GameTime gameTime)
        {


            velocity += new Vector2(GamePad.GetState(playerNumber).ThumbSticks.Left.X * playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, -GamePad.GetState(playerNumber).ThumbSticks.Left.Y * playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            velocity.X = MathHelper.Clamp(velocity.X, -playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);
            velocity.Y = MathHelper.Clamp(velocity.Y, -playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds, playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds);


        }
        public void ApplyFriction(GameTime gameTime)
        {
            velocity *= 0.5f;
        }
        public void Update(GameTime gameTime)
        {
            ControllerMove(gameTime);
            position += velocity;
            ScreenCollision();
            ApplyFriction(gameTime);
            hitBox.Y = (int)position.Y - playerAnimation.frameHeight / 2;
            hitBox.X = (int)position.X - playerAnimation.frameWidth / 2;
            playerAnimation.position = position;
            playerAnimation.Update(gameTime);
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {

                playerAnimation.Draw(spriteBatch);

            }
        }
    }
}
