using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof (Player))]
public class PlayerInput : MonoBehaviour
{

    Player player;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();   
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        player.SetDirectionalInput(directionalInput);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            player.Jump();
        }
        if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
        {
            player.FastFall();
        }
        if (Input.GetKeyDown(KeyCode.LeftAlt))
        {
            player.Dash();
        }
    }
}
