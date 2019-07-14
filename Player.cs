using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class Player : MonoBehaviour
{
    // GENERAL
    public float moveSpeed = 6;
    public float jumpHeight = 5;
    public float timeToReachApex = .4f;
    // used in smoothdamp
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;
    float oldAccelerationTimeGrounded;

    //bool doubleJump;
    float gravity;
    float jumpVelocity;
    float velocityXSmoothing;

    Vector2 velocity;
    Controller2D controller;
    Vector2 directionalInput;


    // WALL JUMP/SLIDE
    public float wallSlideSpeedMax = 3;
    public float wallStickTime = .25f;
    public Vector2 wallJumpClimb;
    public Vector2 wallJumpOff;
    public Vector2 wallLeap;

    float timeToWallUnstick;
    bool wallSliding;
    int wallDirX;


    // DASH
    public float dashVariable = 28;
    bool dashedMidAir;
    bool isDashing;

    //timer
    [SerializeField] float counter;
    [SerializeField]float counterMax;

    //grappling
    RaycastHit2D hit;
    public LayerMask mask;
    public bool isGrappling;
    Vector2 targetPos;
    [SerializeField] float maxDistance = 50f;
    float grappleAngle;
    [SerializeField] float grappleSpeed = 2.25f;


    // MAIN PROGRAM
    private void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToReachApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToReachApex;
        isDashing = false;
        oldAccelerationTimeGrounded = accelerationTimeGrounded;
    }

   

    void Update()
    {
        counter += 1 * Time.deltaTime;
       
        CalculateVelocity();
        HandleWallSliding();
        
        controller.Move(velocity * Time.deltaTime, directionalInput);
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
            isGrappling = false;
        }
        if (controller.collisions.below)
        {
            dashedMidAir = false;
        }
        if (isGrappling && (controller.collisions.left || controller.collisions.right)) {
            isGrappling = false;
            velocity.x = 0;
            velocity.y = 0;
        }

        Debug.Log(isGrappling);
    }

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void Jump()
    {
        if (wallSliding)
        {
            if (wallDirX == directionalInput.x)
            {
                velocity.x = -wallDirX * wallJumpClimb.x;
                velocity.y = wallJumpClimb.y;
            }
            else if (directionalInput.x == 0)
            {
                velocity.x = -wallDirX * wallJumpOff.x;
                velocity.y = wallJumpOff.y;
            }
            else
            {
                velocity.x = -wallDirX * wallLeap.x;
                velocity.y = wallLeap.y;
            }
        }
        //else
        //{
        if (controller.collisions.below)
        {
            velocity.y = jumpVelocity;
            //doubleJump = true;
        }
        //else if (doubleJump)
        //{
        //    doubleJump = false;
        //    velocity.y = jumpVelocity;
        //}
        //}
    }

    public void FastFall()
    {
        if (!controller.collisions.below)
        {
            // add boolean so can only do it once
            wallSliding = false;
            velocity.y = -1.5f * jumpVelocity;
        }
    }

    public void Dash()
    {
        isDashing = true;
        if (!dashedMidAir)
        {
            if (!controller.collisions.below)
            {
                dashedMidAir = true;
            }
            velocity.x = directionalInput.x * dashVariable;
        }
    }

    public void Grapple()
    {
        targetPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        hit = Physics2D.Raycast(transform.position, targetPos - (Vector2)transform.position, maxDistance, mask);
        if (hit)
        {
            if (Vector2.Distance(transform.position, hit.point) >= 2.35)
            {
                isGrappling = true;
            }
        }
        
        if (isGrappling)
        {   
            if (hit.point.x > transform.position.x)
            {
                directionalInput.x = 1;
            }

            if (hit.point.x < transform.position.x)
            {
                directionalInput.x = -1;
            }

            grappleAngle = Mathf.Atan2(hit.point.y - transform.position.y, hit.point.x - transform.position.x);

            velocity.x = grappleSpeed * Mathf.Cos(grappleAngle);
            velocity.y = (grappleSpeed * Mathf.Sin(grappleAngle)) - (gravity * Time.deltaTime);

        }
        if (Vector2.Distance(transform.position, hit.point) <= 2.35)
        {
            isGrappling = false;
        }
    }

        void CalculateVelocity()
    {
        if (!isGrappling)
        {
            float targetVelocityX = directionalInput.x * moveSpeed;

            if (isDashing)
            {
                counter = 0;
                oldAccelerationTimeGrounded = accelerationTimeGrounded;
                accelerationTimeGrounded = accelerationTimeAirborne;
                isDashing = false;
            }
            if (counter >= counterMax)
            {
                accelerationTimeGrounded = oldAccelerationTimeGrounded;
            }

            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        }
        velocity.y += gravity * Time.deltaTime;
    }

    void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        //print("wallDirX: " + wallDirX);
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0 && directionalInput.x!=0)
        {
            wallSliding = true;
           
                if (velocity.y < -wallSlideSpeedMax)
                {
                    velocity.y = -wallSlideSpeedMax;
                }

                if (timeToWallUnstick > 0)
                {
                    velocityXSmoothing = 0;
                    velocity.x = 0;
                    if ((directionalInput.x != wallDirX) && (directionalInput.x != 0))
                    {
                        timeToWallUnstick -= Time.deltaTime;
                    }
                    else
                    {
                        timeToWallUnstick = wallStickTime;
                    }
                }
                else
                {
                    timeToWallUnstick = wallStickTime;
                }
            
        }
    }
}