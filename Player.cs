using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof (Controller2D))]
public class Player : MonoBehaviour
{
    public float moveSpeed = 6;
    public float dashDistance = 28;
    public float jumpHeight = 5;
    public float timeToReachApex = .4f;
    public float accelerationTimeAirborne = .2f;
    public float accelerationTimeGrounded = .1f;

    bool doubleJump;
    bool dashed;
    float gravity;
    float jumpVelocity;
    float velocityXSmoothing;

    Vector3 velocity;

    Controller2D controller;

    private void Start()
    {
        controller = GetComponent<Controller2D>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToReachApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToReachApex;
    }

    private void Update()
    {
        if(controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(controller.collisions.below){
                velocity.y = jumpVelocity;
                doubleJump = true;
            }
            else if (doubleJump)
            {
                doubleJump = false;
                velocity.y = jumpVelocity;
            }
        }

        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (!controller.collisions.below)
            {
                // add boolean so can only do it once
                velocity.y = -1.5f * jumpVelocity;
                print("dropped DOWN");

            }
        }



        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded : accelerationTimeAirborne);

        /*
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            if (controller.collisions.below)
            {
                dashed = false;
            }
            if (!dashed)
            {
                velocity.y = 1;
                velocity.x = input.x * moveSpeed * dashDistance;
                dashed = true;
                print("dashed SIDE");
            }
        } 
        */

        velocity.y += gravity * Time.deltaTime;

 

        controller.Move(velocity * Time.deltaTime);
    }
}
