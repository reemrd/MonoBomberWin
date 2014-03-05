#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Storage;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
#endregion

namespace WinGame
{
    /// <summary>
    /// This is the main type for your game
    /// 
    /// COLOR CODES:
    /// R   - SELECTED CHANNEL
    /// 0   - BLACK, DESTROYABLE BLOCK
    /// 100 - GREEN, OUTER BLOCKS, NON DESTROYABLE
    /// 255 - WHITE, WALKABLE, DESTROYABLE
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D backgroundTexture, bgMap, player, bomb, explode, block;
        Rectangle[] blox = new Rectangle[240];
        //List<Rectangle> btnRects = new List<Rectangle>();
        //float speed = 0.05f;

        //TILE SIZE in pixels - THE FOUNDATION OF EVERYYTHIIING
        int ts = 96;

        Vector2 playerPos = new Vector2(96, 96);
        //List<Rectangle> rectz = new List<Rectangle>();
        Color[] rawData;
        string statuss = "default";
        int yCounter, xCounter;
        Point p;
        string DebugString = "lala";
        //string[] axisNames = new string[10] { "right", "right", "downright", "down", "downleft", "left", "left", "upleft", "top", "upright" };
        int step = 3;
        //will vary depending on selected player position
        int currentBlock = 21;
        int resY, resX;

        KeyboardState keybState = Keyboard.GetState();
        Vector2 bombPosition;
        int bombCountdown = 0;
        int bombBlock;


        public Game1()
            : base()
        {
            graphics = new GraphicsDeviceManager(this);

            graphics.PreferredBackBufferWidth = 1400;
            graphics.PreferredBackBufferHeight = 800;

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

            font = Content.Load<SpriteFont>("spriteFont1");
            backgroundTexture = Content.Load<Texture2D>("bg");
            player = Content.Load<Texture2D>("player");
            bgMap = Content.Load<Texture2D>("bgMap");
            bomb = Content.Load<Texture2D>("bomb");
            explode = Content.Load<Texture2D>("explode");
            block = Content.Load<Texture2D>("block");

            CollisionMap();
        }


        public void CollisionMap()                                                                                                //COLLISION MAP
        {
            //Filling pixel map color array
            rawData = new Color[bgMap.Width * bgMap.Height];
            bgMap.GetData<Color>(rawData);
            yCounter = 0;
            xCounter = 0;

            ////x range - is wider hence we can ommit counting the y array as long as гена хуй we know the lenght of y.. 
            //THE PIECE OF CODE BELOW CAN BE USED TO VISUALIZE THE HUJ BG MAPPING FOR ERROR DETECTION PURPOSES 
            int counter = 0;
            for (int i = 0; i < bgMap.Height; i++)
            {
                //int min = step * i;
                //int max = step * (i+1);
                for (int c = 0; c < bgMap.Width; c++)
                {
                    blox[counter] = new Rectangle(96 * c, 96 * i, 96, 96);
                    //rectz.Add(blox[counter]);
                    counter++;
                }
            }
        }
        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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

            ProcessKeyboard();

            HandleInput();


            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        void move(string axis, int direction)
        {
            if (axis == "x")
            {
                playerPos.X += (step * direction);
                xCounter += (step * direction);
            }
            else
            {
                playerPos.Y += (step * direction);
                yCounter += (step * direction);
            }
        }




        public void HandleInput()
        {                                                                                     //HANDLE INPUT 
            keybState = Keyboard.GetState();
            if (keybState.GetPressedKeys().Length > 0)
            {
                resX = (int)((xCounter + 48) / ts);
                resY = (int)((yCounter + 48) / ts);
                currentBlock = ((resY * 20) + resX) + 21;
                string jujjdf = keybState.GetPressedKeys().ToString();

                 
                //up 
                if (keybState.IsKeyDown(Keys.Up))
                {
                    if ((xCounter % ts == 0 && (rawData[currentBlock - 20].R == 255 || yCounter % ts != 0)) ||
                        (xCounter % ts != 0 && rawData[currentBlock - 20].R == 255 && rawData[currentBlock - 19].R == 255))
                        move("y", -1);
                }
                //d 
                if (keybState.IsKeyDown(Keys.Down))
                {
                    if ((xCounter % ts == 0 && rawData[currentBlock + 20].R == 255) ||
                       (xCounter % ts != 0 && rawData[currentBlock + 20].R == 255 && rawData[currentBlock + 21].R == 255))
                        move("y", 1);
                }
                //l  
                if (keybState.IsKeyDown(Keys.Left))
                {
                    if ((yCounter % ts == 0 && (rawData[currentBlock - 1].R == 255 || xCounter % ts != 0)) ||
                       (yCounter % ts != 0 && rawData[currentBlock - 1].R == 255 && rawData[currentBlock + 19].R == 255))
                        move("x", -1);
                }
                //r 
                if (keybState.IsKeyDown(Keys.Right))
                {
                    if ((yCounter % ts == 0 && rawData[currentBlock + 1].R == 255) ||
                       (yCounter % ts != 0 && rawData[currentBlock + 1].R == 255 && rawData[currentBlock + 21].R == 255))
                        move("x", 1);
                }
            }
 
        }



        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            graphics.GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();

