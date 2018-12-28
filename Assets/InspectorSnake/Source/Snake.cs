using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;

public class Snake
{
    public delegate void EnableScoreEntry(Snake.Direction dir);
    public static event EnableScoreEntry OnMovementInput;

    public List<Vector2Int> snakeBlocksPositions;
    public Vector2Int mapSize = new Vector2Int(20, 20);
    public Vector2Int looseBlock = new Vector2Int(2, 7);

    private const int toggleBoxWidth = 13;
    public bool movementDisabled;

    public Direction movementDirection;

    private HighScore highScore;

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    };

    //Constructor
    public Snake()
    {
        Start();
    }

    //Runs at start
    void Start()
    {
        highScore = new HighScore();

        snakeBlocksPositions = new List<Vector2Int>();
        SetInitialPosition();
    }

    //Sets the initial block positions to near the center
    public void SetInitialPosition()
    {
        snakeBlocksPositions.Add(new Vector2Int(10, 10));
        snakeBlocksPositions.Add(new Vector2Int(9, 10));
        snakeBlocksPositions.Add(new Vector2Int(8, 10));
        snakeBlocksPositions.Add(new Vector2Int(7, 10));
    }

    //Moves all of the blocks of the snake
    public void Move(Direction dir)
    {
        if (!IsPositionOccupied(GetBlockPlusDirectionPosition(snakeBlocksPositions[0], movementDirection)) && !movementDisabled)
        {
            for (int i = snakeBlocksPositions.Count - 1; i >= 0; i--)
            {
                snakeBlocksPositions[i] = (i != 0) ? snakeBlocksPositions[i - 1] : GetBlockPlusDirectionPosition(snakeBlocksPositions[i], movementDirection);
            }
        }
        else
        {
            GameOver();
        }

        CheckForBlockPickup();
    }

    //Returns whether or not a given position is occupied
    public bool IsPositionOccupied(Vector2Int v)
    {
        for (int i = 0; i < snakeBlocksPositions.Count - 1; i++)
        {
            if (snakeBlocksPositions[i] == v)
                return true;
        }

        return IsEdgeOfMap(v);
    }

    //Returns whether or not a given position is along the edge of the map
    private bool IsEdgeOfMap(Vector2Int v)
    {
        return v.x < 0 || v.x == mapSize.x - 1 || v.y < 0 || v.y == mapSize.y - 1;
    }

    //Returns a given direction from a position
    private Vector2Int GetBlockPlusDirectionPosition(Vector2Int v, Direction dir)
    {
        switch (dir)
        {
            case Direction.Down:
                return new Vector2Int(v.x, v.y + 1);

            case Direction.Up:
                return new Vector2Int(v.x, v.y - 1);

            case Direction.Right:
                return new Vector2Int(v.x + 1, v.y);

            case Direction.Left:
                return new Vector2Int(v.x - 1, v.y);

            default:
                return GetBlockPlusDirectionPosition(v, movementDirection);
        }
    }

    //Generates a new loose block, checking that it's a valid position
    void GenerateNewBlock()
    {
        Vector2Int proposedPosition = new Vector2Int(Random.Range(0, mapSize.x - 1), Random.Range(0, mapSize.y - 1));

        for (int i = 0; i < snakeBlocksPositions.Count - 1; i++)
        {
            if (snakeBlocksPositions[i] == proposedPosition)
            {
                GenerateNewBlock();
                break;
            }
        }

        looseBlock = proposedPosition;
    }

    //Checks whether or not the loose block has been picked up
    void CheckForBlockPickup()
    {
        if (snakeBlocksPositions[0] == looseBlock)
        {
            PickupBlock();
        }
    }

    //Adds the loose block to the snake
    void PickupBlock()
    {
        GenerateNewBlock();

        snakeBlocksPositions.Add(new Vector2Int(snakeBlocksPositions[snakeBlocksPositions.Count - 1].x,
            snakeBlocksPositions[snakeBlocksPositions.Count - 1].y));
    }

    //Runs when the game ends
    void GameOver()
    {
        //Show highscore on GUI
        //if highscore > old highscore, prompt to enter new one
        movementDisabled = true;
    }
}
