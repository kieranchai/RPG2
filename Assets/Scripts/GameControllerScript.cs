using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControllerScript : MonoBehaviour
{
    private bool isPaused;
    private PlayerScript player;

    private void Awake()
    {
        player = FindObjectOfType<PlayerScript>();
        isPaused = false;
    }

    private void Update()
    {
        if (!isPaused)
        {
            if (Input.GetMouseButtonDown(0)) player.tryAttack();
        }
    }

    private void FixedUpdate()
    {
        if (!isPaused)
        {
            Vector2 moveDir = Vector2.zero;
            if (Input.GetKey(KeyCode.W)) moveDir += Vector2.up;
            if (Input.GetKey(KeyCode.S)) moveDir += Vector2.down;
            if (Input.GetKey(KeyCode.A)) moveDir += Vector2.left;
            if (Input.GetKey(KeyCode.D)) moveDir += Vector2.right;

            //move player position
            player.movePlayer(moveDir.normalized * Time.fixedDeltaTime);

        }
    }
}
