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
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteFont font;
        Texture2D backgroundTexture, bgMap, player, up, down, left, right, bomb;
        //Rectangle[] blox = new Rectangle[240];
        //List<Rectangle> btnRects = new List<Rectangle>();
        //float speed = 0.05f;
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
            //up = Content.Load<Texture2D>("up");
            //down = Content.Load<Texture2D>("up");
            //left = Content.Load<Texture2D>("up");
            //right = Content.Load<Texture2D>("up");
            bomb = Content.Load<Texture2D>("bomb");
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
            // int counter = 0;
            // for (int i = 0; i < bgMap.Height; i++)
            // {    
            //     //int min = step * i;
            //     //int max = step * (i+1);
            //     for (int c = 0; c < bgMap.Width; c++)
            //     {
            //         blox[counter] = new Rectangle(96*c, 96*i , 96, 96);
            //         //rectz.Add(blox[counter]);
            //         counter++;
            //     }  
            // } 
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
            playerPos.X += (step*direction);
            xCounter += (step * direction);
            }else
            {
            playerPos.Y += (step * direction);
            yCounter += (step * direction);
            }
        } 
      
            
        

        public void HandleInput()
        {                                                                                     //HANDLE INPUT 
                    keybState = Keyboard.GetState();
                    resX = (int)(xCounter / 96);
                    resY = (int)(yCounter / 96);
                    currentBlock = ((resY * 20) + resX) + 21;
             
                    //up 
                    if (keybState.IsKeyDown(Keys.Up))
                    {
                        if  ((xCounter % 96 == 0 && (rawData[currentBlock-20].B == 255 || yCounter % 96 != 0)) ||
                            (xCounter % 96 != 0 && rawData[currentBlock - 20].B == 255 && rawData[currentBlock - 19].B == 255)) 
                            move("y", -1);        
                    }
                    //d 
                    if (keybState.IsKeyDown(Keys.Down))
                    {
                        if ((xCounter % 96 == 0 && rawData[currentBlock + 20].B == 255) || 
                           (xCounter % 96 != 0 && rawData[currentBlock + 20].B == 255 && rawData[currentBlock + 21].B == 255))
                            move("y", 1); 
                    }
                    //l  
                    if (keybState.IsKeyDown(Keys.Left))
                    {
                        if ((yCounter % 96 == 0 && (rawData[currentBlock - 1].B == 255 || xCounter % 96 != 0)) ||
                           (yCounter % 96 != 0 && rawData[currentBlock - 1].B == 255 && rawData[currentBlock + 19].B == 255))
                            move("x", -1);
                    }
                    //r 
                    if (keybState.IsKeyDown(Keys.Right))
                    {
                        if ((yCounter % 96 == 0 && rawData[currentBlock + 1].B == 255) ||
                           (yCounter % 96 != 0 && rawData[currentBlock + 1].B == 255 && rawData[currentBlock + 21].B == 255))
                           move("x", 1);
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

            string lol = "o";
            string current = currentBlock + " xCounter: " + xCounter + " yCounter: " + yCounter;


            // spriteBatch.DrawString(font, DebugString, new Vector2(1222, 444), Color.Orange);

            //  string closesd = "closest " + closest.ToString();
            spriteBatch.DrawString(font, current, new Vector2(455, 455), Color.Red);


            //for (int x = 0; x < 240; x++)
            //             {
            //                 spriteBatch.Draw(up, blox[x], rawData[x]);
            //             }
            spriteBatch.DrawString(font, lol, new Vector2(355, 355), Color.Orange);

            if (keybState.IsKeyDown(Keys.X))
            {
                Vector2 xer = new Vector2((resX*96+96),(resY*96+96));
                spriteBatch.Draw(bomb, xer, Color.AliceBlue);
            } 


            spriteBatch.End();
            base.Draw(gameTime);
        }


        void ProcessKeyboard()
        {
            keybState = Keyboard.GetState();
            if (keybState.IsKeyDown(Keys.Escape))
                Exit();

           
        }

         

    }
}
