using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Tile
{
    public class Tile
    {
        private Texture2D sprite;
        public Vector2 Position { get; private set; }
        public int Value {  get; private set; }


        //Constructor
        public Tile(Texture2D tileSprite, Vector2 position, int value)
        {
            sprite = tileSprite;
            Position = position;
            Value = value;
        }
        
        public void Draw(SpriteBatch spriteBatch) 
        {
            spriteBatch.Draw(sprite, Position, Color.White);
        
        }
    
    }
}