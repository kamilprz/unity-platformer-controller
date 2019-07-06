using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grappleHook : MonoBehaviour
{
    Vector2 targetPos;
    Vector2 position;
    RaycastHit2D hit;
    [SerializeField] float maxDistance = 20f;
    [SerializeField] float grappleSpeed = 2.25f;
    public LayerMask mask;
    bool isGrappling;

    [HideInInspector]
    public BoxCollider2D myCollider;
    Bounds bounds;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = new Vector2(0.0f, 0.0f);
        isGrappling = false;
        myCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        bounds = myCollider.bounds;
        Vector2 topRight = new Vector2 (bounds.max.x, bounds.max.y);
        float boundsDistance = Mathf.Abs(Vector2.Distance(bounds.center, topRight));

        position = (Vector2)transform.position;
        Debug.DrawRay(transform.position, targetPos - position, Color.red);
        // right click
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            targetPos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(position, targetPos - position, maxDistance, mask);
            if (hit) {
                if (Mathf.Abs(Vector2.Distance(transform.position,hit.point )) > boundsDistance ) {
                    isGrappling = true;
                }
            }
        }
        if (isGrappling)
        {
            hit = Physics2D.Raycast(position, targetPos - position, maxDistance, mask);
            
            if (hit)
            {
                transform.position = Vector2.MoveTowards(transform.position, hit.point, grappleSpeed);
            }
            if (Mathf.Abs(Vector2.Distance(transform.position, hit.point)) <= boundsDistance) {
                isGrappling = false;
            }
        }
    }
}
