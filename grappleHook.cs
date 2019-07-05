using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grappleHook : MonoBehaviour
{
    Controller2D controller;
    Vector2 targetPos;
    Vector2 position;
    RaycastHit2D hit;
    [SerializeField] float maxDistance = 100f;
    [SerializeField] float grappleSpeed = 2.25f;
    public LayerMask mask;
    public bool isGrappling;

    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<Controller2D>();
        targetPos = new Vector2(0.0f, 0.0f);
        isGrappling = false;
    }

    // Update is called once per frame
    void Update()
    {
        position = (Vector2)transform.position;
        Debug.DrawRay(transform.position, targetPos - position, Color.red);
        // right click
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            isGrappling = true;
            targetPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
        if (isGrappling)
        {
            hit = Physics2D.Raycast(position, targetPos - position, maxDistance, mask);
            print("isGrappling in grapple script");
            if (hit)
            {
                transform.position = Vector2.MoveTowards(transform.position, hit.point, grappleSpeed);
            }
            if (Mathf.Abs(Vector2.Distance(transform.position, hit.point)) <= 3.62) {
                isGrappling = false;
            }
        }
    }
}
