using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class GameControllerScript : MonoBehaviour
{
    public static GameControllerScript GameController { get; private set; }
    public bool isPaused;
    public Character selectedCharacter;

    void Awake()
    {
        if (GameController != null && GameController != this)
        {
            Destroy(gameObject);
            return;
        }
        GameController = this;
        DontDestroyOnLoad(gameObject);

        isPaused = false;
    }

    private void Update()
    {
        if (PlayerScript.Player && !isPaused)
        {
            PlayerScript.Player.LookAtMouse();
            bool success = PlayerScript.Player.MovePlayer(new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized);

            if (!success)
            {
                success = PlayerScript.Player.MovePlayer(new Vector2(Input.GetAxisRaw("Horizontal"), 0));

                if (!success)
                {
                    success = PlayerScript.Player.MovePlayer(new Vector2(0, Input.GetAxisRaw("Vertical")));
                }
            }

            if (Input.GetMouseButton(0))
            {
                PlayerScript.Player.transform.GetChild(0).GetComponentInChildren<WeaponScript>().TryAttack();
            }

            if (Input.GetKeyDown(KeyCode.Q) && PlayerScript.Player.inventory.Count == PlayerScript.Player.inventory.Capacity)
            {
                Weapon temp = PlayerScript.Player.inventory[0];
                PlayerScript.Player.inventory[0] = PlayerScript.Player.equippedWeapon;
                PlayerScript.Player.EquipWeapon(temp);
            }
            if (Input.GetKeyDown(KeyCode.L)) {
                PlayerScript.Player.UpdateExperience(10);  //testing only
            }
            if (Input.GetKeyDown(KeyCode.H)) {
                Debug.Log(PlayerScript.Player.currentHealth);
                PlayerScript.Player.takeDamage(0.2f);
            }
        }
    }

    public void LoadSceneWithCharacter(Character characterData)
    {
        this.selectedCharacter = characterData;
        SceneManager.LoadScene("Loading");
    }
}
