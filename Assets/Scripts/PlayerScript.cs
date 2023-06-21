using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Player { get; private set; }

    public int characterId;
    public string characterName;
    public int maxHealth;
    public float speed;
    public string spritePath;

    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer characterSprite;

    public Weapon equippedWeapon;
    public List<Weapon> inventory = new List<Weapon>();
    public int gold = 0;
    public int currentHealth;

    private void Awake()
    {
        if (Player != null && Player != this)
        {
            Destroy(gameObject);
            return;
        }
        Player = this;

        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        transform.GetChild(0).gameObject.SetActive(false);

        SetPlayerData(GameControllerScript.GameController.selectedCharacter);
    }

    private void LateUpdate()
    {
        SkinChoice();
    }

    public void SetPlayerData(Character characterData)
    {
        this.characterId = characterData.characterId;
        this.characterName = characterData.characterName;
        this.maxHealth = characterData.maxHealth;
        this.speed = characterData.speed;
        this.spritePath = characterData.spritePath;
        this.currentHealth = this.maxHealth;

        characterSprite = gameObject.GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>(this.spritePath);
        characterSprite.sprite = sprite;

        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void EquipWeapon(Weapon weaponData)
    {
        if (this.equippedWeapon)
        {
            inventory.Add(this.equippedWeapon);
        }
        this.equippedWeapon = weaponData;
        transform.GetChild(0).GetChild(0).GetComponent<WeaponScript>().SetWeaponData(weaponData);
    }

    public void SkinChoice()
    {
        if (characterSprite.sprite.name.Contains("Main"))
        {
            string spriteName = characterSprite.sprite.name;
            spriteName = spriteName.Replace("Main", characterName);
            characterSprite.sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
        }
    }

    public void LookAtMouse()
    {
        Vector2 mousePos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = (Vector3)(mousePos - new Vector2(transform.position.x, transform.position.y));

        transform.GetChild(0).gameObject.transform.right = this.transform.up.normalized;
    }

    public void MovePlayer()
    {
        var input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        rb.velocity = input.normalized * speed;

        if (rb.velocity.magnitude > 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
    }
}