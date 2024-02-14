using System;
using System.Diagnostics;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Drawing;
using System.Runtime.InteropServices;

namespace TileGame
{
    public class Game1 : Game
    {        
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        
        //Game Window Variables
        private const int windowHeight = 712;
        private const int windowWidth = 587;
        
        //Game Components
        private GameBoard gameBoard;
        private Tile[] tiles;
        private int moveCount= 0;
        private TimeSpan timer;
        private bool gameSolved = false;

        //Keyboard Variables     
        KeyboardState currentKeyboardState;
        KeyboardState previousKeyboardState;

        //Textures, sounds and fonts
        private Texture2D titleBoard;
        private SoundEffect winSound;
        private Texture2D winBackdrop;
        private SpriteFont font;
        private SoundEffect clickSound;
        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize() //Called once
        {
            Window.Title = "15 Puzzle"; //Updates window title
            _graphics.PreferredBackBufferWidth = windowWidth;  //Set Height and Width in pixels using constant variable 
            _graphics.PreferredBackBufferHeight = windowHeight; 
            _graphics.ApplyChanges();

            timer = TimeSpan.Zero; //Initialize the timer and create the gameboard
           
            base.Initialize();
        }

        protected override void LoadContent() //Called once 
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            titleBoard = Content.Load<Texture2D>("TitleCard");
            winBackdrop = Content.Load<Texture2D>("WinBackdrop");
            font = Content.Load<SpriteFont>("ArialFont");
            winSound = Content.Load<SoundEffect>("WinSound");
            clickSound = Content.Load<SoundEffect>("Click");

            Texture2D[] tileSprites = new Texture2D[16]; //Loads the tile textures and assigns their sprites with an array of the sprite textures
            tiles = new Tile[16];
            
            for (int i = 0; i < 16; i++)
            {
                tileSprites[i] = Content.Load<Texture2D>($"tile_{i}"); //Uses string interpolation to load each tile "tile_0", "tile_1", etc... (thank you W3schools) 
                tiles[i] = new Tile(i, tileSprites[i]);
            }

            gameBoard = new GameBoard(tiles);
        }

