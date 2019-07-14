using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grappleHook : MonoBehaviour
{
   [SerializeField] Vector3 targetPos;
    Vector2 position;
    RaycastHit2D hit;
    [SerializeField] float maxDistance = 20f;
    [SerializeField] float grappleSpeed = 2.25f;
    public LayerMask mask;
    bool isGrappling;
    [SerializeField] float timer;

    [HideInInspector]
    public BoxCollider2D myCollider;
    Bounds bounds;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = new Vector3(0.0f, 0.0f);
        isGrappling = false;
        myCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(timer);
        timer +=1*Time.deltaTime;
        bounds = myCollider.bounds;
        Vector2 topRight = new Vector2 (bounds.max.x, bounds.max.y);
        float boundsDistance = Vector2.Distance(bounds.center, topRight);
        Debug.Log(boundsDistance);
        //position = (Vector2)transform.position;
        Debug.DrawRay(transform.position, targetPos - transform.position, Color.red);
        // right click
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            timer = 0;
            isGrappling = false;
            targetPos = (Vector3)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            hit = Physics2D.Raycast(transform.position, targetPos - transform.position, maxDistance, mask);
            if (hit) {
                if (Vector2.Distance(transform.position,hit.point ) >= boundsDistance ) {
                    isGrappling = true;
                }
            }
        }
        if (isGrappling)
        {
            Debug.Log("yes");
            hit = Physics2D.Raycast(transform.position, targetPos - transform.position, maxDistance, mask);
            if (timer >= 0.45)
            {
                isGrappling = false;
            }
            if (hit)
            {
                transform.position = Vector2.MoveTowards(transform.position, hit.point, grappleSpeed);
            }
           
            if (Vector2.Distance(transform.position, hit.point) <= boundsDistance+1.90)
            {
                isGrappling = false;
            }
          
            

        }
    }
}
