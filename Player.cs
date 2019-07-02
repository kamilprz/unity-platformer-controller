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


    // MAIN PROGRAM
    private void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToReachApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToReachApex;
    }

    void Update()
    {
        CalculateVelocity();
        HandleWallSliding();

        controller.Move(velocity * Time.deltaTime, directionalInput);
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }
        if (controller.collisions.below)
        {
            dashedMidAir = false;
        }
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
        if (!dashedMidAir)
        {
            if (!controller.collisions.below)
            {
                dashedMidAir = true;
            }
            velocity.x = directionalInput.x * moveSpeed * dashVariable;
        }
    }

    void CalculateVelocity()
    {
        float targetVelocityX = directionalInput.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
    }

    void HandleWallSliding()
    {
        wallDirX = (controller.collisions.left) ? -1 : 1;
        //print("wallDirX: " + wallDirX);
        wallSliding = false;
        if ((controller.collisions.left || controller.collisions.right) && !controller.collisions.below && velocity.y < 0)
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