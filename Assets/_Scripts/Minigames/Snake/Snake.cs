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

    [SerializeField] private InputActionReference changeDirection;

    private void Start()
    {
        changeDirection.action.Enable();
    }

    private void Awake()
    {
        gridPosition = new Vector2(10, 10);
        gridMoveTimerMax = 1f;
        gridMoveTimer = gridMoveTimerMax;
        gridMoveDirection = new Vector2(1, 0);
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

        gridMoveTimer += Time.deltaTime; //Constant Movement
        if (gridMoveTimer >= gridMoveTimerMax)
        {
            gridPosition += gridMoveDirection;
            gridMoveTimer -= gridMoveTimerMax;
        }

        transform.position = new Vector3(gridPosition.x, gridPosition.y);
    }
}
