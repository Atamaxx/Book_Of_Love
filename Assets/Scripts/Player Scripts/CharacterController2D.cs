using System;
using UnityEngine;
using UnityEngine.Events;

public class CharacterController2D : MonoBehaviour
{
    public float m_JumpForce = 20f;                                             // Amount of force added when the player jumps.
    public float m_DashForce = 20f;                                             // Amount of force added when the player dashes.
    [SerializeField] private int m_MaxNumberOfJumps = 2;
    [Range(0, 1)] public float m_JumpCutMultiplier = 1f;                        // Decreasing "y" velocity on releasing "jump button" for controllable jump
    [Range(0, .3f)][SerializeField] private float m_MovementSmoothing = .05f;   // How much to smooth out the movement
    [SerializeField] private float m_FallGravityMultiplier = 3f;

    public bool m_AirControl = false;                                           // Whether or not a player can steer while jumping;
    public bool m_Grounded = true;                                              // Whether or not the player is grounded.
    private bool m_FacingRight = true;                                          // For determining which way the player is currently facing.
    private bool m_IsJumping = false;
    
    public LayerMask m_WhatIsGround;                                            // A mask determining what is ground to the character
    [SerializeField] private Transform m_GroundCheck;                           // A position marking where to check if the player is grounded.
    [SerializeField] private Dash dashing;

    [SerializeField] const float k_GroundedRadius = .4f;                                         // Radius of the overlap circle to determine if grounded
    private Vector3 m_Velocity = Vector3.zero;
    private Vector2 m_TargetVelocity;
    private Rigidbody2D m_Rigidbody2D;
    private float m_GravityScale;
    private int m_CurrentJump = 0;
    private float landingVelocityThreshold = -1f;





    [Header("Events")]
    [Space]

    public UnityEvent OnLandEvent;



    [System.Serializable]
    public class BoolEvent : UnityEvent<bool> { }


    private void Awake()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();

        m_GravityScale = m_Rigidbody2D.gravityScale;


        if (OnLandEvent == null)
            OnLandEvent = new UnityEvent();
    }

    private void Start()
    {
        m_GroundCheck = transform.Find("GroundCheck").GetComponent<Transform>();
    }




    public void Move(float move, bool jump, bool dash)
    {
        GroundCheck();
        // If the input is moving the player right and the player is facing left...
        if (move > 0 && !m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }
        // Otherwise if the input is moving the player left and the player is facing right...
        else if (move < 0 && m_FacingRight)
        {
            // ... flip the player.
            Flip();
        }


        // If the player should jump...
        if ((m_Grounded || m_CurrentJump < m_MaxNumberOfJumps) && jump)
        {
            m_CurrentJump++;
            OnSecondJump();
            m_Grounded = false;
            m_IsJumping = true;
            Jump(m_JumpForce);
        }

        #region Jump Gravity
        if (m_Rigidbody2D.velocity.y < 0)
        {
            m_Rigidbody2D.gravityScale = m_GravityScale * m_FallGravityMultiplier;
        }
        else
        {
            m_Rigidbody2D.gravityScale = m_GravityScale;
        }
        #endregion

        if (dash)
        {
            if (!dashing.DashThoughWalls(m_WhatIsGround))
            {
                PerfomDash(m_DashForce);
            }

        }




        // Move the character by finding the target velocity
        m_TargetVelocity.Set(move, m_Rigidbody2D.velocity.y);
        // And then smoothing it out and applying it to the character
        m_Rigidbody2D.velocity = Vector3.SmoothDamp(m_Rigidbody2D.velocity, m_TargetVelocity, ref m_Velocity, m_MovementSmoothing);
        // m_Rigidbody2D.velocity = m_TargetVelocity;



    }

    private void OnSecondJump()
    {
        if (m_CurrentJump <= 1) return;

        m_Rigidbody2D.velocity = new(m_Rigidbody2D.velocity.x, 0);
    }

    private void Flip()
    {
        // Switch the way the player is labelled as facing.
        m_FacingRight = !m_FacingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    #region Jump
    public void Jump(float jumpForce)
    {
        //apply force, using impluse force mode
        m_Rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
    }
    public void OnJumpUp(float jumpCutMultiplier)
    {
        if (m_Rigidbody2D.velocity.y > 0 && m_AirControl && m_IsJumping)
        {
            //reduces current y velocity by amount (0 - 1)
            m_Rigidbody2D.AddForce(jumpCutMultiplier * m_Rigidbody2D.velocity.y * Vector2.down, ForceMode2D.Impulse);
        }
    }

    //private void WhileJumping()
    //{
    //    if (!m_IsJumping) return;

    //    #region Jump Gravity
    //    if (m_Rigidbody2D.velocity.y < 0)
    //    {
    //        m_Rigidbody2D.gravityScale = m_GravityScale * m_FallGravityMultiplier;
    //    }
    //    else
    //    {
    //        m_Rigidbody2D.gravityScale = m_GravityScale;
    //    }
    //    #endregion
    //}
    #endregion

    #region Dash

    private void PerfomDash(float dashForce)
    {
        if (m_FacingRight)
        {
            m_Rigidbody2D.AddForce(Vector2.right * dashForce, ForceMode2D.Impulse);
        }

        if (!m_FacingRight)
        {
            m_Rigidbody2D.AddForce(Vector2.left * dashForce, ForceMode2D.Impulse);
        }
    }

    #endregion

    #region When You Are Butters
    public void WhenGrounded()
    {
        m_IsJumping = false;
        m_CurrentJump = 0;
    }

    private void GroundCheck()
    {
        bool wasGrounded;
        wasGrounded = m_Grounded;

        m_Grounded = Physics2D.OverlapCircle(m_GroundCheck.position, k_GroundedRadius, m_WhatIsGround);

        //if (!wasGrounded && m_Grounded)
        //    OnLandEvent.Invoke();
        if (m_Grounded)
        {
            if (m_Rigidbody2D.velocity.y <= landingVelocityThreshold)
            {
                OnLandEvent.Invoke();
            }

        }
    }

    #endregion

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(m_GroundCheck.position, k_GroundedRadius);
    }
}