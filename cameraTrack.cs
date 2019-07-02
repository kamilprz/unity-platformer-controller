using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraTrack : MonoBehaviour
{
    public Controller2D target;
    public Vector2 focusAreaSize;
    FocusArea focusArea;

    public float verticalOffset;
    public float lookAheadDistX;
    public float lookSmoothTimeX;
    public float verticalSmoothTime;

    float currentLookAheadX;
    float targetLookAheadX;
    float lookAheadDirX;
    float smoothLookVelocityX;
    float smoothLookVelocityY;
    bool lookAheadStopped;



    void Start()
    {
        focusArea = new FocusArea(target.myCollider.bounds, focusAreaSize);
    }

    // lateupdate will be after player has moved
    private void LateUpdate()
    {
        // update the focusArea
        focusArea.Update(target.myCollider.bounds);
        // update where camera focusPosition should be (centre point of camera)
        Vector2 focusPosition = focusArea.centre + Vector2.up * verticalOffset;

        // if focusArea changed X position
        if (focusArea.velocity.x != 0)
        {
            lookAheadDirX = Mathf.Sign(focusArea.velocity.x);
            if (Mathf.Sign(target.playerInput.x) == Mathf.Sign(focusArea.velocity.x) && target.playerInput.x != 0)
            {
                lookAheadStopped = false;
                targetLookAheadX = lookAheadDirX * lookAheadDistX;
            }
            else
            {
                if (!lookAheadStopped)
                {
                    lookAheadStopped = true;
                    targetLookAheadX = currentLookAheadX + (lookAheadDirX * lookAheadDistX - currentLookAheadX) / 4f;
                }
            }
        }

        targetLookAheadX = lookAheadDirX * lookAheadDistX;
        currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, targetLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);

        focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothLookVelocityY, verticalSmoothTime);
        focusPosition += Vector2.right * currentLookAheadX;
        transform.position = (Vector3)focusPosition + Vector3.forward * -10;
    }

    // function to visualise the focusArea in scene window
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, .5f);
        Gizmos.DrawCube(focusArea.centre, focusAreaSize);
    }

    struct FocusArea
    {
        // focus area is an area around a player where the player can move without the camera moving
        public Vector2 centre;
        public Vector2 velocity;
        float left, right;
        float top, bottom;

        // constructor
        public FocusArea(Bounds targetBounds, Vector2 size)
        {
            right = targetBounds.center.x + size.x / 2;
            left = targetBounds.center.x - size.x / 2;
            top = targetBounds.min.y + size.y;
            bottom = targetBounds.min.y;
            velocity = Vector2.zero;
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        public void Update(Bounds targetBounds)
        {
            float shiftX = 0;
            // targetBounds refer to players collider bounds
            // if player has moved out of the focusArea to the left
            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            // if player has moved out of focusArea to the right
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }
            // update - if player didnt move out of focusArea, update will be by 0
            left += shiftX;
            right += shiftX;

            // same principle as above, but with Y-axis
            float shiftY = 0;
            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }
            top += shiftY;
            bottom += shiftY;

            // calculate new centre of focusArea
            centre = new Vector2((left + right) / 2, (top + bottom) / 2);
            // velocity is by how much it has moved
            velocity = new Vector2(shiftX, shiftY);
        }
    }
}