﻿using System;
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
      public+  bool ready = false;
        public float mass;
        float playerSpeed;
        public Vector2 position;
        public Vector2 velocity;
        public float angle;
        public int playerSelected;
        int health;
       public Animation playerAnimation;
      public  bool active;
        public Rectangle hitBox;
        public Matrix playerTransformation;
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
        private float AngleLerp(float nowrap, float wraps, float lerp)
        {

            float c, d;

            if (wraps < nowrap)
            {
                c = wraps + MathHelper.TwoPi;
                //c > nowrap > wraps
                d = c - nowrap > nowrap - wraps
                    ? MathHelper.Lerp(nowrap, wraps, lerp)
                    : MathHelper.Lerp(nowrap, c, lerp);

            }
            else if (wraps > nowrap)
            {
                c = wraps - MathHelper.TwoPi;
                //wraps > nowrap > c
                d = wraps - nowrap > nowrap - c
                    ? MathHelper.Lerp(nowrap, c, lerp)
                    : MathHelper.Lerp(nowrap, wraps, lerp);

            }
            else { return nowrap; } //Same angle already

            return MathHelper.WrapAngle(d);
        }
        float preAngle;
        float targetAngle;
        public void Rotate()

        {
            preAngle = angle;
            if (GamePad.GetState(playerNumber).ThumbSticks.Left.Y != 0 || GamePad.GetState(playerNumber).ThumbSticks.Left.X != 0)
            {
                
                angle = AngleLerp(preAngle, -(float)Math.Atan2(GamePad.GetState(playerNumber).ThumbSticks.Left.Y, GamePad.GetState(playerNumber).ThumbSticks.Left.X),0.05f);
                   
                
                
            }
        }

        public void ApplyFriction(GameTime gameTime)
        {
            velocity *= 0.2f* (float)gameTime.ElapsedGameTime.TotalSeconds;
        }
        public void Update(GameTime gameTime)
        {
            Rotate();
            ControllerMove(gameTime);
            Vector2 direction = new Vector2((float)Math.Cos(angle),
                                   (float)Math.Sin(angle));
            direction.Normalize();
            position += direction * 200 * (float)gameTime.ElapsedGameTime.TotalSeconds - new Vector2(100 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0);

            
            ScreenCollision();
            ApplyFriction(gameTime);
            hitBox.Y = (int)position.Y - playerAnimation.frameHeight / 2;
            hitBox.X = (int)position.X - playerAnimation.frameWidth / 2;
            playerAnimation.position = position;
            playerAnimation.angle = angle;
            playerAnimation.Update(gameTime);
            playerTransformation =
                    Matrix.CreateTranslation(new Vector3(-playerAnimation.origin, 0.0f)) *
                    Matrix.CreateScale(playerAnimation.scale) *
                    Matrix.CreateTranslation(new Vector3(playerAnimation.position, 0.0f));
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