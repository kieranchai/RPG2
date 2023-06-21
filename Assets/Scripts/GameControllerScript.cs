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

            if (Input.GetMouseButton(0)) PlayerScript.Player.transform.GetChild(0).GetComponentInChildren<WeaponScript>().TryAttack();
        }
    }

    public void LoadScene(Character characterData)
    {
        this.selectedCharacter = characterData;
        SceneManager.LoadScene("Scene1");
    }
}
