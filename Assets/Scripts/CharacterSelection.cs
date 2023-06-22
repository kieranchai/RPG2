using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
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

    void UpdateCharacterData(int selectedCharacter)
    {
        characterName.text = allCharacters[selectedCharacter].characterName.ToUpper();
        characterImage.sprite = Resources.Load<Sprite>(allCharacters[selectedCharacter].spritePath);
        characterHp.text = allCharacters[selectedCharacter].maxHealth.ToString() + "HP";
        characterSpeed.text = allCharacters[selectedCharacter].speed.ToString() + "MS";
    }

    public void NextCharacter()
    {
        selectedCharacter++;
        if (selectedCharacter > allCharacters.Length - 1)
        {
            selectedCharacter = 0;
        }
        UpdateCharacterData(selectedCharacter);
    }

    public void PrevCharacter()
    {
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
