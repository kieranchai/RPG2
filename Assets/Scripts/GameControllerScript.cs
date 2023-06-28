using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

public class GameControllerScript : MonoBehaviour
{
    public static GameControllerScript GameController { get; private set; }
    public bool isPaused;
    public Character selectedCharacter;
    public bool isAlive = true;
    public bool gameOver = false;

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
        if (PlayerScript.Player && !isPaused && this.isAlive)
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


            if (Input.GetKeyDown(KeyCode.L))
            {
                PlayerScript.Player.UpdateExperience(10);  //testing only
            }
            if (Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log(PlayerScript.Player.currentHealth);
                PlayerScript.Player.TakeDamage(0.2f);
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                PlayerScript.Player.EquipWeapon(Array.Find(ShopController.shop.allWeapons, weapon => weapon.weaponName == "M4 Tactical"));
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                PlayerScript.Player.EquipWeapon(Array.Find(ShopController.shop.allWeapons, weapon => weapon.weaponName == "Desert Eagle"));
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                PlayerScript.Player.EquipWeapon(Array.Find(ShopController.shop.allWeapons, weapon => weapon.weaponName == "Mag 7"));
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                PlayerScript.Player.EquipWeapon(Array.Find(ShopController.shop.allWeapons, weapon => weapon.weaponName == "RPG 7"));
            }
        }
    }

    public void LoadSceneWithCharacter(Character characterData)
    {
        this.selectedCharacter = characterData;
        SceneManager.LoadScene("Loading");
    }

    public void PlayerDied()
    {
        this.isAlive = false;
        PopupController.Popup.SetDeathPopUp();
        PlayerScript.Player.gameObject.GetComponent<Animator>().enabled = false;
        PlayerScript.Player.gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/Player_Death");
        StartCoroutine(ZoomIn());
    }

    private IEnumerator ZoomIn()
    {
        Camera camera = GameObject.Find("Main Camera").GetComponent<Camera>();
        {
            while (Camera.main.orthographicSize > 2.5)
            {
                yield return new WaitForSeconds(0.03f);
                Camera.main.orthographicSize -= 0.1f;
            }
        }

        yield return new WaitForSeconds(5f);
        this.gameOver = true;
    }


}
