using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FlappyBirdPlayer : MonoBehaviour
{
    [SerializeField] private InputActionReference flap;

    private Vector3 direction;
    public float gravity = -9.8f;
    public float strength = 5f;
    public bool ActionPressed => flap.action.triggered;


    private void Start()
    {
        flap.action.Enable();
    }

    private void OnDisable()
    {
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }

    private void Update()
    {
        if (ActionPressed)
        {
            direction = Vector3.up * strength;
        }

        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Obstacle")
        {
            FindObjectOfType<FlappyGameManager>().LoseLife();
        } else if (collision.gameObject.tag == "Scoring")
        {
            FindObjectOfType<FlappyGameManager>().IncreaseScore();
        }
    }
}
