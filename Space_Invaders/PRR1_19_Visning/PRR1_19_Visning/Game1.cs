using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace PRR1_19_Visning
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {

        private static List<IUpdate> Update1 = new List<IUpdate>();
        private static List<IDraw> Draw1 = new List<IDraw>();

        static public void ADD(IUpdate update)
        {

        }


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D Player, Player2, Invader, Invader2, Bullet, Background, Ufo; // Spelar, invader, bullet och backgrunds texture2d'n
        Vector2 BackgroundPos = new Vector2(0, 0);
        List<Vector2> PlayerBulletPos = new List<Vector2>();
        List<Vector2> EnemyBulletPos = new List<Vector2>();

        public static int score;
        public static int hiscore;
        SpriteFont ScoreFont;
        Vector2 ScorePosition;


        float EnTimer = 2; // timer för fiendens bullet
        const float ResertTimer = 2; // Återställer tiden på timern

        const int start = 0; // Enum för att välja scen
        const int spel = 1;
        const int gameover = 2;


        Rectangle[,] PlayerRec;
        int PlayerYPos = 340;
        int PlayerXPos = 100;


        Rectangle[,] UfoRec;
        int Ypos = 100;
        int size = 4;


        Rectangle[,] rectinvader;
        int rows = 4; // Antalet rader med fiender uppått
        int cols = 8; // Antalet rader med fiender åt sidan 
        bool ishit = false;
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
            Player = Content.Load<Texture2D>("Player");
            Bullet = Content.Load<Texture2D>("Bullet");
            Player2 = Content.Load<Texture2D>("Player2");
            Invader = Content.Load<Texture2D>("Invader");
            Invader2 = Content.Load<Texture2D>("Invader2");           


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
                    UfoRec[y, s].X = -1500;
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
                    Vector2 EnemyPos = new Vector2(rectinvader[r, c].X, rectinvader[r, c].Y); // EnemyPos värdet används för att bestämma varifrån enemybullet ska uppstå
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
                        rectinvader[r, c].X = rectinvader[r, c].X + 1;
                    if (direction.Equals("Left"))
                        rectinvader[r, c].X = rectinvader[r, c].X - 1;
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
                        rectinvader[r, c].Y = rectinvader[r, c].Y + 7;
                    }
                }

            // Skjuter bullets från playern
            if (kNewState.IsKeyDown(Keys.Space) && kOldState.IsKeyUp(Keys.Space))
                for (int x = 0; x < PlayerXPos; x++)
                    for (int y = 0; y < PlayerYPos; y++)
                    {
                        Vector2 PlayerPos1 = new Vector2(PlayerRec[x, y].X, PlayerRec[x, y].Y);

                        {
                            PlayerBulletPos.Add(PlayerPos1);
                        }
                    }


            // Timer som kontrolerar intervallet som fiender skjuter bullets
            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            EnTimer -= elapsed;
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    if (EnTimer < 0)
                    {
                        EnTimer = ResertTimer;
                        //EnemyBulletPos.Add(Ufo);
                         // EnemyBulletPos.Add(Player);
                    }


            // Flyttar playerns bullet i y led
            for (int i = 0; i < PlayerBulletPos.Count; i++)
            {
                PlayerBulletPos[i] = PlayerBulletPos[i] - new Vector2(0, 3);
            }


            // Om invadern träffas av en bullet
            foreach (Vector2 bullet in PlayerBulletPos)
                for (int r = 0; r < rows; r++)
                    for (int c = 0; c < cols; c++)
                        if (rectinvader[r, c].Contains(bullet)) 
                        {
                            rectinvader[r, c].Y = 10000;
                            //Bullet.Y = 10000;
                            score += 10;
                        }


            // Om spelarens träffas av en bullet
            foreach (Vector2 bullet in EnemyBulletPos)
                for (int x = 0; x < PlayerXPos; x++)
                    for (int y = 0; y < PlayerYPos; y++)
                        if (PlayerRec[x, y].Contains(bullet)) 
                        {
                            Exit();
                        }



            // Om Ufot träffas av playerbullet
            foreach (Vector2 bullet in PlayerBulletPos)
                for (int s = 0; s < size; s++)
                    for (int y = 0; y < Ypos; y++)
                        if (UfoRec[y, s].Contains(bullet))
                        {
                            UfoRec[y, s].Y = 10000;
                            //PlayerBulletPos.Y = 10000;
                            ishit = true;
                            score += 50;
                        }


            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)

                    if (rectinvader[r, c].Y > 9999) // Om Invadernars bullet träffar spelarens bullet
                    {
                        // Exit();
                    }



            for (int i = 0; i < EnemyBulletPos.Count; i++)
            {
                EnemyBulletPos[i] = EnemyBulletPos[i] - new Vector2(0, -1);
            }

            foreach (Vector2 bullet in EnemyBulletPos)
                // for (int r = 0; r < rows; r++)
                //   for (int c = 0; c < cols; c++)
                //if (Player.Contains(Bullet)) // Player har ingen deifinition för .Contains behöver hitta alternativ metod
                // {
                //   .Exit()// Behöver kod här som gör att man kan avsluta programmet och visa hiscore
                // }




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


            // Ritar ut invadrarna på både x och y axeln (r, c)
            for (int r = 0; r < rows; r++)
                for (int c = 0; c < cols; c++)
                    spriteBatch.Draw(Invader, rectinvader[r, c], Color.White);


            // Ritar ut ufot
            for (int s = 0; s < size; s++)
                for (int y = 0; y < Ypos; y++)
                    spriteBatch.Draw(Ufo, UfoRec[y, s], Color.White);


            // Ritar ut Playern
            for (int x = 0; x < PlayerXPos; x++)
                for (int y = 0; y < PlayerYPos; y++)
                    spriteBatch.Draw(Player, PlayerRec[x, y], Color.White);     


            // Rítar ut playerbullet
            foreach (Vector2 bulletPos in PlayerBulletPos)
            {
                Rectangle rec = new Rectangle();
                rec.Location = bulletPos.ToPoint();
                rec.Size = new Point(20, 15);
                spriteBatch.Draw(Bullet, rec, Color.White);
            }


            // Ritar ut enemybullet
            foreach (Vector2 bulletPos in EnemyBulletPos)
            {
                Rectangle rec = new Rectangle();
                rec.Location = bulletPos.ToPoint();
                rec.Size = new Point(30, 20);
                spriteBatch.Draw(Bullet, rec, Color.Red);
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }

        void RemoveObjects()
        {

        }
    }
}

