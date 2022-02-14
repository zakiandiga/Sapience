using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Fungus;

public class Snake : MonoBehaviour
{
    private enum Direction //The movement states of the snake
    {
        Left,
        Right,
        Up,
        Down
    }

    private enum State //The alive or dead states for the snake
    {
        Alive,
        Dead
    }

    private State state;
    private Direction gridMoveDirection;
    private Vector2 gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<SnakeMovePosition> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;

    public Flowchart flowchart;
    public static bool isDead;

    [SerializeField] private InputActionReference changeDirection;

    public void Setup(LevelGrid levelGrid)
    {
        this.levelGrid = levelGrid;
    }
    
    private void Start()
    {
        changeDirection.action.Enable();
    }

    private void Awake()
    {
        gridPosition = new Vector2(10, 10);
        gridMoveTimerMax = .3f; //Change speed of snake using this variable
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = Direction.Right;

        snakeMovePositionList = new List<SnakeMovePosition>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>();

        state = State.Dead; //Snake starts dead until Fungus gameStarted bool commands it to switch to alive
        isDead = false;
    }
    public Vector2 ChangePosition
    {
        get
        {
            return changeDirection.action.ReadValue<Vector2>();
        }
    }

    private void OnDisable()
    {
        changeDirection.action.Disable();
    }

    private void Update()
    {
        if (flowchart.GetBooleanVariable("gameStarted") == true) //Starts the snake upon the completion of the Fungus flowchart
        {
            state = State.Alive;
        }

        switch (state) {
            case State.Alive:
                HandleInput();
                HandleGridMovement();
                break;
            case State.Dead:
                isDead = true;
                break;
        }
    }

