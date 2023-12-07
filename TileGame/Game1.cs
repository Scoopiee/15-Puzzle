using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace TileGame
{
    public class Game1 : Game
    {

        Tile.Tile myTile;
        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        Texture2D footballTexture;
        Texture2D tennisballTexture;
        Texture2D myTileTexture;

        MouseState currentMouseState;
        MouseState previousMouseState;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //Add initialization logic 

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            footballTexture = Content.Load<Texture2D>("ball");
            tennisballTexture = Content.Load<Texture2D>("tennisball");
            myTileTexture = Content.Load<Texture2D>("demotile");
            

           
            //this.Content - load game content 
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
           
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Released &&
                previousMouseState.LeftButton == ButtonState.Pressed)
            {
                Point mousePosition = new Point(currentMouseState.X, currentMouseState.Y);

                // Check if the mouse is over the first image
                Rectangle image1Bounds = new Rectangle(100, 100, footballTexture.Width, footballTexture.Height);
                if (image1Bounds.Contains(mousePosition))
                {
                    // Action to perform when the first image is clicked
                    // Example: Change a variable's value, trigger an event, etc.
                    // Add your custom logic here
                    System.Diagnostics.Debug.WriteLine("Football Clicked!"); 
                }

                // Check if the mouse is over the second image
                Rectangle image2Bounds = new Rectangle(200, 100, tennisballTexture.Width, tennisballTexture.Height);
                if (image2Bounds.Contains(mousePosition))
                {
                    // Action to perform when the second image is clicked
                    // Example: Play a sound, transition to another screen, etc.
                    // Add your custom logic here
                    System.Diagnostics.Debug.WriteLine("Tennisball clicked!");
                }
                Rectangle image3bounds = new Rectangle(300, 100, myTileTexture.Width, myTileTexture.Height); 
                if (image3bounds.Contains(mousePosition))
                {


                    System.Diagnostics.Debug.WriteLine("Tile clicked!");
                }
            }
            // update logic

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();

            // Draw the sprite at a specific position (for example, at coordinates (100, 100))
            _spriteBatch.Draw(footballTexture, new Vector2(100, 100), Color.White);
            _spriteBatch.Draw(tennisballTexture, new Vector2(200, 100), Color.White);
            _spriteBatch.Draw(myTileTexture, new Vector2(300, 100), Color.White);

            _spriteBatch.End();


            // You'll draw these rectangles in the Draw method using _spriteBatch.DrawRectangle(...)

            // drawing code 

            base.Draw(gameTime);
        }
    }
}