﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System;


namespace PRR1_19_Visning
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {


        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D Player, Player2, Player3, Invader, Invader2, Bullet, Background, Ufo, Pill; // Spelar, invader, bullet och backgrunds texture2d'n

        Vector2 BackgroundPos = new Vector2(0, 0);

        public static int score;
        SpriteFont ScoreFont;
        Vector2 ScorePosition;

        Rectangle RectBullet, EnRectBullet, UfoRectBullet, GoodPillRect, BadPillRect; // Här skapas rectanglarna som inte har några speciella förutbestämda värde

        int GSpeedMultiplier = 1;
        int BSpeedMultiplier = 1;

        float EnTimer = 3; // timer för fiendens bullet
        const float ResertTimer = 3; // Återställer tiden på timern

        double AnimationTimer = 1; // Timer för invadrarnas animation
        const double ResertAnimation = 1.5; // Återställer timern




        // Tiden som pillren kan tas efter att de spawnat (eller försöker ta dig)
        int GPillTimeToGet = SlowDown.Timetoclaim; // Står att min value inte får vara större än maxvalue men det stämmer inte att minvalue är störe än maxvalue
        int BPillTimeToGet = SpeedUp.Timetoclaim;


        // Timrar som bestämer när powerups spawnar under spelet, resert är utkommenterat då de ändå i nuläget bara är tänkt att spawna en gång då koden blir simplare
        float GPillSpawnTimer = SlowDown.SpawnTime;
        float BPillSpawnTimer = SpeedUp.SpawnTime;


        // Timrar som bestämer hur länge powerupsen är aktiva
        float BPillTimer = SpeedUp.Timeactive;
        float GPillTimer = SlowDown.Timeactive; // kan ksk flytta dessa timrar till Slowdowns och speedup klasserna för o göra koden mer lättläsligt


        // Dessa används för att identifiera om powerupsen är aktiva
        bool GoodPillActive = false;
        bool BadPillActive = false;


        // Enum för att välja olika karaktärer
        private enum Character
        {
            Normal,
            Rambo,
            Female,
        }

        private Character character;

        bool InvaderAnimation = false;


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

            BadPillRect.Y = 1000; // BadPillRect spawnas på 1000 så det inte kommer åka in i spelaren innan det "spawnar"
            GoodPillRect.Y = 450;

            BadPillRect.X = -1000; // Jag spawnar båda långt borta i början då de spawnar vid x = 0 om man inte anger något värde
            GoodPillRect.X = -1000;

            RectBullet.X = 1000; // Jag väljer att skriva såhär så de inte spawnar vid (0, 0) i början av spelet
            UfoRectBullet.X = 1000;

            character = Character.Normal; // Alltså är standard charactären i början Normal/Player
            InvaderAnimation = false;

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
            GoodPillRect.Width = Pill.Width / 12;
            GoodPillRect.Height = Pill.Height / 12;
            BadPillRect.Width = Pill.Width / 12;
            BadPillRect.Height = Pill.Height / 12;



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

            Background = Content.Load<Texture2D>("Background"); // Bakgrunden i spelet
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
                    // Ändrar positionen för spelaren samt håller spelaren inom ramen
                    if ((kNewState.IsKeyDown(Keys.Right)) && (PlayerRec[x, y].X < 700)) 
                        PlayerRec[x, y].X += 6;
                    if ((kNewState.IsKeyDown(Keys.Left)) && (PlayerRec[x, y].X > 0))
                        PlayerRec[x, y].X -= 6;

                    // Ennum för karaktärsbyte
                    if (kNewState.IsKeyDown(Keys.NumPad1)) 
                        character = Character.Normal;

                        if (kNewState.IsKeyDown(Keys.NumPad2))
                            character = Character.Rambo;

                            if (kNewState.IsKeyDown(Keys.NumPad3))
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
                        rectinvader[r, c].X += (2 * BSpeedMultiplier)/GSpeedMultiplier;
                    if (direction.Equals("Left"))
                        rectinvader[r, c].X -= (2 * BSpeedMultiplier)/GSpeedMultiplier;
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
                        UfoIsHit = false;
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


            float elapsed3 = (float)gameTime.ElapsedGameTime.TotalSeconds;
            GPillSpawnTimer -= elapsed3; 
            BPillSpawnTimer -= elapsed3;


            // Om tiden det är kvar tills att spawna är mindre än 0 och större än tiden pillret ska synas i (dock med ett minus framför) "spawna pillret" annars ta bort pillret
            if (GPillSpawnTimer < 0 && GPillSpawnTimer > SlowDown.Timetoclaim && GoodPillActive == false)
            {
                GoodPillRect.X = SpeedUp.Powerpos; 
            }
            else
            {
                GoodPillRect.X = 1000;
            }


            if (BPillSpawnTimer < 0 && BPillSpawnTimer > SpeedUp.Timetoclaim && BadPillActive == false)
            {
                BadPillRect.X = SlowDown.Powerpos;
                BadPillRect.Y = 450;
            }

            else
            {
                BadPillRect.Y = 1000;
                BadPillRect.X = 1000;  
            }


            // Om spelaren kolliderar med BadPill
            for (int x = 0; x < PlayerXPos; x++)
                for (int y = 0; y < PlayerYPos; y++)
                    if (PlayerRec[x, y].Contains(BadPillRect))
                    {
                        BadPillRect.Y = 1000;
                        BadPillActive = true;
                    }


            // Om spelaren kolliderar med GoodPill
            for (int x = 0; x < PlayerXPos; x++)
                for (int y = 0; y < PlayerYPos; y++)
                    if (PlayerRec[x, y].Contains(GoodPillRect))
                    {
                        GoodPillRect.X = 1000;
                        GoodPillActive = true;
                    }


            // Bestämer hur länge pillereffekten är aktiv
            if (SlowDown.SpawnTime - SlowDown.Timetoclaim + SlowDown.Timetoclaim < elapsed3)
            {
                GoodPillActive = false;
            }

            if (SpeedUp.SpawnTime - SpeedUp.Timetoclaim + SpeedUp.Timetoclaim < elapsed3)
            {
                BadPillActive = false;
            }


            if (GoodPillActive == true)
            {
                GSpeedMultiplier = SlowDown.EffectStrenght;
            }

            else
            {
                GSpeedMultiplier = 1;
            }


            if (BadPillActive == true)
            {
                BSpeedMultiplier = SpeedUp.EffectStrenght;
                BadPillRect.Y = 450;
            }

            else 
            {
                BSpeedMultiplier = 1;
                BadPillRect.Y = 1000;
            }


            // Flyttar BadPill mot spelaren
            for (int x = 0; x < PlayerXPos; x++)
                for (int y = 0; y < PlayerYPos; y++)
                    if (BadPillRect.X < PlayerRec[x, y].X && BadPillActive == true)
                    {
                        BadPillRect.X += 1;
                    }

            for (int x = 0; x < PlayerXPos; x++)
                for (int y = 0; y < PlayerYPos; y++)
                    if (BadPillRect.X > PlayerRec[x, y].X && BadPillActive == true)
                    {
                        BadPillRect.X -= 1; // Varför rör den sig så snabbt till spelaren då värdet bara ändras med -= 1 alt +=1?
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
                        if (rectinvader[r, c].Intersects(RectBullet)) 
                        {
                            rectinvader[r, c].Y = -10000;
                            RectBullet.Y += 1000;
                            IsHit = true;
                            score += 10;
                        }

                // Om playerbullet träffar invaderbullet
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
                        if (UfoRec[y, s].Intersects(RectBullet))
                        {
                            UfoRec[s, y].Y = -10000;
                            RectBullet.Y = 1000;
                            UfoIsHit = true;
                            score += 50;
                        }


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
            if (GoodPillActive == false)
            spriteBatch.Draw(Pill, BadPillRect, Color.Red);


            // Ritar ut GreenPill alltså Powerupen Speedup
            if (BadPillActive == false)
            spriteBatch.Draw(Pill, GoodPillRect, Color.Green);


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}

