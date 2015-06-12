
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
        GameScreen gameState = GameScreen.GameRunning;
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
        Texture2D river;
        public void InitializeSelectScreen()
        {

            beachRing = Content.Load<Texture2D>("BeachRing");
            longBoat = Content.Load<Texture2D>("Longboat");
            airCarrier = Content.Load<Texture2D>("Aircraft");
            battleShip = Content.Load<Texture2D>("BattleShip");
            playerIcons = new List<Texture2D>();
            playerTexNames = new List<string>();

            playerIcons.Add(beachRing);
            playerIcons.Add(longBoat);
            playerIcons.Add(airCarrier);
            playerIcons.Add(battleShip);


        }
        int readyPlayers = 0;
        List<string> playerTexNames;
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
                gameState = GameScreen.GameRunning;
            }



        }
        public void PlayerSelect(GameTime gameTime, int num)
        {

            if (0 <= players[num].playerSelected && players[num].playerSelected < playerIcons.Count)
            {

                elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                if (elapsedTime > menuTime)
                {
                    players[num].playerSelected += (int)(GamePad.GetState(players[num].playerNumber).ThumbSticks.Left.X*2);
                    elapsedTime = 0;
                }

            }


            if (0 > players[num].playerSelected)
            {
                players[num].playerSelected = 0;
            }
            if (players[num].playerSelected >= playerIcons.Count)
            {
                players[num].playerSelected = playerIcons.Count-1;
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

                particleWake = Content.Load<Texture2D>("wake");
                player.Initialize(SpawnList[i], playerIndex, 500,particleWake);
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
            obstacleList = new List<Obstacle>();
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
        int menuTime = 100;
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
                        gameState = GameScreen.SelectScreen;
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
        public void UpdateExplosions(GameTime gameTime)
        {
            for (int i = 0; i < explosions.Count; i++)
            {
                explosions[i].Update(gameTime);
                if (!explosions[i].active)
                {
                    
                    explosions.RemoveAt(i);
                }
            }
        }

        Animation obstacleAnimation =  new Animation();
        protected override void Initialize()
        {
            riverPos = new Vector2(0, 0);
            riverPosTwo = new Vector2(1600, 0);

            riverPosThree = new Vector2(3200, 0);
            riverbankPos= new Vector2(0, 0);
            riverbankPosTwo= new Vector2(3720, 0);

            riverbankPosThree = new Vector2(0, 1080 - 85);
            riverbankPosFour = new Vector2(3720, 1080-85);
            randomizer = new Random();
            riverBankOne = new Animation();
            riverBankTwo = new Animation();
            riverBankThree = new Animation();
            riverBankFour = new Animation();

            InitializeSelectScreen();
            InitializeMainMenu();

            InitializeGame();
            explosions = new List<Animation>();
            readyPlayers = 0;
            base.Initialize();
        }

        Texture2D riverTwo;
        Texture2D riverThree;
        Texture2D playerOneWin, playerTwoWin, playerThreeWin, playerFourWin, winningTex;
        protected override void LoadContent()
        {

            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            playerOneWin = Content.Load<Texture2D>("playerOneWin");

            playerTwoWin = Content.Load<Texture2D>("playerTwoWins");

            playerThreeWin = Content.Load<Texture2D>("playerThreeWins");

            playerFourWin = Content.Load<Texture2D>("playerFourWins");

            explosionTex = Content.Load<Texture2D>("explosion");
            river = Content.Load<Texture2D>("river");
            riverTwo = Content.Load<Texture2D>("river");

            riverBankThree.LoadContent(Content, "riverBank");
            riverBankFour.LoadContent(Content, "riverBank");

            riverBankOne.LoadContent(Content, "riverBank2");
            riverBankTwo.LoadContent(Content, "riverBank2");

            riverThree = Content.Load<Texture2D>("river");
            foreach (Player player in players)
            {
                player.LoadContent(Content, "Boats");
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
        Animation riverBankOne, riverBankTwo, riverBankThree, riverBankFour;
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

                           
                                players[i].health -= 100;
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
            rockTime = randomizer.Next(1000, 2500);

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

            UpdateExplosions(gameTime);
            GameOverCheck();
        }
        Vector2 riverPos;
        Vector2 riverPosTwo;
        Vector2 riverPosThree;
        Vector2 riverSpeed = new Vector2(500, 0);
        Vector2 riverbankPos;
        Vector2 riverbankPosTwo;
        Vector2 riverbankPosThree;
        Vector2 riverbankPosFour;
        static public Texture2D particleWake;
        protected override void Update(GameTime gameTime)
        {

            riverPos -= riverSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            riverPosTwo -= riverSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            riverPosThree -= riverSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            riverbankPos -= riverSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            riverbankPosTwo -= riverSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            riverbankPosThree -= riverSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            riverbankPosFour -= riverSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
           
            if (riverPos.X<-1600)
            {
                riverPos.X = 3200;
            }
            if (riverPosTwo.X < -1600)
            {
                riverPosTwo.X = 3200;
            }
            if (riverPosThree.X < -1600)
            {
                riverPosThree.X = 3200;
            }
            if (riverbankPos.X < -3720)
            {
                riverbankPos.X = 3720;
            }
            if (riverbankPosTwo.X < -3720)
            {
                riverbankPosTwo.X = 3720;
            }
            if (riverbankPosThree.X < -3720)
            {
                riverbankPosThree.X = 3720;
            }
            if (riverbankPosFour.X < -3720)
            {
                riverbankPosFour.X = 3720;
            }
            riverBankOne.position = riverbankPos;
            riverBankOne.Update(gameTime);
            riverBankTwo.position = riverbankPosTwo;
            riverBankTwo.Update(gameTime);
            riverBankThree.position = riverbankPosThree;
            riverBankThree.Update(gameTime);
            riverBankFour.position = riverbankPosThree;
            riverBankFour.Update(gameTime);
            mouseState = Mouse.GetState();
            if (gameState == GameScreen.SelectScreen)
            {
                UpdateSelectScreen(gameTime);
            }
            if (gameState == GameScreen.GameRunning)
            {
                UpdateGame(gameTime);
            }
            if (gameState == GameScreen.MainMenu)
            {
                UpdateMenu(gameTime);
            }
            if (gameState == GameScreen.GameOver)
            {
                UpdateGameOverScreen();
            }



            base.Update(gameTime);
        }
        public void DrawGameOver()
        {
            //spriteBatch.Draw(backgroundGame, new Vector2(0, 0), Color.White);

            spriteBatch.Draw(winningTex, new Vector2((1920 - winningTex.Width) / 2, (1080 - winningTex.Height) / 2), Color.Green);

        }
        public void UpdateGameOverScreen()
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.B == ButtonState.Pressed)
            {
                gameState = GameScreen.MainMenu;
                ResetGame();
            }


        }
        public void ResetGame()
        {
            deadCount = 0;
            Initialize();
        }
        static public  int deadCount = 0;
        int winningPlayer;
        static Texture2D explosionTex;
        static List<Animation> explosions;
        static public void AddExplosion(Vector2 position, float scale)
        {
            Animation explosion = new Animation();


            explosion.spriteSheet = explosionTex;
            explosion.Initialize(true, 10, 0.25f, position, 0f, Color.White);
            explosion.scale = scale;
            explosions.Add(explosion);
        }
        public void GameOverCheck()
        {
            if (deadCount == (numOfPlayers - 1))
            {
                foreach (Player player in players)
                {
                    if (player.active)
                    {
                        winningPlayer = (int)player.playerNumber;
                        switch (winningPlayer)
                        {
                            case (int)PlayerIndex.One:
                                winningTex = playerOneWin;
                                break;
                            case (int)PlayerIndex.Two:
                                winningTex = playerTwoWin;
                                break;
                            case (int)PlayerIndex.Three:
                                winningTex = playerThreeWin;
                                break;
                            case (int)PlayerIndex.Four:
                                winningTex = playerFourWin;
                                break;
                        }
                        gameState = GameScreen.GameOver;
                    }
                }

            }
        }
        public void DrawExplosions(SpriteBatch spriteBatch)
        {
            foreach (Animation expl in explosions)
            {
                expl.Draw(spriteBatch);
            }
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
            spriteBatch.Draw(river, riverPos, Color.White);
            spriteBatch.Draw(riverTwo, riverPosTwo, Color.White);

            spriteBatch.Draw(riverThree, riverPosThree, Color.White);

            riverBankOne.Draw(spriteBatch);

            if (gameState == GameScreen.SelectScreen)
            {
                DrawIcons(spriteBatch);
            }
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


                    //spriteBatch.Draw(rectangleTexture, player.hitBox, Color.Green);


                    player.Draw(spriteBatch);
                }

                DrawExplosions(spriteBatch);
            }
           if(gameState == GameScreen.GameOver)
            {
                DrawGameOver();
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
