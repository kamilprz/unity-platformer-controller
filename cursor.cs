using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor : MonoBehaviour
{
   [SerializeField] GameObject cur;
   [SerializeField] Camera cam;

    // Update is called once per frame
    void Update()
    {   
        Vector2 pos;
        pos = cam.ScreenToWorldPoint(Input.mousePosition);
        cur.transform.position = pos;
    }
}
