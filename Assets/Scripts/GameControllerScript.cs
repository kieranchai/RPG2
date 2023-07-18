using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using System;

// KIERAN AND JOEL

public class GameControllerScript : MonoBehaviour
{
    public static GameControllerScript GameController { get; private set; }
    public bool isPaused;
    public Character selectedCharacter;
    public bool isAlive = true;
    public bool gameOver = false;
    public bool hasPlayedTutorial = false;
    public bool canAttack = true;


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
        canAttack = true;
    }

    private void Update()
    {
        if (PlayerScript.Player && !isPaused && this.isAlive && hasPlayedTutorial)
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

            if (Input.GetMouseButton(0) && canAttack)
            {
                PlayerScript.Player.currWeapon.TryAttack();
            }

            if (Input.GetKeyDown(KeyCode.Q) && PlayerScript.Player.inventory.Count == PlayerScript.Player.inventory.Capacity)
            {
                PlayerScript.Player.SwapWeapon();
            }


            //Cheats
            if (Input.GetKeyDown(KeyCode.L))
            {
                PlayerScript.Player.UpdateExperience(10);
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                PlayerScript.Player.cash += 500;
                PlayerScript.Player.UpdateCash(500);
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
        SceneManager.LoadScene("Scene1");
    }

    public void PlayerDied()
    {
        this.isAlive = false;
        PopupController.Popup.SetDeathPopUp();
        PlayerScript.Player.gameObject.GetComponent<Animator>().enabled = false;
        PlayerScript.Player.gameObject.GetComponent<SpriteRenderer>().sprite = AssetManager.Assets.GetSprite("CharacterSprites/Player_Death");
        StartCoroutine(ZoomIn());
    }

    private IEnumerator ZoomIn()
    {
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
