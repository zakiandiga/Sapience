using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Fungus;

public class PlayerController : MonoBehaviour
{
    public BlockReference currentBlockReference;

    private Vector2 inputVector = Vector2.zero;
    private Rigidbody2D rb;

    [SerializeField] private float speed = 20;
    
    Animator anim;

    [SerializeField] private KeyCode upInput, downInput, leftInput, rightInput;
    private bool canMove = false;
    
    private PlayerMode playerMode = PlayerMode.walking;

    public static event Action<BlockReference> OnCallingDialogue;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        MovementManager.OnBlockEnd += EnablingPlayer;
    }

    private void OnDisable()
    {
        MovementManager.OnBlockEnd -= EnablingPlayer;
    }

    private void EnablingPlayer(string blockName)
    {
        canMove = true;
    }

    private void DisablingPlayer(string playerName)
    {
        canMove = false;
    }

    public void MoveInput()
    {
        if (playerMode == PlayerMode.walking) //Drew replaced "if (!isTalking)" with the PlayerMode state machine
        {
                        
            if (inputVector != Vector2.zero)
            {
                anim.SetFloat("Horizontal", inputVector.x);
                anim.SetFloat("Vertical", inputVector.y);
            }
            anim.SetFloat("Speed", inputVector.sqrMagnitude);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + inputVector * speed * Time.deltaTime);
    }

    void Update()
    {
        if(canMove)
        {
            if (Input.GetKey(upInput))
            {
                inputVector.y = 1;
            }

            if (Input.GetKey(downInput))
            {
                inputVector.y = -1;
            }

            if (Input.GetKey(leftInput))
            {
                inputVector.x = -1;
            }

            if (Input.GetKey(rightInput))
            {
                inputVector.x = 1;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                DisablingPlayer("test");
                OnCallingDialogue?.Invoke(currentBlockReference);
                Debug.Log("Player calling dialogue");
            }

            if (Input.GetKeyUp(upInput) || Input.GetKeyUp(downInput) || Input.GetKeyUp(leftInput) || Input.GetKeyUp(rightInput))
                inputVector = Vector2.zero;
        }
        
        // Non-physic movement mode
        //    Vector3 currentPos = transform.position;
        //    currentPos += inputVector * speed * Time.deltaTime;
        //    transform.position = currentPos;
    }

    public enum PlayerMode
    {
        walking,
        idle,
        talking
    }
}
