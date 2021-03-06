﻿
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using Windows.UI.Notifications;
using Windows.Data.Xml.Dom;

namespace BoatRaceFlee
{
    
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        MouseState mouseState;
        Random randomizer;

        static public List<Rectangle> explosionHitBoxes;
        List<Vector2> SpawnList = new List<Vector2>();
        List<Pickup> pickups;

        List<Player> players;
        
        int numOfPlayers = 4;
        enum GameScreen
        {
            MainMenu,
            SelectScreen,
            DificultySelect,
            GameRunning,
            GameOver,
            NumSelect

        }
        GameScreen gameState = GameScreen.MainMenu;
        public Game1()
        {

            IsMouseVisible = true;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.IsFullScreen = true;
            graphics.PreferredBackBufferHeight = 1080;

            graphics.PreferredBackBufferWidth = 1920;

            
        }
        List<Texture2D> playerIcons;
        Texture2D beachRing;
        Texture2D longBoat;
        Texture2D airCarrier;
        Texture2D battleShip;
        Texture2D river, duck;
        public void InitializeSelectScreen()
        {

            beachRing = Content.Load<Texture2D>("BeachRing");
            longBoat = Content.Load<Texture2D>("Longboat");
            airCarrier = Content.Load<Texture2D>("Aircraft");
            battleShip = Content.Load<Texture2D>("BattleShip");

            duck = Content.Load<Texture2D>("Duck");
            playerIcons = new List<Texture2D>();
            playerTexNames = new List<string>();

            playerIcons.Add(beachRing);
            playerIcons.Add(longBoat);
            playerIcons.Add(airCarrier);
            playerIcons.Add(battleShip);
            playerIcons.Add(duck);


        }
        int readyPlayers = 0;
        List<string> playerTexNames;
        public void UpdateSelectScreen(GameTime gameTime)
        {

            eldelay += (float)gameTime.ElapsedGameTime.Milliseconds;
            if (eldelay > delay)
            {
                for (int i = 0; i < numOfPlayers; i++)
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
                if (readyPlayers == numOfPlayers)
                {
                    gameState = GameScreen.DificultySelect;
                }
            }



        }
        float elapsedTime;

