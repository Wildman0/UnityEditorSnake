using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class EditorSnake : EditorWindow
{
    public static EditorSnake window;
    private Snake snake;

    private bool gamePaused = true;
    
    public delegate void MovementInput(Snake.Direction dir);
    public static event MovementInput OnMovementInput;
    
    private const int toggleBoxWidth = 13;
    private const int movementPanelOffset = 50;
    private const int verticalOffset = 20;
    private const int horizontalOffset = 20;

    private const int millisecondsBetweenTicks = 250;

    private const int buttonSideLong = 40;
    private const int buttonSideShort = 20;

    private DateTime lastTickTime;
    private TimeSpan timeToLastTickDifference;

    public bool enterHighScoreIsActive;

    [MenuItem("Snake/Snake")]
    public static void ShowEditorWindow()
    {
        Init();
    }
    
    //Runs on editor window destruction
    void OnDestroy()
    {
        EditorApplication.update -= Update;
        OnMovementInput -= UpdateMovementDirection;
    }
    
    //Called on window initialization
    static void Init()
    {
        window = (EditorSnake)EditorWindow.GetWindow(typeof(EditorSnake));
        window.Show();
    }

    //Runs at start
    void OnEnable()
    {
        snake = new Snake();
        gamePaused = true;
        AddEventSubscriptions();
    }

    //Adds all necessary event subscriptions
    void AddEventSubscriptions()
    {
        EditorApplication.update += Update;
        OnMovementInput += UpdateMovementDirection;
    }

    //Updates the current movement direction
    void UpdateMovementDirection(Snake.Direction dir)
    {
        if (!IsDirectionOpposite(dir, snake.movementDirection))
        {
            snake.movementDirection = dir;
        }
    }

    //Returns whether or not 2 directions are opposites
    bool IsDirectionOpposite(Snake.Direction dir1, Snake.Direction dir2)
    {
        return (Mathf.Abs((int) dir1 - (int) dir2) == 2);
    }

    //Runs on any GUI update
    void OnGUI()
    {
        DrawStartButton();
        DrawBorders();
        SetMovementButtonInputs();
        DrawSnakeBlocks(snake.snakeBlocksPositions);
        DrawLooseBlock();
    }

    //Draws start button and gets it's input
    void DrawStartButton()
    {
        if (GUI.Button(new Rect(295, 70, 100, 30), "Start Game"))
        {
            gamePaused = false;
        }
    }

    //Draws the borders of the map
    void DrawBorders()
    {
        DrawHorizontalBorders();
        DrawVerticalBorders();
    }

    //Draws the horizontal borders of the map
    void DrawHorizontalBorders()
    {
        for (int i = 0; i < snake.mapSize.x + 1; i++)
        {
            GUI.Toggle(new Rect(toggleBoxWidth * i, 0, 0, 0), true, "");
            GUI.Toggle(new Rect(toggleBoxWidth * i, snake.mapSize.y * toggleBoxWidth + 1, 0, 0), true, "");
        }
    }

    //Draws the vertical borders of the map
    void DrawVerticalBorders()
    {
        for (int i = 0; i < snake.mapSize.x + 1; i++)
        {
            GUI.Toggle(new Rect(0, toggleBoxWidth * i, 0, 0), true, ""); 
            GUI.Toggle(new Rect(snake.mapSize.y * toggleBoxWidth + 1, toggleBoxWidth * i, 0, 0), true, "");
        }
    }

    //Draws all snake blocks
    void DrawSnakeBlocks(List<Vector2Int> blockPositions)
    {
        for (int i = 0; i < blockPositions.Count; i++)
        {
            GUI.Toggle(new Rect((blockPositions[i].x + 1) * toggleBoxWidth, (blockPositions[i].y + 1) * toggleBoxWidth, 0, 0), snake.movementDisabled, "");
        }
    }

    //Draws the loose block
    void DrawLooseBlock()
    {
        GUI.Toggle(new Rect((snake.looseBlock.x + 1) * toggleBoxWidth, (snake.looseBlock.y + 1) * toggleBoxWidth, 0, 0), true, "");
    }

    //Draws the movement buttons
    void SetMovementButtonInputs()
    {
        Rect centerOfMovementButtons = new Rect((snake.mapSize.x + 1) * toggleBoxWidth + movementPanelOffset,
            snake.mapSize.y / 2 * toggleBoxWidth, 0, 0);

        if (GUI.Button(
            new Rect(centerOfMovementButtons.x, centerOfMovementButtons.y - verticalOffset, buttonSideLong,
                buttonSideShort), "^"))
        {
            OnMovementInput(Snake.Direction.Up);
        }

        if (GUI.Button(
            new Rect(centerOfMovementButtons.x, centerOfMovementButtons.y + verticalOffset + buttonSideShort,
                buttonSideLong, buttonSideShort), "v"))
        {
            OnMovementInput(Snake.Direction.Down);
        }

        if (GUI.Button(
            new Rect(centerOfMovementButtons.x + horizontalOffset + buttonSideShort, centerOfMovementButtons.y,
                buttonSideShort, buttonSideLong), ">"))
        {
            OnMovementInput(Snake.Direction.Right);
        }

        if (GUI.Button(
            new Rect(centerOfMovementButtons.x - horizontalOffset, centerOfMovementButtons.y, buttonSideShort,
                buttonSideLong), "<"))
        {
            OnMovementInput(Snake.Direction.Left);
        }
    }

    //Runs every frame (kinda)
    void Update()
    {
        if (DateTime.Now > lastTickTime + TimeSpan.FromMilliseconds(millisecondsBetweenTicks) && !gamePaused)
        {
            Tick();
            lastTickTime = DateTime.Now;
        }
    }

    //Runs as often as specified
    void Tick()
    {
        snake.Move(snake.movementDirection);

        window.Repaint();
    }
}
