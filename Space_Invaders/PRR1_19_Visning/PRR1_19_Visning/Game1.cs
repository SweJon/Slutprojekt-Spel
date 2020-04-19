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

        Texture2D Player, Invader, Invader2, Bullet, Background; // Spelar, invader, bullet och backgrunds texture2d'n
        Rectangle PlayerRec;
        Vector2 BackgroundPos = new Vector2(0,0);
        Vector2 PlayerPos = new Vector2(100, 340);  // Positionen
        Vector2 InvaderPos = new Vector2(50, 20);  //Invaderns startposition
        List<Vector2> PlayerBulletPos = new List<Vector2>();
        List<Vector2> EnemyBulletPos = new List<Vector2>();
        public static int score = 0;
        public static int hiscore;


        float EnTimer = 2; // timer för fiendens bullet
        const float ResertTimer = 2; // Återställer tiden på timern

        const int start = 0; // Enum för att välja scen
        const int spel = 1;
        const int gameover = 2;


        Rectangle[,] rectinvader; 
        int rows = 4; // Antalet rader med fiender uppått
        int cols = 8; // Antalet rader med fiender åt sidan 
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
            // TODO: Add your initialization logic here
            
            graphics.PreferredBackBufferHeight = 850;  
            graphics.PreferredBackBufferWidth = 850;
            //graphics.ApplyChanges(); // Något skapar en glitch med spelarens textur så att den dupliceras när man ändrar resolutionen och placerar handen längst ned

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
            Player = Content.Load<Texture2D>("Player");
            Bullet = Content.Load<Texture2D>("Bullet");
            Invader = Content.Load<Texture2D>("Invader");
            Invader2 = Content.Load<Texture2D>("Invader2");
            PlayerRec = new Rectangle(); // Gör om spelaren till en rectangle för att kollisionen ska fungera


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
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            kNewState = Keyboard.GetState();

            if ((kNewState.IsKeyDown(Keys.Right)) && (PlayerPos.X < 700)) // Ändrar positionen för spelaren samt håller spelaren inom ramen
                PlayerPos.X+=6;
            if ((kNewState.IsKeyDown(Keys.Left)) && (PlayerPos.X > 0))
                PlayerPos.X-=6;


            int rightside = graphics.GraphicsDevice.Viewport.Width;
            int leftside = 0; 

            // Ändrar positionen för alla invaders
            for (int r = 0; r < rows; r++) 
                for (int c = 0; c < cols; c++)
                    {
                    if (direction.Equals("Right"))
                        rectinvader[r, c].X =  rectinvader[r, c].X + 1;
                    if (direction.Equals("Left"))
                        rectinvader[r, c].X =  rectinvader[r, c].X - 1;
                    }


            // Kollar om invaderserna når kanten
            string changedir = "No";
            for (int r = 0; r < rows; r++) 
                for (int c = 0; c < cols; c++)
                    {
                       if(rectinvader[r, c].X +  rectinvader[r, c].Width > rightside)
                        {
                        direction = "Left";
                        changedir = "Yes";
                        }

                       if(rectinvader[r, c].X < leftside)
                        {
                        direction = "Right";
                        changedir = "Yes";
                        }

                    if (changedir.Equals("Yes"))
                        {
                           rectinvader[r, c].Y = rectinvader[r, c].Y + 7;
                        }
                    }


            if(kNewState.IsKeyDown(Keys.Space) && kOldState.IsKeyUp(Keys.Space))
            {
                PlayerBulletPos.Add(PlayerPos);
            }


            float elapsed = (float)gameTime.ElapsedGameTime.TotalSeconds;
            EnTimer -= elapsed;
            if (EnTimer < 0)
            {
                EnTimer = ResertTimer;
                EnemyBulletPos.Add(InvaderPos);
            }


            for (int i = 0; i < PlayerBulletPos.Count; i++)
            {
                PlayerBulletPos[i] = PlayerBulletPos[i] - new Vector2(0,3);
            }
            foreach(Vector2 bullet in PlayerBulletPos)
            for (int r = 0; r < rows; r++) 
                for (int c = 0; c < cols; c++)
                       if (rectinvader[r,c].Contains(bullet))
                        {
                            rectinvader[r,c].Y = -100000;
                            //PlayerBulletPos.Y = 10000;
                            score += 1;
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


                if(rectinvader[r,c].Y > 100) // Om Invadernars bullet träffar spelarens bullet
                {
                    Exit();
                }



            // Tab bort objekt säkert
            RemoveObjects();



            kOldState = kNewState;

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            spriteBatch.Begin();
          
            spriteBatch.Draw(Background, BackgroundPos, Color.White); // Allting som skrivs ut bakom denna bild kommer inte att kunna synas


            for (int r = 0; r < rows; r++) 
                for (int c = 0; c < cols; c++)
                    spriteBatch.Draw(Invader, rectinvader[r, c], Color.White); // Ritar ut invadrarna på både x och y axeln (r, c)

            //spriteBatch.Draw(Player, PlayerRec(), PlayerPos, Color.White);
            spriteBatch.Draw(Player, PlayerPos, Color.White);        
            

            foreach (Vector2 bulletPos in PlayerBulletPos)
            {
                Rectangle rec = new Rectangle();
                rec.Location = bulletPos.ToPoint();               
                rec.Size = new Point(20,15);
                spriteBatch.Draw(Bullet, rec, Color.White);
            }


            foreach (Vector2 bulletPos in EnemyBulletPos)
            {
                Rectangle rec = new Rectangle();
                rec.Location = bulletPos.ToPoint();
                rec.Size = new Point(30, 20);
                spriteBatch.Draw(Bullet, rec, Color.Red);
            }


            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }

        void RemoveObjects()
        {
            List<Vector2> temp = new List<Vector2>();
            foreach (var item in PlayerBulletPos) // Gör så att kulan försvinner vid miss
            {
                if (item.Y >= 5)
                {
                    temp.Add(item);
                }
            }

            PlayerBulletPos = temp;
        }
    }
}

