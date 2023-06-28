using System;
using UnityEngine;
using UnityEngine.UI;

public class HospitalController : MonoBehaviour
{
    public static HospitalController hospital { get; private set; }

    public bool isOpen;
    // public bool healthUpgraded;
    // public bool speedUpgraded;
    public float healthupgradeModifier;
    public float speedUpgradeModifier;
    public int healthupgradeCost;
    public int speedupgradeCost;
    private GameObject[] slots;
    private Upgrades[] allUpgrades;
    private Upgrades[] healthUpgrades;
    private Upgrades[] speedUpgrades;
    [SerializeField] private GameObject hospitalPanel;

    private void Awake()
    {
        if (hospital != null && hospital != this)
        {
            Destroy(gameObject);
            return;
        }
        hospital = this;

        slots = new GameObject[hospitalPanel.transform.childCount];
        for (int i = 0; i < hospitalPanel.transform.childCount; i++)
        {
            slots[i] = hospitalPanel.transform.GetChild(i).gameObject;
        }
    }

    private void Start()
    {
        allUpgrades = Resources.LoadAll<Upgrades>("ScriptableObjects/Upgrades");
        Array.Sort(allUpgrades, (a, b) => a.upgradeId - b.upgradeId);

        healthUpgrades = Array.FindAll(allUpgrades, element => element.upgradeType == "HEALTH");
        Array.Sort(allUpgrades, (a, b) => a.upgradeId - b.upgradeId);

        speedUpgrades = Array.FindAll(allUpgrades, element => element.upgradeType == "SPEED");
        Array.Sort(allUpgrades, (a, b) => a.upgradeId - b.upgradeId);

        updateModifiers();
        refreshHospitalUI();
    }
    public void OpenShop()
    {
        hospitalPanel.SetActive(true);
    }

    public void CloseShop()
    {
        hospitalPanel.SetActive(false);
    }

    public void refreshHospitalUI()
    {
        if (PlayerScript.Player.healthupgradeLevel >= healthUpgrades.Length)
        {
            slots[0].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            slots[0].transform.Find("Health Upgrade").GetComponent<Text>().text = "Max Health Upgraded";
            slots[0].transform.Find("Description").GetComponent<Text>().text = "Max Health Increased To LVL " + (PlayerScript.Player.healthupgradeLevel).ToString();
            slots[0].transform.Find("Health Upgrade").GetComponent<Text>().color = new Color(0.8018f, 0.1021f, 0.1021f, 1f);
            slots[0].transform.Find("Description").GetComponent<Text>().color = new Color(0.8018f, 0.1021f, 0.1021f, 1f);
            slots[0].transform.Find("Cost").gameObject.SetActive(false);
        }
        else
        {
            slots[0].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            slots[0].transform.GetComponent<Button>().onClick.AddListener(() => upgradeHealth());
            slots[0].transform.Find("Description").GetComponent<Text>().text = "Increase Max Health to LVL " + (PlayerScript.Player.healthupgradeLevel+1).ToString();
            slots[0].transform.Find("Cost").GetComponent<Text>().text = "$ " + healthupgradeCost.ToString();
        }

        if (PlayerScript.Player.speedupgradeLevel >= speedUpgrades.Length)
        {
            slots[1].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            slots[1].transform.Find("Speed Upgrade").GetComponent<Text>().text = "Max Speed Upgraded";
            slots[1].transform.Find("Description").GetComponent<Text>().text = "Max Speed Increased To LVL " + (PlayerScript.Player.speedupgradeLevel).ToString();
            slots[1].transform.Find("Speed Upgrade").GetComponent<Text>().color = new Color(0.8018f, 0.1021f, 0.1021f, 1f);
            slots[1].transform.Find("Description").GetComponent<Text>().color = new Color(0.8018f, 0.1021f, 0.1021f, 1f);
            slots[1].transform.Find("Cost").gameObject.SetActive(false);

        }
        else
        {
            slots[1].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            slots[1].transform.GetComponent<Button>().onClick.AddListener(() => upgradeSpeed(speedUpgradeModifier, speedupgradeCost));
            slots[1].transform.Find("Description").GetComponent<Text>().text = "Increase Speed to LVL " + (PlayerScript.Player.speedupgradeLevel+1).ToString();
            slots[1].transform.Find("Cost").GetComponent<Text>().text = "$ " + speedupgradeCost.ToString();
        }


    }

    public void upgradeHealth()
    {
        if (PlayerScript.Player.cash >= healthupgradeCost)
        {
            PlayerScript.Player.maxHealth += healthupgradeModifier;
            PlayerScript.Player.cash -= healthupgradeCost;
            PlayerScript.Player.UpdateCash(-healthupgradeCost);
            PlayerScript.Player.UpdateHealth();
            PlayerScript.Player.healthupgradeLevel++;
            updateModifiers();
            refreshHospitalUI();
        }
        else
        {
            return;
        }

    }

    public void upgradeSpeed(float amount, int cost)
    {
        if (PlayerScript.Player.cash >= speedupgradeCost)
        {
            PlayerScript.Player.speed += amount;
            PlayerScript.Player.cash -= cost;
            PlayerScript.Player.UpdateCash(-cost);
            PlayerScript.Player.speedupgradeLevel++;
        }
        updateModifiers();
        refreshHospitalUI();

    }

    public void updateModifiers()
    {
        if (PlayerScript.Player.healthupgradeLevel < healthUpgrades.Length)
        {
            healthupgradeModifier = Array.Find(healthUpgrades, element => element.upgradeLevel == PlayerScript.Player.healthupgradeLevel + 1).upgradeModifier;
            healthupgradeCost = Array.Find(healthUpgrades, element => element.upgradeLevel == PlayerScript.Player.healthupgradeLevel + 1).upgradeCost;
        }

        if (PlayerScript.Player.speedupgradeLevel < speedUpgrades.Length)
        {
            speedUpgradeModifier = Array.Find(speedUpgrades, element => element.upgradeLevel == PlayerScript.Player.speedupgradeLevel + 1).upgradeModifier;
            speedupgradeCost = Array.Find(speedUpgrades, element => element.upgradeLevel == PlayerScript.Player.speedupgradeLevel + 1).upgradeCost;
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