    private void HandleInput()
    {
        if (ChangePosition.y > 0) //Up
        {
            if (gridMoveDirection != Direction.Down)
            {
                gridMoveDirection = Direction.Up;
            }

        }

        if (ChangePosition.y < 0) //Down
        {
            if (gridMoveDirection != Direction.Up)
            {
                gridMoveDirection = Direction.Down;
            }
        }

        if (ChangePosition.x < 0) //Left
        {
            if (gridMoveDirection != Direction.Right)
            {
                gridMoveDirection = Direction.Left;
            }

        }

        if (ChangePosition.x > 0) //Right
        {
            if (gridMoveDirection != Direction.Left)
            {
                gridMoveDirection = Direction.Right;
            }

        }
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime; //Constant Movement
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            SnakeMovePosition previousSnakeMovePosition = null;
            if (snakeMovePositionList.Count > 0)
            {
                previousSnakeMovePosition = snakeMovePositionList[0];
            }

            SnakeMovePosition snakeMovePosition = new SnakeMovePosition(previousSnakeMovePosition, gridPosition, gridMoveDirection);
            snakeMovePositionList.Insert(0, snakeMovePosition);

            Vector2 gridMoveDirectionVector;
            switch (gridMoveDirection) //Changes the direction of the snake
            {
                default:
                case Direction.Right: gridMoveDirectionVector = new Vector2(+1, 0); break;
                case Direction.Left: gridMoveDirectionVector = new Vector2(-1, 0); break;
                case Direction.Up: gridMoveDirectionVector = new Vector2(0, +1); break;
                case Direction.Down: gridMoveDirectionVector = new Vector2(0, -1); break;
            }

            gridPosition += gridMoveDirectionVector;

            bool snakeAteFood = levelGrid.TrySnakeEatFood(gridPosition);
            if (snakeAteFood)
            {
                //Snake ate food, grow body
                snakeBodySize++;
                CreateSnakeBodyPart();
            }

            if (snakeMovePositionList.Count >= snakeBodySize + 1)
            {
                snakeMovePositionList.RemoveAt(snakeMovePositionList.Count - 1);
            }

            UpdateSnakeBodyParts();

            foreach (SnakeBodyPart snakeBodyPart in snakeBodyPartList)
            {
                Vector2 snakeBodyPartGridPosition = snakeBodyPart.GetGridPosition();
                if (gridPosition == snakeBodyPartGridPosition) //Game over if snake runs into tail
                {
                    Debug.Log("Dead!");
                    state = State.Dead;
                }
            }

            if (gridPosition.x > 19 || gridPosition.x < 0) //game over if snake runs into side walls
            {
                Debug.Log("Dead!");
                state = State.Dead;
            }

            if (gridPosition.y > 19 || gridPosition.y < 0) //game over if snake runs into top/bottom walls
            {
                Debug.Log("Dead!");
                state = State.Dead;
            }

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirectionVector)); //Corrects snake head direction. Add -90 if new snake head sprite's front faces up rather than right
        }
    }

    private void CreateSnakeBodyPart()
    {
        snakeBodyPartList.Add(new SnakeBodyPart(snakeBodyPartList.Count));
    }

    private void UpdateSnakeBodyParts()
    {
        for (int i = 0; i < snakeBodyPartList.Count; i++)
        {
            snakeBodyPartList[i].SetSnakeMovePosition(snakeMovePositionList[i]);
        }
    }

    private float GetAngleFromVector(Vector2 dir)
    {
        float n = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if (n < 0) n += 360;
        return n;
    }

    public Vector2 GetGridPosition()
    {
        return gridPosition;
    }

    public List<Vector2> GetFullSnakeGridPositionList() //Return the full list of positions occupied by the snake: Head + Body
    {
        List<Vector2> gridPositionList = new List<Vector2>() { gridPosition };
        foreach (SnakeMovePosition snakeMovePosition in snakeMovePositionList)
        {
            gridPositionList.Add(snakeMovePosition.GetGridPosition());
        }
        return gridPositionList;
    }


    private class SnakeBodyPart //Constructor for snake body
    {
        private SnakeMovePosition snakeMovePosition;
        private Transform transform;
        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetSnakeMovePosition(SnakeMovePosition snakeMovePosition)
        {
            this.snakeMovePosition = snakeMovePosition;
            transform.position = new Vector3(snakeMovePosition.GetGridPosition().x, snakeMovePosition.GetGridPosition().y);

            float angle;
            switch (snakeMovePosition.GetDirection()) //Adds the angle to body segments that are changing direction
            {
                default:
                case Direction.Up: //Currently going Up
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 0; break;
                        case Direction.Left: //Previously was going Left
                            angle = 0 + 45; break;
                        case Direction.Right: //Previously was going Right
                            angle = 0 - 45; break;
                    }
                    break;
                case Direction.Down: //Currently going Down
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 180; break;
                        case Direction.Left: //Previously was going Left
                            angle = 180 + 45; break;
                        case Direction.Right: //Previously was going Right
                            angle = 180 - 45; break;
                    }
                    break;
                case Direction.Left: //Currently going to the Left
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = -90; break;
                        case Direction.Down: //Previously was going Down
                            angle = -45; break;
                        case Direction.Up: //Previously was going Up
                            angle = 45; break;
                    }
                    break;
                case Direction.Right: //Currently going to the Right
                    switch (snakeMovePosition.GetPreviousDirection())
                    {
                        default:
                            angle = 90; break;
                        case Direction.Down: //Previously was going Down
                            angle = 45; break;
                        case Direction.Up: //Previously was going Up
                            angle = -45; break;
                    }
                    break;
            }

            transform.eulerAngles = new Vector3(0, 0, angle);
        }

        public Vector2 GetGridPosition()
        {
            return snakeMovePosition.GetGridPosition();
        }
    }


    private class SnakeMovePosition //Handles one Move Position from the Snake
    {
        private SnakeMovePosition previousSnakeMovePosition;
        private Vector2 gridPosition;
        private Direction direction;

        public SnakeMovePosition(SnakeMovePosition previousSnakeMovePosition, Vector2 gridPosition, Direction direction)
        {
            this.previousSnakeMovePosition = previousSnakeMovePosition;
            this.gridPosition = gridPosition;
            this.direction = direction;
        }

        public Vector2 GetGridPosition()
        {
            return gridPosition;
        }

        public Direction GetDirection()
        {
            return direction;
        }

        public Direction GetPreviousDirection()
        {
            if (previousSnakeMovePosition == null)
            {
                return Direction.Right;
            } else
            {
                return previousSnakeMovePosition.direction;
            }
            
        }
    }
}
