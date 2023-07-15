using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopController : MonoBehaviour
{
    public static ShopController shop { get; private set; }
    public bool isOpen;

    [SerializeField] private GameObject shopPanel;
    [SerializeField] private TMP_Text shopTimer;
    private GameObject[] slots;

    public Weapon[] allWeapons;
    private Weapon[] availableWeapons = new Weapon[3];

    private float refreshTimer = 15f;

    private void Awake()
    {
        if (shop != null && shop != this)
        {
            Destroy(gameObject);
            return;
        }
        shop = this;
        allWeapons = AssetManager.Assets.allWeapons.ToArray();
        allWeapons = Array.FindAll(allWeapons, e => e.weaponName != "Training Glock");
        Array.Sort(allWeapons, (a, b) => a.weaponId - b.weaponId);

        slots = new GameObject[shopPanel.transform.childCount];
        for (int i = 0; i < shopPanel.transform.childCount; i++)
        {
            slots[i] = shopPanel.transform.GetChild(i).gameObject;
        }
        RefreshShopWeapons();
    }

    private void Update()
    {
        shopTimer.text = $"Refreshing in {refreshTimer:N0}";
        refreshTimer -= Time.deltaTime;
        if(refreshTimer <= 0f)
        {
            RefreshShopWeapons();
            refreshTimer = 15f;
        }
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
        shopTimer.gameObject.SetActive(true);
    }

    public void CloseShop()
    {
        GameControllerScript.GameController.canAttack = true;
        shopPanel.SetActive(false);
        shopTimer.gameObject.SetActive(false);
    }

    public void BuyWeapon(Weapon weaponToBuy)
    {
        if (PlayerScript.Player.cash >= weaponToBuy.cost)
        {
            PlayerScript.Player.EquipWeapon(weaponToBuy);
            PlayerScript.Player.cash -= weaponToBuy.cost;
            PlayerScript.Player.UpdateCash(-weaponToBuy.cost);
            AudioManager.instance.PlaySFX("Shop Purchase");
        }
        else
        {
            //not enough money to buy
            AudioManager.instance.PlaySFX("Shop Denied");
            return;
        }
    }

    public void RefreshShopWeapons()
    {
        for (int i = 0; i < 3; i++)
        {
            availableWeapons[i] = allWeapons[Random.Range(0, allWeapons.Length)];
        }
        RefreshUI();
    }


    public void RefreshUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            int i2 = i;
            slots[i].transform.GetComponent<Button>().onClick.AddListener(() => BuyWeapon(availableWeapons[i2]));
            slots[i].transform.GetChild(0).GetComponent<Image>().sprite = AssetManager.Assets.GetSprite(availableWeapons[i2].thumbnailPath); 
            slots[i].transform.Find("Name").GetComponent<Text>().text = availableWeapons[i].weaponName.ToUpper();
            slots[i].transform.Find("Cost").GetComponent<Text>().text = "$" + availableWeapons[i].cost.ToString();

            if (availableWeapons[i].weaponType == "spread")
            {
                slots[i].transform.Find("Damage").GetComponent<Text>().text = availableWeapons[i].attackPower.ToString() + "X5 DMG";
            }
            else
            {
                slots[i].transform.Find("Damage").GetComponent<Text>().text = availableWeapons[i].attackPower.ToString() + " DMG";
            }

            slots[i].transform.Find("Fire Rate").GetComponent<Text>().text = availableWeapons[i].cooldown.ToString() + "/S";
            slots[i].transform.Find("Range").GetComponent<Text>().text = availableWeapons[i].weaponRange.ToString() + "M";
            slots[i].transform.Find("Ammo Count").GetComponent<Text>().text = availableWeapons[i].ammoCount.ToString() + "X";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            OpenShop();
            isOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            CloseShop();
            isOpen = false;
        }
    }
}
