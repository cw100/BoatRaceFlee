using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace BoatRaceFlee
{
    class Particle
    {
        Texture2D particleTexture;
        Vector2 position;
        Vector2 velocity;
        float scale = 1f;
        float angle;
        float ttl;
        float timeAlive = 0;
        public bool active = true;
        public Color particleColor;
        SpriteEffects flip;

        public void Initialize(Texture2D particletexture, float live, Vector2 intialposition, Color color, Vector2 intialvelocity, float ang, SpriteEffects intflip)
        {
            flip = intflip;
            angle = ang;
            ttl = live;
            particleTexture = particletexture;
            particleColor = color;
            velocity = intialvelocity;
            position = intialposition;
        }
        public void Update(GameTime gameTime, float ang)
        {
            if (active)
            {
                angle = ang;
                particleColor.A = (Byte)(255 - ((255 / ttl) * timeAlive));
                position += velocity * gameTime.ElapsedGameTime.Seconds;
                timeAlive += gameTime.ElapsedGameTime.Seconds;
                scale = 1 - ( timeAlive / ttl);
                if (timeAlive > ttl)
                {
                    active = false;
                }
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (active)
            {
                
                Rectangle source = new Rectangle( 0, 0, particleTexture.Width, particleTexture.Height);
                Vector2 origin = new Vector2(particleTexture.Width / 2.0f, particleTexture.Height / 2.0f);
                spriteBatch.Draw(particleTexture, position, source, particleColor, angle,
                  origin, scale, flip, 0f);
            }
        }
    }
}
