
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BoatRaceFlee
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Random randomizer;
        
        List<Vector2> SpawnList = new List<Vector2>();

        List<Player> players;
        
        int numOfPlayers = 4;

        public Game1()
        {
            
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 1080;

            graphics.PreferredBackBufferWidth = 1920;

            
        }

        public void InitializePlayers(int numOfPlayers)
        {
            SpawnList.Add(new Vector2(2 + 150, 350));
            SpawnList.Add(new Vector2(2 + 150, 850));
            SpawnList.Add(new Vector2(1610 + 150, 850));
            SpawnList.Add(new Vector2(1612 + 150, 400));
            for (int i = 0; i < numOfPlayers; i++)
            {
                Player player;
                PlayerIndex playerIndex;
                playerIndex = (PlayerIndex)i;
                player = new Player();
                player.Initialize(SpawnList[i], playerIndex, 500);
                players.Add(player);

            }

        }
        Animation obstacleAnimation =  new Animation();
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            randomizer = new Random();
            players = new List<Player>();
            InitializePlayers(numOfPlayers);
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {


            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            foreach (Player player in players)
            {
                player.LoadContent(Content, "Arrow");
            }

            // TODO: use this.Content to load your game content here
        }

        List<Obstacle> obstacleList = new List<Obstacle>();

        public void AddRock()
        {
            Obstacle obstacle = new Obstacle();
            obstacleAnimation = new Animation();
            Vector2 pos = new Vector2(1920, randomizer.Next(0, 1080));
            obstacleAnimation.LoadContent(Content, "Rock");
           
                obstacle.Initialize(pos, new Vector2(300f, 0), obstacleAnimation);
            
            obstacleList.Add(obstacle);
        }
        public void RockCollision()
        {
            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < obstacleList.Count; j++)
                {
                    if (Collision.CollidesWith(players[i].playerAnimation,
                    obstacleList[j].obstacleAnimation, players[i].playerTransformation,
                    obstacleList[j].obstacleTransformation))

                    {


                        players[i].active = false;
                    }
                }
            }
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        float elapsedTime;
        float rockTime=1000;
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (elapsedTime > rockTime)
            {
                AddRock();
                elapsedTime = 0;
            }
            foreach (Obstacle rock in obstacleList)
            {
                rock.Update(gameTime);
            }
            RockCollision();




           foreach (Player player in players)
                player.Update(gameTime);
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            Texture2D rectangleTexture = new Texture2D(GraphicsDevice, 1, 1, false, SurfaceFormat.Color);

            Color[] color = new Color[1 * 1];
            for (int i = 0; i < color.Length; i++)
            {
                color[i] = Color.White;
            }
            rectangleTexture.SetData(color);

            spriteBatch.Begin();
            foreach (Obstacle rock in obstacleList)
            {
                rock.Draw(spriteBatch);

            }
            // TODO: Add your drawing code here
            foreach (Player player in players)
            {
                

                spriteBatch.Draw(rectangleTexture, player.hitBox, Color.Green);


                player.Draw(spriteBatch);
            }

            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