        float elapsedmenuTime;
        public void PlayerSelect(GameTime gameTime, int num)
        {

            if (0 <= players[num].playerSelected && players[num].playerSelected < playerIcons.Count)
            {

                elapsedmenuTime += gameTime.ElapsedGameTime.Milliseconds;
                if ((int)(GamePad.GetState(players[num].playerNumber).ThumbSticks.Left.X) != 0)
                    {
                    if (elapsedmenuTime > menuTime)
                    {
                        players[num].playerSelected += (int)(GamePad.GetState(players[num].playerNumber).ThumbSticks.Left.X );
                        elapsedmenuTime = 0;
                    }
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
        float explosionsTimer = 200;
        int expnum= 0;
        public void ScreenExplosions(GameTime gameTime)
        {
            
                elapsedTime += gameTime.ElapsedGameTime.Milliseconds;
                if (explosionsTimer < elapsedTime)
                {
                    AddExplosion(new Vector2(20, 50 * (expnum + 1)), 3f);
                AddExplosion(new Vector2(20, 1080-50 * (expnum + 1)), 3f);
                elapsedTime = 0;
                if( expnum<18)
                    {
                    expnum++;
                    }
                else
                {
                    expnum = 0;
                }
                }
            
        }

        public void InitializePlayers(int numOfPlayers)
        {
            SpawnList.Add(new Vector2(2 + 150, 250));
            SpawnList.Add(new Vector2(2 + 150, 450));
            SpawnList.Add(new Vector2(2 + 150, 650));
            SpawnList.Add(new Vector2(2 + 150, 850));
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
            List<Pickup> pickups = new List<Pickup>();
            explosionHitBoxes = new List<Rectangle>();
            screenScale = new Vector3(scaleX, scaleY, 1.0f);
            mouseState = new MouseState();
            players = new List<Player>();
            obstacleList = new List<Obstacle>();
            InitializePlayers(4);
        }
      

        List<Button> menuButtons;

        List<Button> difButtons;
        List<Button> numButtons;
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
        public void InitializeDifMenu()
        {
            difButtons = new List<Button>();

            Button button = new Button();
            button.Initialize(new Vector2((1920) / 2, 300 + 140 / 2), "EasyButton", "Easy", 1);
            difButtons.Add(button);

            button = new Button();
            button.Initialize(new Vector2((1920) / 2, 500 + 140 / 2), "NormalButton", "Normal", 2);
            difButtons.Add(button);

            button = new Button();
            button.Initialize(new Vector2((1920) / 2, 700 + 140 / 2), "HardButton", "Hard", 3);
            difButtons.Add(button);
        }
        public void InitializeNumMenu()
        {
            numButtons = new List<Button>();

            Button button = new Button();
            button.Initialize(new Vector2((1920) / 2, 300 + 140 / 2), "2", "2", 1);
            numButtons.Add(button);

            button = new Button();
            button.Initialize(new Vector2((1920) / 2, 500 + 140 / 2), "3", "3", 2);
            numButtons.Add(button);

            button = new Button();
            button.Initialize(new Vector2((1920) / 2, 700 + 140 / 2), "4", "4", 3);
            numButtons.Add(button);
        }
        int menuTime = 200;
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
        float elapseddifmenuTime = 0;
        public void DifMenuSelect(GameTime gameTime, PlayerIndex num)
        {
            if (1 <= currentMenuItem && currentMenuItem <= difButtons.Count)
            {

                elapseddifmenuTime += gameTime.ElapsedGameTime.Milliseconds;
                if ((GamePad.GetState(num).ThumbSticks.Left.Y) != 0)
                {
                    if (elapseddifmenuTime > menuTime)
                    {
                        currentMenuItem -= MathHelper.Clamp( (int)(GamePad.GetState(num).ThumbSticks.Left.Y*1000),-1,1);
                        elapseddifmenuTime = 0;
                    }
                }



            }


            if (1 > currentMenuItem)
            {
                currentMenuItem = 1;
            }
            if (currentMenuItem > difButtons.Count)
            {
                currentMenuItem = difButtons.Count;
            }
        }
        float elapsednummenuTime = 0;
        public void PlayerNumMenuSelect(GameTime gameTime, PlayerIndex num)
        {
            if (1 <= currentMenuItem && currentMenuItem <=numButtons.Count)
            {

                elapsednummenuTime += gameTime.ElapsedGameTime.Milliseconds;
                if ((GamePad.GetState(num).ThumbSticks.Left.Y) != 0)
                {
                    if (elapsednummenuTime > menuTime)
                    {
                        currentMenuItem -= MathHelper.Clamp((int)(GamePad.GetState(num).ThumbSticks.Left.Y * 1000), -1, 1);
                        elapsednummenuTime = 0;
                    }
                }



            }


            if (1 > currentMenuItem)
            {
                currentMenuItem = 1;
            }
            if (currentMenuItem > numButtons.Count)
            {
                currentMenuItem = numButtons.Count;
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
                        gameState = GameScreen.NumSelect;
                        
                    }
                    if (button.buttonName == "exit")
                    {
                        Exit();
                    }
                }
            }

        }
        int dif;
        public void DifUpdateMenu(GameTime gameTime)
        {
            DifMenuSelect(gameTime, PlayerIndex.One);
            foreach (Button button in difButtons)
            {
                button.Update(gameTime, mouseState, currentMenuItem);
                if (button.CheckForClick(mouseState))
                {

                    if (button.buttonName == "Easy")
                    {
                        dif = 1;
                        gameState = GameScreen.GameRunning;
                    }
                    if (button.buttonName == "Normal")
                    {
                        dif = 2;
                        gameState = GameScreen.GameRunning;
                    }
                    if (button.buttonName == "Hard")
                    {

                        dif = 3;
                        gameState = GameScreen.GameRunning;
                    }
                }
            }

        }
        float delay= 200;
        float eldelay=0;
        public void NumUpdateMenu(GameTime gameTime)
        {
            eldelay += (float)gameTime.ElapsedGameTime.Milliseconds;
            if (eldelay > delay)
            {
                PlayerNumMenuSelect(gameTime, PlayerIndex.One);
                foreach (Button button in numButtons)
                {
                    button.Update(gameTime, mouseState, currentMenuItem);
                    if (button.CheckForClick(mouseState))
                    {

                        if (button.buttonName == "2")
                        {
                            players.RemoveAt(3);
                            players.RemoveAt(2);
                            
                            numOfPlayers = 2;
                            eldelay = 0;
                            gameState = GameScreen.SelectScreen;
                        }
                        if (button.buttonName == "3")
                        {
                            players.RemoveAt(3);
                            numOfPlayers = 3;
                            eldelay = 0;
                            gameState = GameScreen.SelectScreen;
                        }
                        if (button.buttonName == "4")
                        {

                            numOfPlayers = 4;
                            eldelay = 0;
                            gameState = GameScreen.SelectScreen;
                        }
                    }
                }
            }

        }
        public void DrawMenu(GameTime gameTime)
        {
            spriteBatch.Draw(Title, new Vector2((1920- 1898 )/ 2, 200), Color.White);
            foreach (Button button in menuButtons)

            {
                button.Draw(spriteBatch);
            }

        }
        public void DrawDifMenu(GameTime gameTime)
        {
            
            foreach (Button button in difButtons)

            {
                button.Draw(spriteBatch);
            }

        }
        public void DrawNumMenu(GameTime gameTime)
        {

            foreach (Button button in numButtons)

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
                    explosionHitBoxes.RemoveAt(i);
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
            riverbankPos= new Vector2(3720 / 2, 28);
            riverbankPosTwo= new Vector2((3720 / 2) + 3720, 28);

            riverbankPosThree = new Vector2(3720 / 2, 1080- 28);
            riverbankPosFour = new Vector2((3720 / 2)+ 3720, 1080 - 28);
            randomizer = new Random();
            riverBankOne = new Animation();
            riverBankTwo = new Animation();
            riverBankThree = new Animation();
            riverBankFour = new Animation();
            riverBankOne.Initialize(1, 1, riverbankPos, 0f, Color.White);
            riverBankTwo.Initialize(1, 1, riverbankPosTwo, 0f, Color.White);
            riverBankThree.Initialize(1, 1, riverbankPosThree, 0f, Color.White);
            riverBankFour.Initialize(1, 1, riverbankPosFour, 0f, Color.White);
            pickups = new List<Pickup>();
            InitializeSelectScreen();
            InitializeMainMenu();
            InitializeDifMenu();
            InitializeNumMenu();
            totalSeconds = 0;
            InitializeGame();
            explosions = new List<Animation>();
            readyPlayers = 0;
            eldelay = 0;
            base.Initialize();
        }

        Texture2D riverTwo;
        Texture2D riverThree;
        Texture2D playerOneWin, playerTwoWin, playerThreeWin, playerFourWin, winningTex;
        Texture2D Title;
        protected override void LoadContent()
        {

            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Title = Content.Load<Texture2D>("Title");
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
            foreach (Button button in difButtons)
            {
                button.LoadContent(Content, button.fileName);
            }
            foreach (Button button in numButtons)
            {
                button.LoadContent(Content, button.fileName);
            }


        }

        List<Obstacle> obstacleList = new List<Obstacle>();
        public void AddPickup()
        {
            Pickup pickup = new Pickup();
            pickup.LoadContent(Content, "fuel");
            pickup.Initialize(new Vector2(300, 0),  new Vector2(1970, randomizer.Next(100, 1000)),5000f,300f);
           
            pickups.Add(pickup);

        }
        public void UpdatePickup(GameTime gameTime)
        {
            foreach(Pickup pickup in pickups)
            {
                pickup.Update(gameTime);
            }
        }
        public void DrawPickup(SpriteBatch spriteBatch)
        {
            foreach (Pickup pickup in pickups)
            {
                pickup.Draw(spriteBatch);
            }
        }
        public void AddRock()
        {
            for (int k = 0; k < randomizer.Next(1, 5); k++)
            {
                Obstacle obstacle = new Obstacle();
                float scale = (float)randomizer.NextDouble()  + 0.5f;
                obstacleAnimation = new Animation();
                obstacleAnimation.scale = scale;
                obstacleAnimation.LoadContent(Content, "Rock");
                Vector2 pos = new Vector2(1920 + obstacleAnimation.frameWidth, randomizer.Next(obstacleAnimation.frameHeight, 1080- obstacleAnimation.frameHeight));
                obstacle.angle = (float)randomizer.Next(0, 180);
                obstacle.Initialize(pos, new Vector2(300f+randomizer.Next(-50,50), 0), obstacleAnimation);

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
        public void PickupCollision()
        {

            for (int i = 0; i < players.Count; i++)
            {
                for (int j = 0; j < pickups.Count; j++)
                {
                    if (players[i].hitBox.Intersects(pickups[j].hitBox))
                    {
                        if (Collision.CollidesWith(players[i].playerAnimation,
                        pickups[j].pickupAnimation, players[i].playerTransformation,
                        pickups[j].pickupAnimation.transformation))

                        {


                            players[i].buff += pickups[j].velocityBuff;
                            players[i].buffTime += pickups[j].buffTime;
                            pickups.RemoveAt(j);
                        }
                    }
                }
            }

        }
        public void SideCollision()
        {

            for (int i = 0; i < players.Count; i++)
            {
               
                    if (players[i].hitBox.Intersects(new Rectangle(0, 0, 1920, 60)))
                    {
                        if (Collision.CollidesWith(players[i].playerAnimation,
                        riverBankOne, players[i].playerTransformation,
                        riverBankOne.transformation))

                        {


                            players[i].health -= 100;
                        }
                        if (Collision.CollidesWith(players[i].playerAnimation,
                        riverBankTwo, players[i].playerTransformation,
                        riverBankTwo.transformation))

                        {


                            players[i].health -= 100;
                        }
                    }
                    if (players[i].hitBox.Intersects(new Rectangle(0, 1020, 1920, 60)))
                    {
                        if (Collision.CollidesWith(players[i].playerAnimation,
                        riverBankThree, players[i].playerTransformation,
                        riverBankThree.transformation))

                        {


                            players[i].health -= 100;
                        }
                        if (Collision.CollidesWith(players[i].playerAnimation,
                        riverBankFour, players[i].playerTransformation,
                        riverBankFour.transformation))

                        {


                            players[i].health -= 100;
                        }
                    }
                
            }

        }



        protected override void UnloadContent()
        {
        }
        
        float rockElapsedTime;
        float rockTime=2000;
        float pickupTime= 5000;
        float pickupElapsedTime;
        float totalSeconds;
        public void UpdateGame(GameTime gameTime)
        {
            totalSeconds += (int)(gameTime.ElapsedGameTime.TotalSeconds);
            if (2000 -( totalSeconds * 10*(dif*2)) < 3000 - (totalSeconds * 20) * (dif * 2))
            {
                rockTime = randomizer.Next(2000 - (int)(totalSeconds * 10), 3000 - (int)(totalSeconds * 20));
            }
            else
            {
                rockTime = 500;
            }
            rockElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (rockElapsedTime > rockTime)
            {
                AddRock();
                rockElapsedTime = 0;
            }
            pickupElapsedTime += gameTime.ElapsedGameTime.Milliseconds;
            if (pickupElapsedTime > pickupTime)
            {
                AddPickup();
                pickupElapsedTime = 0;
            }
            UpdatePickup(gameTime);
            foreach (Obstacle rock in obstacleList)
            {
                rock.Update(gameTime);
            }
            RockCollision();
            foreach (Player player in players)
                player.Update(gameTime);
            ScreenExplosions(gameTime);
            UpdateExplosions(gameTime);
            Explosivecollisions();
            SideCollision();
            PickupCollision();
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

        public void Explosivecollisions()
        {
            foreach (Player ply in players)
            {
                for (int i = 0; i <explosionHitBoxes.Count;i++)
                {
                    if (explosionHitBoxes[i].Intersects(ply.hitBox))
                    {
                       if( Collision.CollidesWith(ply.playerAnimation,explosions[i],ply.playerTransformation,explosions[i].transformation))
                            {
                            ply.health -= 100;
                        }
                    }
                    
                }
            }
        }
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
            if (riverbankPos.X < -3720 / 2)
            {
                riverbankPos.X = (3720 / 2) + 3726;
            }
            if (riverbankPosTwo.X < -3720 / 2)
            {
                riverbankPosTwo.X = (3720 / 2) + 3726;
            }
            if (riverbankPosThree.X < -3720 / 2)
            {
                riverbankPosThree.X = (3720 / 2) + 3726;
            }
            if (riverbankPosFour.X < -3720 / 2)
            {
                riverbankPosFour.X = (3720 / 2) + 3726;
            }
            riverBankOne.position = riverbankPos;
            riverBankOne.Update(gameTime);
            riverBankTwo.position = riverbankPosTwo;
            riverBankTwo.Update(gameTime);
            riverBankThree.position = riverbankPosThree;
            riverBankThree.Update(gameTime);
            riverBankFour.position = riverbankPosFour;
            riverBankFour.Update(gameTime);
            mouseState = Mouse.GetState();
            if (gameState == GameScreen.NumSelect)
            {
                NumUpdateMenu(gameTime);
            }
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
            if (gameState == GameScreen.DificultySelect)
            {
                DifUpdateMenu(gameTime);
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
            explosionHitBoxes.Add(new Rectangle((int)position.X, (int)position.Y, explosion.frameWidth * 2, explosion.frameHeight * 2));

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
                        ToastTemplateType toastTemplate = ToastTemplateType.ToastImageAndText01;
                        XmlDocument toastXml = ToastNotificationManager.GetTemplateContent(toastTemplate);
                        XmlNodeList toastTextElements = toastXml.GetElementsByTagName("text");
                        int winnum = winningPlayer + 1;
                        toastTextElements[0].AppendChild(toastXml.CreateTextNode("Player "+ winnum + " Wins!" ));


                        ToastNotification toast = new ToastNotification(toastXml);

                        ToastNotificationManager.CreateToastNotifier().Show(toast);

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
            riverBankTwo.Draw(spriteBatch);
            riverBankThree.Draw(spriteBatch);
            riverBankFour.Draw(spriteBatch);


            if (gameState == GameScreen.SelectScreen)
            {
                DrawIcons(spriteBatch);
            }
            if (gameState == GameScreen.NumSelect)
            {
                DrawNumMenu(gameTime);
            }
            if (gameState == GameScreen.MainMenu)
            {
                DrawMenu(gameTime);
            }
            if (gameState == GameScreen.DificultySelect)
            {
                DrawDifMenu(gameTime);
            }
            if (gameState == GameScreen.GameRunning)
            {
                foreach (Pickup pickup in pickups)
                {
                    //spriteBatch.Draw(rectangleTexture, pickup.hitBox, Color.Green);
                    pickup.Draw(spriteBatch);

                }
                foreach (Player player in players)
                {


                    //spriteBatch.Draw(rectangleTexture, player.hitBox, Color.Green);


                    player.Draw(spriteBatch);
                }
                foreach (Obstacle rock in obstacleList)
                {
                    rock.Draw(spriteBatch);

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
