using System;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public enum Direction //Enum to keep track of directions (thank you again W3schools)
{
    Up,
    Down,
    Left,
    Right
}
public class GameBoard
{
    public Tile[,] board;
    public const int size = 4;
    public int shuffleMoveCount = 500;

    public GameBoard(Tile[] tiles)
    {
        InitializeBoard(tiles);
    }

    public void InitializeBoard(Tile[] tiles)
    {
        board = new Tile[size, size]; //Create the board array 

        int num = 1; //Start with 1 so that the empty tile isn't placed on the board until the end
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (num == size * size)
                {
                    board[i, j] = tiles[0]; //Place the blank tile in the last position
                }
                else
                {
                    board[i, j] = tiles[num++]; 
                }
            }
        }
        ShuffleBoard(shuffleMoveCount);
    }

    public void ShuffleBoard(int moveCount)
    {
        Random rand = new Random();

        for (int i = 0; i < moveCount; i++)
        {
            Direction randomDirection = GetRandomDirection(rand);
            if (IsValidMove(randomDirection))
            {
                MoveTile(randomDirection);
            }
            else
            {
                i--; // If the move isn't valid, repeat the iteration
            }
        }
    }

    private Direction GetRandomDirection(Random rand) //This exists to make the shuffleboard method more readable, gets a random direction 
    {
        Direction[] directions = { Direction.Up, Direction.Down, Direction.Left, Direction.Right };
        return directions[rand.Next(directions.Length)];
    }

    public bool IsSolved() //Check if the board is solved 
    {
        int num = 1;
        
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                
                if (!(i == size - 1 && j == size - 1)) //Skip the last tile for now
                {
                    if (board[i, j].Number != num)
                    {
                        return false;
                    }
                    num++;
                }
            }
        }
        return board[size - 1, size - 1].Number == 0; //Check if the last tile is the blank tile as the number on the blank tile is 0 
    }

    public Tile GetTile(int row, int col) //Useful little method to grab tile from board 
    {
        return board[row, col];
    }

    public bool IsValidMove(Direction direction) //Checks if a move is valid based onthe location of the empty tile
    {

        Point emptyTilePos = FindEmptyTilePosition();
        Point adjacentTilePos = emptyTilePos;
       
        switch (direction)
        {
            case Direction.Up:
                adjacentTilePos.Y += 1;
                break;
            case Direction.Down:
                adjacentTilePos.Y -= 1;
                break;
            case Direction.Left:
                adjacentTilePos.X += 1;
                break;
            case Direction.Right:
                adjacentTilePos.X -= 1;
                break;
        }
        return adjacentTilePos.X >= 0 && adjacentTilePos.X < size && adjacentTilePos.Y >= 0 && adjacentTilePos.Y < size; //Check if the adjacent position is within the bounds of the array 
    }

    public Point FindEmptyTilePosition() //Finds the empty tile position to be used in the move tiles method 
    {
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (board[i, j].Number == 0) // 0 is the empty tile number 
                {
                    return new Point(j, i);
                }
            }
        }
        throw new InvalidOperationException("No empty tile found on the board."); //Should never be called 
    }

    public void MoveTile(Direction direction) //Moves the tiles by swapping them in the gameboard array, position updating is handled in the Tile.draw method 
    {
        Point emptyTilePos = FindEmptyTilePosition();
        Point tileToMovePos = emptyTilePos;
        
        switch (direction)
        {
            case Direction.Up:
                tileToMovePos.Y += 1;
                break;
            case Direction.Down:
                tileToMovePos.Y -= 1;
                break;
            case Direction.Left:
                tileToMovePos.X += 1;
                break;
            case Direction.Right:
                tileToMovePos.X -= 1;
                break;
        }
        Tile temp = board[emptyTilePos.Y, emptyTilePos.X]; //Swapping the tiles
        board[emptyTilePos.Y, emptyTilePos.X] = board[tileToMovePos.Y, tileToMovePos.X];
        board[tileToMovePos.Y, tileToMovePos.X] = temp;
    }
}