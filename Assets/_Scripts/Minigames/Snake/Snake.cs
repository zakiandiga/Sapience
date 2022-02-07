using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Snake : MonoBehaviour
{
    private Vector2 gridMoveDirection;
    private Vector2 gridPosition;
    private float gridMoveTimer;
    private float gridMoveTimerMax;
    private LevelGrid levelGrid;
    private int snakeBodySize;
    private List<Vector2> snakeMovePositionList;
    private List<SnakeBodyPart> snakeBodyPartList;

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
        gridMoveTimerMax = .3f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2(1, 0);

        snakeMovePositionList = new List<Vector2>();
        snakeBodySize = 0;

        snakeBodyPartList = new List<SnakeBodyPart>();
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
        HandleInput();
        HandleGridMovement();
    }

    private void HandleInput()
    {
        if (ChangePosition.y > 0 && gridMoveDirection.y != 1) //Up
        {
            if (gridMoveDirection.y != -1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = +1;
            }

        }

        if (ChangePosition.y < 0 && gridMoveDirection.y != -1) //Down
        {
            if (gridMoveDirection.y != 1)
            {
                gridMoveDirection.x = 0;
                gridMoveDirection.y = -1;
            }
        }

        if (ChangePosition.x < 0 && gridMoveDirection.x != -1) //Left
        {
            if (gridMoveDirection.x != 1)
            {
                gridMoveDirection.x = -1;
                gridMoveDirection.y = 0;
            }

        }

        if (ChangePosition.x > 0 && gridMoveDirection.x != 1) //Right
        {
            if (gridMoveDirection.x != -1)
            {
                gridMoveDirection.x = +1;
                gridMoveDirection.y = 0;
            }

        }
    }

    private void HandleGridMovement()
    {
        gridMoveTimer += Time.deltaTime; //Constant Movement
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridMoveTimer -= gridMoveTimerMax;

            snakeMovePositionList.Insert(0, gridPosition);

            gridPosition += gridMoveDirection;

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

            transform.position = new Vector3(gridPosition.x, gridPosition.y);
            transform.eulerAngles = new Vector3(0, 0, GetAngleFromVector(gridMoveDirection));

            UpdateSnakeBodyParts();
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
            snakeBodyPartList[i].SetGridPosition(snakeMovePositionList[i]);
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
        gridPositionList.AddRange(snakeMovePositionList);
        return gridPositionList;
    }


    private class SnakeBodyPart
    {
        private Vector2 gridPosition;
        private Transform transform;
        public SnakeBodyPart(int bodyIndex)
        {
            GameObject snakeBodyGameObject = new GameObject("SnakeBody", typeof(SpriteRenderer));
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sprite = GameAssets.i.snakeBodySprite;
            snakeBodyGameObject.GetComponent<SpriteRenderer>().sortingOrder = -bodyIndex;
            transform = snakeBodyGameObject.transform;
        }

        public void SetGridPosition(Vector2 gridPosition)
        {
            this.gridPosition = gridPosition;
            transform.position = new Vector3(gridPosition.x, gridPosition.y);
        }
    }
}
