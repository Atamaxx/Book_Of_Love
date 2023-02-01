using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    [SerializeField] private MainController.CharacterController2D movementController;
    private float horizontalMove = 0f;
    private float horizontalInput = 0f;
    private bool jump = false;
    private bool dash = false;
    private bool startMovement = false;

    [SerializeField] private float runSpeed = 500f;
    [SerializeField] private float sprintMultiplier = 2f;
    [SerializeField] private bool canControl = true;
    [SerializeField] private bool moveRight = true;
    [SerializeField] private bool moveLeft = false;
    public bool anyInput = false;

    private float startingRunSpeed;

    int jumpCount = 0;

    private void Start()
    {
        startingRunSpeed = runSpeed;
    }

    private void Update()
    {
        //if (Input.GetButtonDown("Jump"))
        //{
        //    startMovement = true;
        //}

        //if (!startMovement) return;

        HorizontalMovement();

        if (Input.GetButtonDown("Jump"))
            jump = true;


        if (Input.GetButtonUp("Jump"))
            movementController.OnJumpUp(movementController.m_JumpCutMultiplier);


        if (Input.GetButtonDown("Run"))
            runSpeed = startingRunSpeed * sprintMultiplier;

        if (Input.GetButtonUp("Run"))
            runSpeed = startingRunSpeed;


        if (Input.GetButtonDown("Dash"))
            dash = true;


        if (horizontalInput > 0.05f)
            anyInput = true;
        else
            anyInput = false;

    }


    private void FixedUpdate()
    {
        movementController.Move(horizontalMove * Time.fixedDeltaTime, jump, dash);

        jump = false;
        dash = false;
    }



    private void HorizontalMovement()
    {

        if (canControl)
        {
            horizontalInput = Input.GetAxisRaw("Horizontal");
            horizontalMove = horizontalInput * runSpeed;
            return;
        }

        if (moveRight)
        {
            horizontalMove = runSpeed;
        }
        else if (moveLeft)
        {
            horizontalMove = -runSpeed;
        }
        else
        {
            horizontalMove = 0f;
        }


    }


}