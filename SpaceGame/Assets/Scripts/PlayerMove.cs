using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    bool isFacingRight = true;

    [Header("Movement")]
    public float moveSpeed = 5f;
    private bool canMove = true;

    float horizontalMovement;

    [Header("Jumping")]
    public float jumpPower = 5f;
    public int maxJumps = 2;
    public int jumpsRemaining;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.49f, 0.03f);
    public LayerMask groundLayer;
    bool isGrounded;

    [Header("Gravity")]
    public float baseGravity = 2;
    public float maxFallSpeed = 18f;
    public float fallSpeedMultiplier = 2f;

    [Header("WallCheck")]
    public Transform wallCheckPos;
    public Vector2 wallCheckSize = new Vector2(0.49f, 0.03f);
    public LayerMask wallLayer;

    [Header("WallMovement")]
    public float wallSlideSpeed = 2f;
    bool isWallSliding;

    //wall jumping
    bool isWallJumping;
    float wallJumpDirection;
    float wallJumpTime = 0.05f;
    float wallJumpTimer;
    public Vector2 wallJumpPower = new Vector2(5f, 10f);


    private void Start()
    {

    }

    private void Update()
    {
        GroundCheck();
        ProcessGravity();
        ProcessWallSlide();
        ProcessWallJump();

        if (!isWallJumping)
        {
            rb.velocity = new Vector2(horizontalMovement * (moveSpeed), rb.velocity.y); //move
            Flip();
        }

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);
        animator.SetBool("isWallSliding", isWallSliding);

    }

    public void ProcessGravity()
    {
        if (rb.velocity.y < 0)
        {
            rb.gravityScale = baseGravity * fallSpeedMultiplier; //player falls increasingly faster
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -maxFallSpeed)); //caps fallspeed
        }
        else
        {
            rb.gravityScale = baseGravity;
        }
    }


    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    public void Jump(InputAction.CallbackContext context)
    {

        if (!canMove)
        {
            return;
        }
        else if (jumpsRemaining > 0)
        {
            if (context.performed)
            {
                //holding button = full height
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                jumpsRemaining--;
                animator.SetTrigger("jump");
            }
            else if (context.canceled)
            {
                //press button = half height
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                jumpsRemaining--;
                animator.SetTrigger("jump");
            }
        }

        if (jumpsRemaining <= 0)
        {
            return;
        }

        //Wall Jump
        if (context.performed && wallJumpTimer > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpDirection * wallJumpPower.x, wallJumpPower.y);
            wallJumpTimer = 0f;
            animator.SetTrigger("jump");


            //force flip
            /*  if (transform.localScale.x != wallJumpDirection) //not facing the way we jump
              {
                  isFacingRight = !isFacingRight;
                  Vector3 ls = transform.localScale;
                  ls.x *= -1f;
                  transform.localScale = ls;
              }
            */
            Invoke(nameof(CancelWallJump), wallJumpTime + 0.1f); //wall jump lasts 0.5seconds, can jump again after 0.6seconds
        }

    }

    private void GroundCheck()
    {
        if (Physics2D.OverlapBox(groundCheckPos.position, groundCheckSize, 0, groundLayer))
        {
            jumpsRemaining = maxJumps;
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private bool WallCheck()
    {
        return Physics2D.OverlapBox(wallCheckPos.position, wallCheckSize, 0, wallLayer);
    }

    private void ProcessWallSlide()
    {
        if (!isGrounded && WallCheck() && horizontalMovement != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(rb.velocity.y, -wallSlideSpeed)); //caps falling speed
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void ProcessWallJump()
    {
        if (!isWallSliding)
        {
            isWallJumping = false;
            wallJumpDirection = -transform.localScale.x; //jump in opposite direction
            wallJumpTimer = wallJumpTime;

            CancelInvoke(nameof(CancelWallJump));
        }
        else if (wallJumpTimer > 0f)
        {
            wallJumpTimer = Time.deltaTime;
        }
    }

    private void CancelWallJump()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontalMovement < 0 || !isFacingRight && horizontalMovement > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 ls = transform.localScale;
            ls.x *= -1f;
            transform.localScale = ls;
        }
    }


    private void OnDrawGizmosSelected()
    {
        //groundCheck box
        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        //wallCheck box
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(wallCheckPos.position, wallCheckSize);
    }

    public void DisableMovement()
    {
        horizontalMovement = 0;
        canMove = false;
        enabled = false;
    }

    public void EnableMovement()
    {
        canMove = true;
        enabled = true;
    }

}