        protected override void Update(GameTime gameTime) //Called every frame
        {

            previousKeyboardState = currentKeyboardState;
            currentKeyboardState = Keyboard.GetState();

            if (gameBoard.IsSolved() && !gameSolved) //This will only run once as once this triggers the gameSolved flag and therefore cant be ran again until reset
            {
                gameSolved = true;
                winSound.Play();
            }

            if (gameSolved) //Runs when the game is solved
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Space))
                {
                    ResetGame();
                }
            }
            
            if (!gameSolved) //Game Movement, every move is validated with the gameboard IsValidMove method, only runs when game isnt solved
            {
                timer += gameTime.ElapsedGameTime; //Increment timer

                if (IsKeyPressed(Keys.Up) && gameBoard.IsValidMove(Direction.Up))
                {
                    PlayClickSound();
                    gameBoard.MoveTile(Direction.Up);
                    moveCount++;
                }
                if (IsKeyPressed(Keys.Down) && gameBoard.IsValidMove(Direction.Down))
                {
                    PlayClickSound();
                    gameBoard.MoveTile(Direction.Down);
                    moveCount++;
                }
                if (IsKeyPressed(Keys.Left) && gameBoard.IsValidMove(Direction.Left))
                {
                    PlayClickSound();
                    gameBoard.MoveTile(Direction.Left);
                    moveCount++;
                }
                if (IsKeyPressed(Keys.Right) && gameBoard.IsValidMove(Direction.Right))
                {
                    PlayClickSound();
                    gameBoard.MoveTile(Direction.Right);
                    moveCount++;
                }
            }
            
            if (IsKeyPressed(Keys.Escape)) //Exit Game
            {
                Exit();
            }
 
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime) //Called every frame after update
        {
            GraphicsDevice.Clear(Microsoft.Xna.Framework.Color.DarkGray); //Sets background of the game

            if (gameSolved) //Win condition game rendering
            {
                DrawWinScreen();
                DrawTitleCard();
                DrawMoveCount();
                DrawTimer();
            }
            else //Normal game rendering
            {
                DrawTitleCard();
                DrawTiles();
                DrawMoveCount();
                DrawTimer();
            }

            base.Draw(gameTime);
        }


        private void DrawTiles() //Handles drawing of the tiles, utilises the draw method of the tile class which also handles updating tile position
        {
            _spriteBatch.Begin();

            const int yOffset = 155; //Offset for the gameboard grid
            const int xOffset = 30;

            for (int i = 0; i < GameBoard.size; i++) 
            {
                for (int j = 0; j < GameBoard.size; j++)
                {
                    Tile tile = gameBoard.GetTile(i, j); //Loops through each tile in the gameboard and draws it using tile.Draw method
                    tile.Draw(_spriteBatch, j, i, Tile.tileSize, Tile.tileMargin, xOffset, yOffset);
                }
            }

            _spriteBatch.End();
        }

        private void DrawTitleCard() //Handles the drawing of the title card
        {
            _spriteBatch.Begin();

            const int titleY = 30; //Position of title card
            const int titleX = 30;

            _spriteBatch.Draw(titleBoard, new Vector2(titleY, titleX), Microsoft.Xna.Framework.Color.White);

            _spriteBatch.End();


        }

        private void DrawTimer()//Handles drawing of the timer
        {
            _spriteBatch.Begin();

            const int timerY = 95;
            const int timerX = 45;
            string timerText = $"Time: {timer.Minutes:D2}:{timer.Seconds:D2}"; // D2 - 2 Digit decimal number
           
            _spriteBatch.DrawString(font, timerText, new Vector2(timerX, timerY), Microsoft.Xna.Framework.Color.Black);

            _spriteBatch.End();
        }

        private void DrawMoveCount() //Handles drawing of the move counter
        {
            _spriteBatch.Begin();

            string moveText = $"Moves: {moveCount}";
            Vector2 textSize = font.MeasureString(moveText);
            int textWidth = (int)textSize.X; //Calculates the text
            const int moveCountY = 95;
            int moveCountX = 542 - textWidth;

            _spriteBatch.DrawString(font, moveText, new Vector2(moveCountX, moveCountY), Microsoft.Xna.Framework.Color.Black); // Adjust the position as needed

            _spriteBatch.End();
        }

        private void DrawWinScreen() //Handles drawing of the win screen
        {
            _spriteBatch.Begin();

            const int winBackdropY = 155;
            const int winBackdropX = 30;

            _spriteBatch.Draw(winBackdrop, new Vector2(winBackdropX, winBackdropY), Microsoft.Xna.Framework.Color.White); //Background for the win screen

            string winText = "          Congratulations\nPress spacebar to play again!";
            Vector2 textSize = font.MeasureString(winText);
            Vector2 position = new Vector2((GraphicsDevice.Viewport.Width - textSize.X) / 2, (GraphicsDevice.Viewport.Height - textSize.Y) / 2); //Used to centre the message in the window 

            _spriteBatch.DrawString(font, winText, position, Microsoft.Xna.Framework.Color.Black); //Text for the win screen

            _spriteBatch.End();
        }

        private void PlayClickSound()
        {
            Random random = new Random();
            float pitch = (float)(random.NextDouble() * 0.4 - 0.2); // Random pitch between -0.2 and +0.2
            clickSound.Play(1.0f, pitch, 0.0f);
        }
        private bool IsKeyPressed(Keys key) //Method to debounce input so multiple keypresses are not recorded
        {
            return currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key);
        }
        private void ResetGame() //Resets the game variables and reshuffles board once the game is won
        {
            //Re-Shuffle the board
            gameBoard.ShuffleBoard(gameBoard.shuffleMoveCount);    

            // Reset game-related variables
            gameSolved = false;
            timer = TimeSpan.Zero;       
            moveCount = 0;              
        }

    }
}