using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Paddle : MinigameBase
{
    public enum GameState
    {
        Waiting,
        Alive,
        Dead
    }

    public Ball ball { get; private set; }
    private bool oneTime;
    public new Rigidbody2D rigidbody { get; private set; }
    public static GameState state;
    public float speed = 30f; //Speed of the paddle
    public float maxBounceAngle = 75f; //Maximum angle the ball will bounce off the paddle at

    [SerializeField] private InputActionReference changeDirection;
    public Vector2 inputDirection
    {
        get
        {
            return changeDirection.action.ReadValue<Vector2>();
        }
    }
    public Vector2 movingDirection;

    

    private void Awake()
    {
        this.ball = FindObjectOfType<Ball>();
        this.rigidbody = GetComponent<Rigidbody2D>();
        state = GameState.Waiting;
        oneTime = false;
    }

    private void Start()
    {
        changeDirection.action.Enable();
    }

    private void OnDisable()
    {
        changeDirection.action.Disable();
    }



    public void ResetPaddle()
    {
        this.transform.position = new Vector2(0f, this.transform.position.y);
        this.rigidbody.velocity = Vector2.zero;
    }



    private void Update()
    {
        switch (state)
        {
            case GameState.Waiting:
                break;
            case GameState.Alive:
                if (!oneTime)
                {
                    ball.ResetBall();
                    FindObjectOfType<BrickGameManager>().StartMusic();
                    oneTime = true;
                }
                HandleInput();
                break;
            case GameState.Dead:
                FindObjectOfType<BrickGameManager>().StopMusic();
                ball.StopBall();
                EndingMinigame();
                state = GameState.Waiting;
                break;
        }
    }

    private void FixedUpdate()
    {
        if (this.movingDirection != Vector2.zero)
        {
            this.rigidbody.AddForce(this.movingDirection * this.speed);
        }
    }



    private void HandleInput()
    {
        if (inputDirection.x < 0) //Left
        {
            this.movingDirection = Vector2.left;
        }

        if (inputDirection.x > 0) //Right
        {
            this.movingDirection = Vector2.right;
        }

        if (inputDirection.x == 0) //Idle
        {
            this.movingDirection = Vector2.zero;
        }
    }



    private void OnCollisionEnter2D(Collision2D collision) //Changes the ball's bounce depending upon where it hits the paddle. Not strictly necessary but adds some gameplay enhancements
    {
        Ball ball = collision.gameObject.GetComponent<Ball>();

        if(ball != null)
        {
            Vector3 paddlePosition = this.transform.position;
            Vector2 contactPoint = collision.GetContact(0).point;

            float offset = paddlePosition.x - contactPoint.x;
            float width = collision.otherCollider.bounds.size.x / 2; //Gets the width of paddle and divides it by two

            float currentAngle = Vector2.SignedAngle(Vector2.up, ball.rigidbody.velocity);
            float bounceAngle = (offset / width) * this.maxBounceAngle;
            float newAngle = Mathf.Clamp(currentAngle + bounceAngle, -this.maxBounceAngle, this.maxBounceAngle); //Uses the current angle of the ball and where the ball hits the paddle to determine the angle the ball bounces off at

            Quaternion rotation = Quaternion.AngleAxis(newAngle, Vector3.forward); //Sets the ball's new angle that was calculated above
            ball.rigidbody.velocity = rotation * Vector2.up * ball.rigidbody.velocity.magnitude;

            FindObjectOfType<BrickGameManager>().PlayPaddleBounceSound();
        }
    }


        
    public void StartGame()
    {
        state = GameState.Alive;
    }
    
    public void Dead()
    {
        state = GameState.Dead;
    }
}
