using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ShopController : MonoBehaviour
{
    public static ShopController shop { get; private set; }
    public bool isOpen;

    [SerializeField] private GameObject shopPanel;
    private GameObject[] slots;

    public Weapon[] allWeapons;
    private Weapon[] availableWeapons = new Weapon[3];

    private void Awake()
    {
        if (shop != null && shop != this)
        {
            Destroy(gameObject);
            return;
        }
        shop = this;
        allWeapons = Resources.LoadAll<Weapon>("ScriptableObjects/Weapons");
        Array.Sort(allWeapons, (a, b) => a.weaponId - b.weaponId);

        slots = new GameObject[shopPanel.transform.childCount];
        for (int i = 0; i < shopPanel.transform.childCount; i++)
        {
            slots[i] = shopPanel.transform.GetChild(i).gameObject;
        }

        InvokeRepeating("RefreshShopWeapons", 0.0f, 20f);
    }

    public void OpenShop()
    {
        shopPanel.SetActive(true);
    }

    public void CloseShop()
    {
        shopPanel.SetActive(false);
    }

    public void BuyWeapon(Weapon weaponToBuy)
    {
        if (PlayerScript.Player.cash >= weaponToBuy.cost)
        {
            PlayerScript.Player.EquipWeapon(weaponToBuy);
            PlayerScript.Player.cash -= weaponToBuy.cost;
            PlayerScript.Player.UpdateCash(-weaponToBuy.cost);
        } else
        {
            //not enough money to buy
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
            slots[i].transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>(availableWeapons[i].thumbnailPath);
            slots[i].transform.Find("Name").GetComponent<Text>().text = availableWeapons[i].weaponName.ToUpper();
            slots[i].transform.Find("Cost").GetComponent<Text>().text = "$" + availableWeapons[i].cost.ToString();

            if (availableWeapons[i].weaponType == "spread") {
                slots[i].transform.Find("Damage").GetComponent<Text>().text = availableWeapons[i].attackPower.ToString() + "X5 DMG";
            }else {
                slots[i].transform.Find("Damage").GetComponent<Text>().text = availableWeapons[i].attackPower.ToString() + " DMG";
            }

            slots[i].transform.Find("Fire Rate").GetComponent<Text>().text = availableWeapons[i].cooldown.ToString() + "/S";
            slots[i].transform.Find("Range").GetComponent<Text>().text = availableWeapons[i].weaponRange.ToString() + "M";
            slots[i].transform.Find("Ammo Count").GetComponent<Text>().text = availableWeapons[i].ammoCount.ToString() +"X";
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
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
