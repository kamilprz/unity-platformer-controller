using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grappleHook : MonoBehaviour
{
    Vector2 targetPos;
    Vector2 position;
    RaycastHit2D hit;
    public float maxDistance = 20f;
    public float moveDistance = 5f;
    public LayerMask mask;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = new Vector2(0.0f, 0.0f);
        position = (Vector2)transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // right click
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            targetPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);

            //hit = Physics2D.Raycast(transform.position, targetPos - position, maxDistance, mask);

            //if (hit)
            //{
               transform.position = Vector2.MoveTowards(transform.position, targetPos, moveDistance);
           // }
        }
    }
}
