using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;


namespace PRR1_19_Visning
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game//, PowerUp
    {

        // IUpdate och IDraw används inte ens
        private static List<IUpdate> Update1 = new List<IUpdate>(); 
        private static List<IDraw> Draw1 = new List<IDraw>(); 

        static public void ADD(IUpdate update)
        {

        }


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D startButton, exitButton, pauseButton, resumeButton; // Menu texture2ds
        private Vector2 startButtonPosition, exitButtonPosition; // Menu vectorer


        Texture2D Player, Player2, Player3, Invader, Invader2, Bullet, Background, Ufo, Pill; // Spelar, invader, bullet och backgrunds texture2d'n

        Vector2 BackgroundPos = new Vector2(0, 0);

        List<Vector2> EnemyBulletPos = new List<Vector2>();

        public static int score;
        SpriteFont ScoreFont;
        Vector2 ScorePosition;

        Rectangle RectBullet, EnRectBullet, UfoRectBullet;


        float EnTimer = 3; // timer för fiendens bullet
        const float ResertTimer = 3; // Återställer tiden på timern

        double AnimationTimer = 1; // Timer för invadrarnas animation
        const double ResertAnimation = 1.5; // Återställer timern

        enum GameState { 
             Menu,
             Game,
             Paused
        }

        private enum Character // Enum för att välja olika karaktärer
        {
            Normal,
            Rambo,
            Female,
        }

        private Character character;

        bool InvaderAnimation = false;

        const int start = 0; // Enum för att välja scen
        const int spel = 1;
        const int gameover = 2;


        Rectangle[,] PlayerRec;
        int PlayerYPos = 340;
        int PlayerXPos = 100;


        Rectangle[,] UfoRec;
        int Ypos = 100;
        int size = 4;
        bool UfoIsHit = false;

        Random random = new Random();


        Rectangle[,] rectinvader;
        int rows = 4; // Antalet rader med fiender uppått
        int cols = 8; // Antalet rader med fiender åt sidan 
        bool IsHit = false;
        string direction = "Right";


        KeyboardState kNewState;
        KeyboardState kOldState;

        public Game1()
        { 
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            score = 0;
            ScorePosition.X = 10;
            ScorePosition.Y = 10;

            character = Character.Normal; // Alltså är standard charactären i början Normal/Player
            InvaderAnimation = false;

            SpeedUp redpill = new SpeedUp()
            {

            };

            graphics.PreferredBackBufferHeight = 500;
            graphics.PreferredBackBufferWidth = 850;
            //graphics.ApplyChanges(); // Något skapar en glitch med spelarens textur så att den dupliceras när man ändrar resolutionen och placerar handen längst ned

            base.Initialize();
        }

    
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ScoreFont = Content.Load<SpriteFont>("scorefnt");
            Ufo = Content.Load<Texture2D>("Ufo");
            Pill = Content.Load<Texture2D>("Pill");
            Player = Content.Load<Texture2D>("Player");
            Player2 = Content.Load<Texture2D>("Player2");
            Player3 = Content.Load<Texture2D>("Player3");
            Bullet = Content.Load<Texture2D>("Bullet");
            Invader = Content.Load<Texture2D>("Invader");
            Invader2 = Content.Load<Texture2D>("Invader2");


            // Genererar ett slumpmässigt nummer inom ett intervall
            int Randomnumber = random.Next(-1500, -500);


            // Player
            PlayerRec = new Rectangle[PlayerXPos, PlayerYPos];
            for (int x = 0; x < PlayerXPos; x++)
                for (int y = 0; y < PlayerYPos; y++)
                {
                    PlayerRec[x, y].X = 100;
                    PlayerRec[x, y].Y = 340;
                    PlayerRec[x, y].Width = Player.Width;
                    PlayerRec[x, y].Height = Player.Height;
                }


            // Ufo
            UfoRec = new Rectangle[Ypos, size];
            for (int s = 3; s < size; s++)
                for (int y = 0; y < Ypos; y++)
                {
                    UfoRec[y, s].Width = Ufo.Width / s;
                    UfoRec[y, s].Height = Ufo.Height / s;
                    UfoRec[y, s].Y = 100; // Y = y glitchar sig för att skeppet målas ut så många gånger som y++ sker
                    UfoRec[y, s].X = Randomnumber;
                }



            // Invader
            rectinvader = new Rectangle[rows, cols];
            for (int r = 0; r < rows; r++) // Lägger till invaders upp till r = rows 
                for (int c = 0; c < cols; c++) // Gör samma sak fast med columns
                {
                    rectinvader[r, c].Width = Invader.Width;
                    rectinvader[r, c].Height = Invader.Height;
                    rectinvader[r, c].X = 60 * c; // numret är distansen emmellan alla invaders
                    rectinvader[r, c].Y = 60 * r;
                }

            Background = Content.Load<Texture2D>("Background"); // Backgrunden i spelet

            // TODO: use this.Content to load your game content here
        }

        protected override void UnloadContent()
        {
            // Unload any non ContentManager content here
        }

        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {

            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            kNewState = Keyboard.GetState();
            for (int x = 0; x < PlayerXPos; x++)
                for (int y = 0; y < PlayerYPos; y++)
                {
                    if ((kNewState.IsKeyDown(Keys.Right)) && (PlayerRec[x, y].X < 700)) // Ändrar positionen för spelaren samt håller spelaren inom ramen
                        PlayerRec[x, y].X += 6;
                    if ((kNewState.IsKeyDown(Keys.Left)) && (PlayerRec[x, y].X > 0))
                        PlayerRec[x, y].X -= 6;

                    if (kNewState.IsKeyDown(Keys.NumPad1)) // Ennum för karaktärsbyte
                        character = Character.Normal; // See till att kunna få skiten att fungera nu också

                        if (kNewState.IsKeyDown(Keys.NumPad2)) // Enum för karaktärsbyte
                            character = Character.Rambo;

                            if (kNewState.IsKeyDown(Keys.NumPad3)) // Enum för karaktärsbyte
                                character = Character.Female;
                }


            // Flyttar ufot in mot skärmen
            for (int s = 3; s < size; s++) 
                for (int y = 0; y < Ypos; y++)
                {                 
                    UfoRec[y, s].X = UfoRec[y, s].X + 1;
                }


            // Invadrarnas kanter
            int rightside = graphics.GraphicsDevice.Viewport.Width;
            int leftside = 0;


            // Ändrar positionen för alla invaders
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    if (direction.Equals("Right"))
                        rectinvader[r, c].X = rectinvader[r, c].X + 2;
                    if (direction.Equals("Left"))
                        rectinvader[r, c].X = rectinvader[r, c].X - 2;
                }


            // Kollar om invaderserna når kanten
            string changedir = "No";
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                {
                    if (rightside < rectinvader[r, c].X + rectinvader[r, c].Width)
                    {
                        direction = "Left";
                        changedir = "Yes";
                    }

                    if (rectinvader[r, c].X < leftside)
                    {
                        direction = "Right";
                        changedir = "Yes";
                    }

                    if (changedir.Equals("Yes"))
                    {
                        rectinvader[r, c].Y += 7;
                    }
                }


            // Skjuter bullets från playern
            if (kNewState.IsKeyDown(Keys.Space) && kOldState.IsKeyUp(Keys.Space))
                for (int x = 0; x < PlayerXPos; x++)
                    for (int y = 0; y < PlayerYPos; y++)
                    {
                        IsHit = false;
                        RectBullet.X = PlayerRec[x, y].X + 45; // +45 är så att bulleten ska åka ur fingret och inte vänster om spelaren    
                        RectBullet.Y = PlayerRec[x, y].Y;
                    }


            // Flyttar på playerbullet
            if (IsHit == false && UfoIsHit == false)
            {
                RectBullet.Y -= 3; 
            }


            // Timer som kontrolerar intervallet som fiender skjuter bullets och skjuter Enbullet och Ufobullet
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            EnTimer -= elapsed;
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    for (int s = 3; s < size; s++)
                        for (int y = 0; y < Ypos; y++)
                            if (EnTimer < 0)
                    {

                        EnTimer = ResertTimer;

                        EnRectBullet.X = rectinvader[r, c].X;
                        EnRectBullet.Y = rectinvader[r, c].Y;

                        UfoRectBullet.X = UfoRec[y, s].X + 70; // + 70 används för att kompensera för ufots rörelse så att det ser ut som bulletn åker ut från mitten av ufot
                        UfoRectBullet.Y = UfoRec[y, s].Y;
                    }


            // Ger invadrarna en anmation via en timer kallad animationtimer
            float elapsed2 = (float)gameTime.ElapsedGameTime.TotalSeconds;
            AnimationTimer -= elapsed2;
            if (AnimationTimer < 0.5 && AnimationTimer > 0)
            {
                InvaderAnimation = false;
            }

            else
            {
                InvaderAnimation = true;
            }

            // Återställer timern när den når noll
            if (AnimationTimer < 0)
            {
                AnimationTimer = ResertAnimation;
            }


            // Flyttar invadrarnas bullets i y led
            if (0 == 0) // Jag valde att skriva såhär då det alltid är sant
            {
                EnRectBullet.Y += 3;
                UfoRectBullet.Y += 3;
            }



            // Om invadern träffas av en bullet
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                        if (rectinvader[r, c].Contains(RectBullet)) 
                        {
                            rectinvader[r, c].Y = -10000;
                            RectBullet.Y += 1000;
                            IsHit = true;
                            score += 10;
                        }

                // Om playerbullet träffar   
                if (RectBullet.Intersects(EnRectBullet))
                {
                    RectBullet.X += 1000;
                    EnRectBullet.X += 1000;

                }

            // Om playerbullet träffar ufobullet
            if (RectBullet.Intersects(UfoRectBullet))
            {
                RectBullet.X += 1000;
                UfoRectBullet.X += 1000;
            }



            // Om en invader har kommit för nära playern
            for (int x = 0; x < PlayerXPos; x++)
                for (int y = 0; y < PlayerYPos; y++)
                    for (int r = 0; r < rows; r++)
                         for (int c = 0; c < cols; c++)
                        if (PlayerRec[x, y].Y < rectinvader[r, c].Y)
                        {
                             Exit();
                        }


            // Om spelarens träffas av en bullet
                for (int x = 0; x < PlayerXPos; x++)
                    for (int y = 0; y < PlayerYPos; y++)
                        if (PlayerRec[x, y].Contains(EnRectBullet) || PlayerRec[x, y].Contains(UfoRectBullet)) // Om spelaren träffas av antingen Invaderbullet eller Ufobullet
                        {
                            Exit();
                        }



            // Om Ufot träffas av playerbullet
                for (int s = 0; s < size; s++)
                    for (int y = 0; y < Ypos; y++)
                        if (UfoRec[y, s].Contains(RectBullet))
                        {
                            UfoRec[y, s].Y = -10000;
                            RectBullet.Y += 1000;
                            UfoIsHit = true;
                            score += 50;
                        }


                // Tar bort objekt säkert
                RemoveObjects();



            kOldState = kNewState;


            base.Update(gameTime);
        }

        /// This is called when the game should draw itself.
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();

            // Allting som skrivs ut bakom denna bild kommer inte att kunna synas
            spriteBatch.Draw(Background, BackgroundPos, Color.White);

            // Ritar ut score på skärmen under spelet
            spriteBatch.DrawString(ScoreFont, "Score: " + score.ToString(), ScorePosition, Color.White);


            // Ritar ut enemybullet
            if (0 == 0) // Använder if 0 == 0 då det alltid är sant
            {
                EnRectBullet.Size = new Point(30, 20);
                spriteBatch.Draw(Bullet, EnRectBullet, Color.Red);

                UfoRectBullet.Size = new Point(50, 30);
                spriteBatch.Draw(Bullet, UfoRectBullet, Color.Red);
            }

            // Ritar ut invadrarna på både x och y axeln (r, c)
            if (InvaderAnimation == false)
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    spriteBatch.Draw(Invader, rectinvader[r, c], Color.White);

            else
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                    {
                spriteBatch.Draw(Invader2, rectinvader[r, c], Color.White);
            }


            // Ritar ut ufot
            for (int s = 0; s < size; s++)
                for (int y = 0; y < Ypos; y++)
                    spriteBatch.Draw(Ufo, UfoRec[y, s], Color.White);


            // Ritar ut Playern
            if (character == Character.Normal)
            for (int x = 0; x < PlayerXPos; x++)
                for (int y = 0; y < PlayerYPos; y++)
                    spriteBatch.Draw(Player, PlayerRec[x, y], Color.White);

            // Ritar ut Player2
            if (character == Character.Rambo)
                for (int x = 0; x < PlayerXPos; x++)
                    for (int y = 0; y < PlayerYPos; y++)
                        spriteBatch.Draw(Player2, PlayerRec[x, y], Color.White);

            // Ritar ut Player3
            if (character == Character.Female)
                for (int x = 0; x < PlayerXPos; x++)
                    for (int y = 0; y < PlayerYPos; y++)
                        spriteBatch.Draw(Player3, PlayerRec[x, y], Color.White);


            // Ritar ut playerbullet
            if (IsHit == false && UfoIsHit == false)
            {
                RectBullet.Size = new Point(30, 25);
                spriteBatch.Draw(Bullet, RectBullet, Color.White);
            }


            // Ritar ut RedPill alltså Powerdownen(power) slowdown
            //spriteBatch.Draw(Pill, new Vector2(20, 20), Color.Red);


            // Ritar ut GreenPill alltså Powerupen Speedup
            //spriteBatch.Draw(Pill, new Vector2(20, 20), Color.Green);


            spriteBatch.End();

            base.Draw(gameTime);
        }

        void RemoveObjects()
        {

        }
    }
}

