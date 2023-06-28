using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public Text characterName;
    public Image characterImage;
    public Text characterHp;
    public Text characterSpeed;

    private Character[] allCharacters;
    public int selectedCharacter = 0;

    void Awake()
    {
        allCharacters = Resources.LoadAll<Character>("ScriptableObjects/Characters");
        UpdateCharacterData(selectedCharacter);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            NextCharacter();
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            PrevCharacter();
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            LoadGame();
        }
    }

    void UpdateCharacterData(int selectedCharacter)
    {
        characterName.text = allCharacters[selectedCharacter].characterName.ToUpper();
        characterImage.sprite = Resources.Load<Sprite>(allCharacters[selectedCharacter].spritePath);
        characterHp.text = allCharacters[selectedCharacter].maxHealth.ToString() + "HP";
        characterSpeed.text = allCharacters[selectedCharacter].speed.ToString() + "MS";
    }

    public void NextCharacter()
    {
        AudioManager.instance.PlaySFX("Menu Hover");
        selectedCharacter++;
        if (selectedCharacter > allCharacters.Length - 1)
        {
            selectedCharacter = 0;
        }
        UpdateCharacterData(selectedCharacter);
    }

    public void PrevCharacter()
    {
        AudioManager.instance.PlaySFX("Menu Hover");
        selectedCharacter--;
        if (selectedCharacter < 0)
        {
            selectedCharacter = allCharacters.Length - 1;
        }
        UpdateCharacterData(selectedCharacter);
    }

    public void LoadGame()
    {
        GameControllerScript.GameController.LoadSceneWithCharacter(allCharacters[selectedCharacter]);
    }
}
