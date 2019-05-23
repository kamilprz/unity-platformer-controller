using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// cannot exist if there is no player
[RequireComponent(typeof (Player))]
public class PlayerInput : MonoBehaviour
{
    // reference to the player
    Player player;

    void Start()
    {
        player = GetComponent<Player>();   
    }

    void Update()
    {
        Vector2 directionalInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        // sets the input to a variable in player class
        player.SetDirectionalInput(directionalInput);

        // special movement : jump - fast fall - side dash
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
