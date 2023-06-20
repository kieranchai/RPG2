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
        if (!isPaused && player.hasInit)
        {
            player.LookAtMouse();
            player.MovePlayer();

            if (Input.GetMouseButton(0)) player.transform.GetChild(0).GetComponentInChildren<WeaponScript>().TryAttack();
        }
    }
}
