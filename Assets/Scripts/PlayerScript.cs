using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public List<Weapon> inventory = new List<Weapon>(1);
    public int cash;
    public int currentHealth;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject weaponPanel;
    private GameObject[] slots;

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

        slots = new GameObject[inventoryPanel.transform.childCount];
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            slots[i] = inventoryPanel.transform.GetChild(i).gameObject;
        }

        RefreshUI();
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
        this.cash = 800;

        characterSprite = gameObject.GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>(this.spritePath);
        characterSprite.sprite = sprite;

        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void EquipWeapon(Weapon weaponData)
    {
        if (inventory.Count < inventory.Capacity)
        {
            if (this.equippedWeapon)
            {
                AddToInventory(this.equippedWeapon);
            }
            this.equippedWeapon = weaponData;
            transform.GetChild(0).GetChild(0).GetComponent<WeaponScript>().SetWeaponData(weaponData);
        }
        else
        {
            //inven full, replace equipped weapon
            this.equippedWeapon = weaponData;
            transform.GetChild(0).GetChild(0).GetComponent<WeaponScript>().SetWeaponData(weaponData);
        }
        RefreshUI();
    }

    public void AddToInventory(Weapon weaponData)
    {
        if (!weaponData) return;

        inventory.Add(weaponData);
        RefreshUI();
    }

    public void RemoveFromInventory(Weapon weaponData)
    {
        if (!weaponData) return;

        inventory.Remove(weaponData);
        RefreshUI();
    }

    public void RefreshUI()
    {
        //inventory panel
        for (int i = 0; i < slots.Length; i++)
        {
            try
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(inventory[i].thumbnailPath);
            }
            catch
            {
                slots[i].transform.GetChild(0).GetComponent<Image>().sprite = null;
                slots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
            }
        }
        //weapon panel
        try
        {
            weaponPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = true;
            weaponPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().enabled =  true;
            weaponPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(this.equippedWeapon.thumbnailPath);
            weaponPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = transform.GetChild(0).GetChild(0).GetComponent<WeaponScript>().currentAmmoCount.ToString();
        }
        catch
        {
            weaponPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().sprite = null;
            weaponPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>().enabled = false;
            weaponPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = null;
            weaponPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().enabled = false;
        }
    }

    public void ReloadAmmoUI()
    {
        weaponPanel.transform.GetChild(0).GetChild(1).GetComponent<Text>().text = "...";
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
