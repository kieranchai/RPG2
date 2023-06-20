using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public int characterId;
    public string characterName;
    public int maxHealth;
    public float speed;
    public string spritePath;

    public void setPlayerData(Character characterData)
    {
        this.characterId = characterData.characterId;
        this.characterName = characterData.characterName;
        this.maxHealth= characterData.maxHealth;
        this.speed = characterData.speed;
        this.spritePath = characterData.spritePath;

        SpriteRenderer characterSprite = gameObject.GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>(this.spritePath);
        characterSprite.sprite = sprite;
    }

    public void movePlayer(Vector2 moveDir)
    {
        //move player position
        this.transform.position += (Vector3)moveDir * speed;
    }

    public void tryAttack()
    {
        gameObject.GetComponentInChildren<WeaponScript>().doAttack();
    }
}
