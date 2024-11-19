using System;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public Animator animator;
    bool isFacingRight = true;
    private bool isSliding = false;//added

    [Header("Movement")]
    public float moveSpeed = 5f;
    public bool canMove = true; //made it public to access it in the healthManagement class

    [Header("Ice Sliding Settings")]
    public float iceFriction = 0.98f; // Friction factor for ice, closer to 1 means more sliding

    private bool isOnIce = false;

    float horizontalMovement;

    [Header("Jumping")]
    public float jumpPower = 5f;
    public int maxJumps = 2;
    public int jumpsRemaining;

    [Header("GroundCheck")]
    public Transform groundCheckPos;
    public Vector2 groundCheckSize = new Vector2(0.6f, 0.1f); //(0.49f, 0.03f);
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

    [Header("SpotLight")]
    public Light2D spotlight;
    private bool isSpotlightOn = false;
    public SpriteRenderer spriteRenderer;
    public Material defaultMaterial;
    public Material spriteLitMaterial;


    [Header("Target Area")]
    public Vector2 bottomLeftCorner = new Vector2(132.5319f, 20f);
    public Vector2 topRightCorner = new Vector2(190f, 77f); 

    private bool isInTargetArea = false;

    private float materialChangeCooldown = 0.1f; 
    private float lastMaterialChangeTime = 0;


    private void Start()
    {
        if (spotlight != null)
        {
            spotlight.enabled = false;
        }

        if (spriteRenderer != null && defaultMaterial != null)
        {
            spriteRenderer.material = defaultMaterial;
        }

        

    }

    private void Update()
    {
        GroundCheck();
        ProcessGravity();
        ProcessWallSlide();
        ProcessWallJump();

        if (!isWallJumping && canMove && !isSliding) //added && canMove
        {
            rb.velocity = new Vector2(horizontalMovement * (moveSpeed), rb.velocity.y); //move
            Flip();
        }
        

        animator.SetFloat("yVelocity", rb.velocity.y);
        animator.SetFloat("magnitude", rb.velocity.magnitude);
        animator.SetBool("isWallSliding", isWallSliding);

        CheckTargetArea();

        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleSpotlight();
        }

    }


    private void FixedUpdate()
    {
        if (canMove)
        {
            if (isOnIce)
            {
                // Apply sliding behavior on ice
                rb.velocity = new Vector2(rb.velocity.x * iceFriction, rb.velocity.y);

                // If there is player input, apply some control over the sliding
                if (Mathf.Abs(horizontalMovement) > 0.01f)
                {
                    rb.AddForce(new Vector2(horizontalMovement * moveSpeed, 0), ForceMode2D.Force);
                }
            }
            else
            {
                // Normal movement
                rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ice"))
        {
            isOnIce = true;
            Debug.Log("Player is on ice");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Ice"))
        {
            isOnIce = false;
        }
    }







    private void CheckTargetArea()
    {
        Vector2 playerPosition = transform.position;

        float buffer = 0.5f;

        bool isInArea = playerPosition.x >= bottomLeftCorner.x - buffer && playerPosition.x <= topRightCorner.x + buffer &&
                        playerPosition.y >= bottomLeftCorner.y - buffer && playerPosition.y <= topRightCorner.y + buffer;

        if (Time.time - lastMaterialChangeTime >= materialChangeCooldown)
        {
            if (isInArea && !isInTargetArea)
            {                
                isInTargetArea = true;
                ChangeMaterial(spriteLitMaterial);
                lastMaterialChangeTime = Time.time;
            }
            else if (!isInArea && isInTargetArea)
            {
                isInTargetArea = false;
                ChangeMaterial(defaultMaterial);
                lastMaterialChangeTime = Time.time;
            }
        }
    }


    private void ChangeMaterial(Material material)
    {
        if (spriteRenderer != null && material != null)
        {
            spriteRenderer.material = material;
        }
    }

    private void ToggleSpotlight()
    {
        if (spotlight != null)
        {
            isSpotlightOn = !isSpotlightOn;
            spotlight.enabled = isSpotlightOn;

        }
    }

    public void StartSliding()
    {
        isSliding = true;
        rb.velocity = new Vector2(transform.localScale.x * moveSpeed, rb.velocity.y);
        jumpsRemaining = 0;
    }

    public void ProcessGravity()
    {
        if (rb.velocity.y < 0 || isSliding)
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
        if (isSliding)
        {
            horizontalMovement = 0;
        }
        else
        {
            horizontalMovement = context.ReadValue<Vector2>().x;
        }
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
            canMove = true; //added for the spikes behaviour

            if(isSliding)
            {
                isSliding = false;
            }

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
        
            // GroundCheck box
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundCheckPos.position, groundCheckSize);
        

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
