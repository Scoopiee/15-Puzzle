using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public class Tile
{
    public int Number; //The number on the tile
    public Texture2D Sprite; //The sprite associated with this tile
    public Vector2 Position; //The position of the tile on the screen
    public const int tileSize = 128; //Size of each tile (width and height)
    public const int tileMargin = 5; //Margin between tiles
    public Tile(int number, Texture2D sprite) //Position is handled in draw method
    {
        Number = number;
        Sprite = sprite;
    }

    public void Draw(SpriteBatch spriteBatch, int x, int y, int tileSize, int tileMargin,int xOffset, int yOffset) 
    {
        // Calculate the position of the tile
        Vector2 position = new Vector2(x * (tileSize + tileMargin) + xOffset, y * (tileSize + tileMargin)+yOffset);

        // Draw the tile at the calculated position
        spriteBatch.Draw(Sprite, position, Color.White);    
    }
}

