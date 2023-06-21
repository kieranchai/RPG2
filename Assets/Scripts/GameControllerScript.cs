using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

            // Haven't try to see if completely no bugs but can switch and save weapons
            if (Input.GetKey((KeyCode)(48 + GetPressedNumber())) && PlayerScript.Player.inventory.Count >= GetPressedNumber())
            {
                PlayerScript.Player.EquipWeapon(PlayerScript.Player.inventory.ElementAt(GetPressedNumber() - 1));
                PlayerScript.Player.inventory.RemoveAt(GetPressedNumber() - 1);
                PlayerScript.Player.RefreshUI();
            }
        }
    }

    public int GetPressedNumber()
    {
        for (int number = 1; number <= 9; number++)
        {
            if (Input.GetKeyDown(number.ToString()))
                return number;
        }

        return -1;
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
