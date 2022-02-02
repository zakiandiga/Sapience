using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2Int gridPosition;

    private void Awake()
    {
        gridPosition = new Vector2Int(10, 10);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            gridPosition.y += 1;
        }
        
        transform.position = new Vector3(gridPosition.x, gridPosition.y);
    }
}
