using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript Player { get; private set; }

    public int characterId;
    public string characterName;
    public float maxHealth;
    public float speed;
    public string spritePath;
    public int playerExperience;
    public int playerLvl;
    public int healthUpgradeLevel;
    public int speedUpgradeLevel;
    public Rigidbody2D rb;
    public Animator anim;
    public SpriteRenderer characterSprite;

    public Weapon equippedWeapon;
    public List<Weapon> inventory = new List<Weapon>(1);
    public int cash;
    public float currentHealth;

    [SerializeField] private GameObject inventoryPanel;
    [SerializeField] private GameObject weaponPanel;
    [SerializeField] private GameObject playerStatsPanel;
    [SerializeField] private GameObject playerExperiencePanel;
    [SerializeField] private TMP_Text playerPanelCash;
    [SerializeField] private TMP_Text playerWeaponAmmo;

    private GameObject[] slots;

    public ContactFilter2D movementFilter;
    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public float collisionOffset;

    private float lastHitTime;
    private float regenTimer = 3f;

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
    }

    private void Start()
    {
        SetPlayerData(GameControllerScript.GameController.selectedCharacter);

        slots = new GameObject[inventoryPanel.transform.childCount];
        for (int i = 0; i < inventoryPanel.transform.childCount; i++)
        {
            slots[i] = inventoryPanel.transform.GetChild(i).gameObject;
        }

        RefreshUI();
    }

    private void Update()
    {
        lastHitTime += Time.deltaTime;
        if (this.lastHitTime >= this.regenTimer && this.currentHealth < this.maxHealth && GameControllerScript.GameController.isAlive)
        {
            this.currentHealth += 0.1f * 0.5f; //0.5 speed modifier
            UpdateHealth();
        }
    }

    private void LateUpdate()
    {
        SkinChoice();
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            anim.SetBool("isWalking", true);
        }
        else
        {
            anim.SetBool("isWalking", false);
        }
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
        this.playerExperience = 0;
        this.playerLvl = 0;
        this.healthUpgradeLevel = 0;
        this.speedUpgradeLevel = 0;
        
        characterSprite = gameObject.GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>(this.spritePath);
        characterSprite.sprite = sprite;

        transform.GetChild(0).gameObject.SetActive(true);
        UpdateCash(this.cash);
        UpdateHealth();
        GameControllerScript.GameController.isAlive = true;
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
            weaponPanel.transform.GetChild(0).GetChild(1).GetComponent<Image>().enabled = true;
            playerWeaponAmmo.enabled = true;
            weaponPanel.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>(this.equippedWeapon.thumbnailPath);
            playerWeaponAmmo.text = transform.GetChild(0).GetChild(0).GetComponent<WeaponScript>().currentAmmoCount.ToString();
        }
        catch
        {
            weaponPanel.transform.GetChild(0).GetChild(1).GetComponent<Image>().sprite = null;
            weaponPanel.transform.GetChild(0).GetChild(1).GetComponent<Image>().enabled = false;
            playerWeaponAmmo.text = null;
            playerWeaponAmmo.enabled = false;
        }
    }

    public void ReloadAmmoUI()
    {
        playerWeaponAmmo.text = "...";
    }

    public void UpdateCash(int value)
    {
        playerStatsPanel.GetComponent<SlidingNumber>().AddToNumber(value);
    }

    public void UpdateHealth()
    {
        // for (int i = 0; i < playerStatsPanel.transform.Find("Health").GetComponent<HealthUI>().hearts.Length; i++)
        // {
        //     if (i >= currentHealth)
        //     {
        //         playerStatsPanel.transform.Find("Health").GetComponent<HealthUI>().hearts[playerStatsPanel.transform.Find("Health").GetComponent<HealthUI>().hearts.Length - 1 - i].enabled = false;
        //     }
        // }
        playerStatsPanel.GetComponent<HealthBarScript>().updateHealthBar();
    }

    public void UpdateExperience(int experience)
    {
        if (ModifierController.Modifier.isMaxLvl) return;
        AnalyticsController.Analytics.experienceGained += experience;
        playerExperience += experience;
        if (playerExperience >= ModifierController.Modifier.xpNeeded)
        {
            playerExperience -= ModifierController.Modifier.xpNeeded;
            PopupController.Popup.UpdatePopUp("RESPECT UP!");
            playerLvl++;
            ModifierController.Modifier.UpdateModifiers();
        }
        playerExperiencePanel.GetComponent<ExperienceBarScript>().SetExperience(playerExperience);
        playerExperiencePanel.GetComponent<ExperienceBarScript>().SetRespect(playerLvl);
    }

    public void TakeDamage(float damageTaken)
    {
        if (!GameControllerScript.GameController.isAlive) return;
        currentHealth -= damageTaken;
        AnalyticsController.Analytics.damageTaken += damageTaken;
        this.lastHitTime = 0;
        UpdateHealth();
        if (currentHealth <= 0)
        {
            GameControllerScript.GameController.PlayerDied();
        }
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

    public bool MovePlayer(Vector2 direction)
    {
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            speed * ModifierController.Modifier.speedMod * Time.fixedDeltaTime + collisionOffset);

        if (count == 0)
        {
            Vector2 moveVector = direction * speed * ModifierController.Modifier.speedMod * Time.fixedDeltaTime;

            rb.MovePosition(rb.position + moveVector);
            return true;
        }
        else
        {
            /*            foreach (RaycastHit2D hit in castCollisions)
                        {
                            print(hit.ToString());
                        }*/
            return false;
        }
    }

}

