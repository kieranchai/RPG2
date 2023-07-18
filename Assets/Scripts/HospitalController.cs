using System;
using UnityEngine;
using UnityEngine.UI;

// JOEL

public class HospitalController : MonoBehaviour
{
    public static HospitalController Hospital { get; private set; }

    public bool isOpen;

    public float healthUpgradeModifier;
    public float speedUpgradeModifier;
    public int healthUpgradeCost;
    public int speedUpgradeCost;
    private GameObject[] slots;
    private Upgrades[] allUpgrades;
    private Upgrades[] healthUpgrades;
    private Upgrades[] speedUpgrades;
    [SerializeField] private GameObject hospitalPanel;

    private void Awake()
    {
        if (Hospital != null && Hospital != this)
        {
            Destroy(gameObject);
            return;
        }
        Hospital = this;

        slots = new GameObject[hospitalPanel.transform.childCount];
        for (int i = 0; i < hospitalPanel.transform.childCount; i++)
        {
            slots[i] = hospitalPanel.transform.GetChild(i).gameObject;
        }
    }

    private void Start()
    {
        allUpgrades = AssetManager.Assets.allUpgrades.ToArray();
        Array.Sort(allUpgrades, (a, b) => a.upgradeId - b.upgradeId);

        healthUpgrades = Array.FindAll(allUpgrades, element => element.upgradeType == "HEALTH");
        Array.Sort(allUpgrades, (a, b) => a.upgradeId - b.upgradeId);

        speedUpgrades = Array.FindAll(allUpgrades, element => element.upgradeType == "SPEED");
        Array.Sort(allUpgrades, (a, b) => a.upgradeId - b.upgradeId);

        UpdateModifiers();
        RefreshHospitalUI();
    }
    public void OpenShop()
    {
        hospitalPanel.SetActive(true);
    }

    public void CloseShop()
    {
        GameControllerScript.GameController.canAttack = true;
        hospitalPanel.SetActive(false);
    }

    public void RefreshHospitalUI()
    {
        if (PlayerScript.Player.healthUpgradeLevel >= healthUpgrades.Length)
        {
            slots[0].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            slots[0].transform.Find("Health Upgrade").GetComponent<Text>().text = "Max Health Upgraded";
            slots[0].transform.Find("Description").GetComponent<Text>().text = "Max Health Increased To LVL " + (PlayerScript.Player.healthUpgradeLevel).ToString();
            slots[0].transform.Find("Health Upgrade").GetComponent<Text>().color = new Color(0.8018f, 0.1021f, 0.1021f, 1f);
            slots[0].transform.Find("Description").GetComponent<Text>().color = new Color(0.8018f, 0.1021f, 0.1021f, 1f);
            slots[0].transform.Find("Cost").gameObject.SetActive(false);
        }
        else
        {
            slots[0].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            slots[0].transform.GetComponent<Button>().onClick.AddListener(() => UpgradeHealth());
            slots[0].transform.Find("Description").GetComponent<Text>().text = "Increase Max Health to LVL " + (PlayerScript.Player.healthUpgradeLevel+1).ToString();
            slots[0].transform.Find("Cost").GetComponent<Text>().text = "$ " + healthUpgradeCost.ToString();
        }

        if (PlayerScript.Player.speedUpgradeLevel >= speedUpgrades.Length)
        {
            slots[1].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            slots[1].transform.Find("Speed Upgrade").GetComponent<Text>().text = "Max Speed Upgraded";
            slots[1].transform.Find("Description").GetComponent<Text>().text = "Max Speed Increased To LVL " + (PlayerScript.Player.speedUpgradeLevel).ToString();
            slots[1].transform.Find("Speed Upgrade").GetComponent<Text>().color = new Color(0.8018f, 0.1021f, 0.1021f, 1f);
            slots[1].transform.Find("Description").GetComponent<Text>().color = new Color(0.8018f, 0.1021f, 0.1021f, 1f);
            slots[1].transform.Find("Cost").gameObject.SetActive(false);

        }
        else
        {
            slots[1].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            slots[1].transform.GetComponent<Button>().onClick.AddListener(() => UpgradeSpeed(speedUpgradeModifier, speedUpgradeCost));
            slots[1].transform.Find("Description").GetComponent<Text>().text = "Increase Speed to LVL " + (PlayerScript.Player.speedUpgradeLevel+1).ToString();
            slots[1].transform.Find("Cost").GetComponent<Text>().text = "$ " + speedUpgradeCost.ToString();
        }


    }

    public void UpgradeHealth()
    {
        if (PlayerScript.Player.cash >= healthUpgradeCost)
        {
            AudioManager.instance.PlaySFX("Shop Purchase");
            PlayerScript.Player.maxHealth += healthUpgradeModifier;
            PlayerScript.Player.cash -= healthUpgradeCost;
            PlayerScript.Player.UpdateCash(-healthUpgradeCost);
            PlayerScript.Player.UpdateHealth();
            PlayerScript.Player.healthUpgradeLevel++;
            UpdateModifiers();
            RefreshHospitalUI();
        }
        else
        {
            AudioManager.instance.PlaySFX("Shop Denied");
            return;
        }

    }

    public void UpgradeSpeed(float amount, int cost)
    {
        if (PlayerScript.Player.cash >= speedUpgradeCost)
        {
            AudioManager.instance.PlaySFX("Shop Purchase");
            PlayerScript.Player.speed += amount;
            PlayerScript.Player.cash -= cost;
            PlayerScript.Player.UpdateCash(-cost);
            PlayerScript.Player.speedUpgradeLevel++;
            UpdateModifiers();
            RefreshHospitalUI();
        }else
        {
            AudioManager.instance.PlaySFX("Shop Denied");
            return;
        }
    }

    public void UpdateModifiers()
    {
        if (PlayerScript.Player.healthUpgradeLevel < healthUpgrades.Length)
        {
            healthUpgradeModifier = Array.Find(healthUpgrades, element => element.upgradeLevel == PlayerScript.Player.healthUpgradeLevel + 1).upgradeModifier;
            healthUpgradeCost = Array.Find(healthUpgrades, element => element.upgradeLevel == PlayerScript.Player.healthUpgradeLevel + 1).upgradeCost;
        }

        if (PlayerScript.Player.speedUpgradeLevel < speedUpgrades.Length)
        {
            speedUpgradeModifier = Array.Find(speedUpgrades, element => element.upgradeLevel == PlayerScript.Player.speedUpgradeLevel + 1).upgradeModifier;
            speedUpgradeCost = Array.Find(speedUpgrades, element => element.upgradeLevel == PlayerScript.Player.speedUpgradeLevel + 1).upgradeCost;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OpenShop();
            isOpen = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            CloseShop();
            isOpen = false;
        }
    }
}
