
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace BoatRaceFlee
{
    
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState mouseState;
        Random randomizer;
        
        List<Vector2> SpawnList = new List<Vector2>();

        List<Player> players;
        
        int numOfPlayers = 4;
        enum GameScreen
        {
            MainMenu,
            SelectScreen,
            GameRunning,
            GameOver

        }
        GameScreen gameState = GameScreen.MainMenu;
        public Game1()
        {

            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.PreferredBackBufferHeight = 1080;

            graphics.PreferredBackBufferWidth = 1920;

            
        }
        List<Texture2D> playerIcons;
        Texture2D beachRing;
        Texture2D longBoat;
        Texture2D airCarrier;
        Texture2D battleShip;
        public void InitializeSelectScreen()
        {

            beachRing = Content.Load<Texture2D>("BeachRing");
            longBoat = Content.Load<Texture2D>("Longboat");
            airCarrier = Content.Load<Texture2D>("Aircraft");
            battleShip = Content.Load<Texture2D>("BattleShip");
            playerIcons = new List<Texture2D>();
            

            playerIcons.Add(beachRing);
            playerIcons.Add(longBoat);
            playerIcons.Add(airCarrier);
            playerIcons.Add(playerIcons);


        }
        public void UpdateSelectScreen(GameTime gameTime)
        {


            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].ready)
                {
                    PlayerSelect(gameTime, i);
                }
                if (GamePad.GetState((PlayerIndex)i).Buttons.A == ButtonState.Pressed && !players[i].ready)
                {
                    players[i].ready = true;
                    readyPlayers += 1;
                }
                if (GamePad.GetState((PlayerIndex)i).Buttons.B == ButtonState.Pressed && players[i].ready)
                {
                    players[i].ready = false;
                    readyPlayers -= 1;
                }
            }
            if (readyPlayers == 4)
            {
                currentGameScreen = GameScreen.GameRunning;
            }



        }
        public void DrawIcons(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (!players[i].ready)
                {
                    spriteBatch.Draw(playerIcons[players[i].playerSelected], new Vector2((1920 - 150) / 2, 300 + 100 * i), Color.White);
                }
                else
                {
                    spriteBatch.Draw(playerIcons[players[i].playerSelected], new Vector2(100 + (1920 - 150) / 2, 300 + 100 * i), Color.White);
                }
            }
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
        int VirtualScreenWidth = 1920;
        int VirtualScreenHeight = 1080;
        Vector3 screenScale;

        int currentMenuItem = 1;
        public void InitializeGame()
        {
            float scaleX = (float)GraphicsDevice.Viewport.Width / (float)VirtualScreenWidth;
            float scaleY = (float)GraphicsDevice.Viewport.Height / (float)VirtualScreenHeight;

            screenScale = new Vector3(scaleX, scaleY, 1.0f);
            mouseState = new MouseState();
            players = new List<Player>();
            InitializePlayers(numOfPlayers);
        }
        List<Button> menuButtons;
        public void InitializeMainMenu()
        {
            menuButtons = new List<Button>();
            
            Button button = new Button();
            button.Initialize(new Vector2((1920) / 2, 500 + 140 / 2), "PlayButton", "play", 1);
            menuButtons.Add(button);
            
            button = new Button();
            button.Initialize(new Vector2((1920) / 2, 700 + 140 / 2), "ExitButton", "exit", 2);
            menuButtons.Add(button);
        }
        int menuTime = 1000;
        public void MenuSelect(GameTime gameTime, PlayerIndex num)
        {
            if (1 <= currentMenuItem && currentMenuItem <= menuButtons.Count)
            {

                
                   
                        currentMenuItem -= (int)(GamePad.GetState(num).ThumbSticks.Left.Y*10);
                        elapsedTime = 0;
                    
                

            }


            if (1 > currentMenuItem)
            {
                currentMenuItem = 1;
            }
            if (currentMenuItem > menuButtons.Count)
            {
                currentMenuItem = menuButtons.Count;
            }
        }
        public void UpdateMenu(GameTime gameTime)
        {
            MenuSelect(gameTime, PlayerIndex.One);
            foreach (Button button in menuButtons)
            {
                button.Update(gameTime, mouseState, currentMenuItem);
                if (button.CheckForClick(mouseState))
                {

                    if (button.buttonName == "play")
                    {
                        gameState = GameScreen.GameRunning;
                    }
                    if (button.buttonName == "exit")
                    {
                        Exit();
                    }
                }
            }

        }
        public void DrawMenu(GameTime gameTime)
        {
           // spriteBatch.Draw(MenuBackground, new Vector2(0, 0), Color.White);
            foreach (Button button in menuButtons)

            {
                button.Draw(spriteBatch);
            }

        }

        Animation obstacleAnimation =  new Animation();
        protected override void Initialize()
        {
            randomizer = new Random();
            InitializeMainMenu();

            InitializeGame();
                    
            base.Initialize();
        }

       
        protected override void LoadContent()
        {

            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            
            foreach (Player player in players)
            {
                player.LoadContent(Content, "Arrow");
            }
            foreach (Button button in menuButtons)
            {
                button.LoadContent(Content, button.fileName);
            }


        }

        List<Obstacle> obstacleList = new List<Obstacle>();

        public void AddRock()
        {
            for (int k = 0; k < randomizer.Next(1, 5); k++)
            {
                Obstacle obstacle = new Obstacle();
                obstacleAnimation = new Animation();
                
                obstacleAnimation.LoadContent(Content, "Rock");
                Vector2 pos = new Vector2(1920 + obstacleAnimation.frameWidth, randomizer.Next(obstacleAnimation.frameHeight, 1080- obstacleAnimation.frameHeight));
                obstacle.angle = (float)randomizer.Next(0, 180);
                obstacle.Initialize(pos, new Vector2(300f, 0), obstacleAnimation);

                obstacleList.Add(obstacle);
            }
        }
        public void RockCollision()
        {
            
                for (int i = 0; i < players.Count; i++)
                {
                    for (int j = 0; j < obstacleList.Count; j++)
                    {
                        if (players[i].hitBox.Intersects(obstacleList[j].hitBox))
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
            
        }

      
        protected override void UnloadContent()
        {
        }
        
        float elapsedTime;
        float rockTime=2000;
        public void UpdateGame(GameTime gameTime)
        {
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
        }
        protected override void Update(GameTime gameTime)
        {


            mouseState = Mouse.GetState();
            //if (gameState == GameScreen.SelectScreen)
            //{
            //    UpdateSelectScreen(gameTime);
            //}
            if (gameState == GameScreen.GameRunning)
            {
                UpdateGame(gameTime);
            }
            if (gameState == GameScreen.MainMenu)
            {
                UpdateMenu(gameTime);
            }
            //if (gameState == GameScreen.GameOver)
            //{
            //    UpdateGameOverScreen();
            //}


            
            base.Update(gameTime);
        }

        
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
            if (gameState == GameScreen.MainMenu)
            {
                DrawMenu(gameTime);
            }
                if (gameState == GameScreen.GameRunning)
            {
                foreach (Obstacle rock in obstacleList)
                {
                    rock.Draw(spriteBatch);

                }
                foreach (Player player in players)
                {


                    spriteBatch.Draw(rectangleTexture, player.hitBox, Color.Green);


                    player.Draw(spriteBatch);
                }
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
