using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelection : MonoBehaviour
{
    public Text characterName;
    public Button characterButton;
    public Image characterImage;
    public Text characterHp;
    public Text characterSpeed;
    public Character characterData;

    void Awake()
    {
        FillCharacterData();
    }

    void FillCharacterData()
    {
        characterName.text = characterData.characterName;
        characterImage.sprite = Resources.Load<Sprite>(characterData.spritePath);
        characterHp.text = characterData.maxHealth.ToString() + "HP";
        characterSpeed.text = characterData.speed.ToString() + "MS";
        characterButton.onClick.AddListener(delegate { GameControllerScript.GameController.LoadScene(characterData); });
    }
}
