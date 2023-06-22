using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameControllerScript : MonoBehaviour
{
    public static GameControllerScript GameController { get; private set; }
    private bool isPaused;
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
        if (!isPaused && PlayerScript.Player)
        {
            PlayerScript.Player.LookAtMouse();
            PlayerScript.Player.MovePlayer();

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

            if (Input.GetKeyDown(KeyCode.B))
            {
                if (!ShopController.shop.isOpen)
                {
                    ShopController.shop.OpenShop();
                } else
                {
                    ShopController.shop.CloseShop();
                }
            }
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LoadSceneWithCharacter(Character characterData)
    {
        this.selectedCharacter = characterData;
        SceneManager.LoadScene("Scene1");
    }
}