            Vector2 bgMiddle = new Vector2(backgroundTexture.Width / 3, backgroundTexture.Height / 3);

            //var arrowMid = new Vector2(up.Width / 2, up.Height / 2);
            spriteBatch.Draw(backgroundTexture, new Vector2(0, 0), null, Color.White, 0, new Vector2(0, 0), 1.0f, SpriteEffects.None, 0f);
            spriteBatch.Draw(player, playerPos, null, Color.Red, 0, new Vector2(0, 0), 1f, SpriteEffects.None, 0f);

            //string lol = "o";
            string current = currentBlock + " xCounter: " + xCounter + " yCounter: " + yCounter;


            // spriteBatch.DrawString(font, DebugString, new Vector2(1222, 444), Color.Orange);

            //  string closesd = "closest " + closest.ToString();



            for (int x = 0; x < 240; x++)
            {
                if (rawData[x].R == 0 && rawData[x].G != 255)
                {
                    spriteBatch.Draw(block, blox[x], Color.White);
                }
                else if (rawData[x].R == 100)
                    spriteBatch.Draw(block, blox[x], Color.Red);
            }
            //spriteBatch.DrawString(font, lol, new Vector2(355, 355), Color.Orange);
           spriteBatch.DrawString(font, current, new Vector2(455, 455), Color.Red);

            DrawBomb();



            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawBomb()
        {
            if (keybState.IsKeyDown(Keys.X))
            {
                if (bombCountdown == 0)
                {
                    int x = ts;
                    int y = ts;
                    bombCountdown = 120;
                    if (xCounter % ts > 48)
                    {
                        x = ts * 2;
                        bombBlock = currentBlock + 1;
                    }

                    else if (yCounter % ts > 48)
                    {
                        bombBlock = currentBlock + 21;
                        y = ts * 2;
                    }
                    else
                        bombBlock = currentBlock;


                    bombPosition = new Vector2((resX * ts + x), (resY * ts + y));

                }
            }

            if (bombCountdown > 0)
            {
                spriteBatch.Draw(bomb, bombPosition, Color.AliceBlue);
                bombCountdown--;

                if (bombCountdown > 0 && bombCountdown < 33)
                {
                    if (rawData[bombBlock + 1].R != 100)
                    { //RIGHT
                        spriteBatch.Draw(explode, new Vector2((bombPosition.X + ts), (bombPosition.Y)), Color.White);
                        if (bombCountdown == 1)
                            rawData[bombBlock + 1].R = 255;
                    }

                    if (rawData[bombBlock - 1].R != 100)
                    { //LEFT
                        spriteBatch.Draw(explode, new Vector2((bombPosition.X - ts), (bombPosition.Y)), Color.White);
                        if (bombCountdown == 1)
                            rawData[bombBlock - 1].R = 255;
                    }

                    if (rawData[bombBlock + 20].R != 100)
                    { //BOTTOM
                        spriteBatch.Draw(explode, new Vector2((bombPosition.X), (bombPosition.Y + ts)), Color.White);
                        if (bombCountdown == 1)
                            rawData[bombBlock + 20].R = 255;
                    }

                    if (rawData[bombBlock - 20].R != 100)
                    { //TOP
                        spriteBatch.Draw(explode, new Vector2((bombPosition.X), (bombPosition.Y - ts)), Color.White);
                        if (bombCountdown == 1)
                            rawData[bombBlock - 20].R = 255;
                    }

                }
            }
        }




        void ProcessKeyboard()
        {
            keybState = Keyboard.GetState();
            if (keybState.IsKeyDown(Keys.Escape))
                Exit();


        }



    }
}
