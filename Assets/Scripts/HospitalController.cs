using System;
using UnityEngine;
using UnityEngine.UI;

// JOEL

public class HospitalController : MonoBehaviour
{
    public static HospitalController Hospital { get; private set; }

    public bool isOpen;
    private GameObject[] slots;
    private Upgrades[] allUpgrades;
    public Upgrades[] healthUpgrades;
    public Upgrades[] speedUpgrades;
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
        
        foreach (Upgrades upgrade in allUpgrades) {
            upgrade.isCompleted = false;
        }

        healthUpgrades = Array.FindAll(allUpgrades, element => element.upgradeType == "HEALTH");
        Array.Sort(allUpgrades, (a, b) => a.upgradeId - b.upgradeId);

        speedUpgrades = Array.FindAll(allUpgrades, element => element.upgradeType == "SPEED");
        Array.Sort(allUpgrades, (a, b) => a.upgradeId - b.upgradeId);
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
        if (!Array.Find(healthUpgrades, element => element.isCompleted == false))
        {
            slots[0].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            slots[0].transform.Find("Health Upgrade").GetComponent<Text>().text = "Max Health Upgraded";
            slots[0].transform.Find("Description").GetComponent<Text>().text = "Max Health Increased To LVL " + healthUpgrades.Length;
            slots[0].transform.Find("Health Upgrade").GetComponent<Text>().color = new Color(0.8018f, 0.1021f, 0.1021f, 1f);
            slots[0].transform.Find("Description").GetComponent<Text>().color = new Color(0.8018f, 0.1021f, 0.1021f, 1f);
            slots[0].transform.Find("Cost").gameObject.SetActive(false);
        }
        else
        {
            slots[0].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            slots[0].transform.GetComponent<Button>().onClick.AddListener(() => UpgradeHealth(Array.Find(healthUpgrades, element => element.isCompleted == false)));
            slots[0].transform.Find("Description").GetComponent<Text>().text = "Increase Max Health to LVL " + Array.Find(healthUpgrades, element => element.isCompleted == false).upgradeLevel.ToString();
            slots[0].transform.Find("Cost").GetComponent<Text>().text = "$ " + Array.Find(healthUpgrades, element => element.isCompleted == false).upgradeCost.ToString();
        }

        if (!Array.Find(speedUpgrades, element => element.isCompleted == false))
        {
            slots[1].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            slots[1].transform.Find("Speed Upgrade").GetComponent<Text>().text = "Max Speed Upgraded";
            slots[1].transform.Find("Description").GetComponent<Text>().text = "Max Speed Increased To LVL " + speedUpgrades.Length;
            slots[1].transform.Find("Speed Upgrade").GetComponent<Text>().color = new Color(0.8018f, 0.1021f, 0.1021f, 1f);
            slots[1].transform.Find("Description").GetComponent<Text>().color = new Color(0.8018f, 0.1021f, 0.1021f, 1f);
            slots[1].transform.Find("Cost").gameObject.SetActive(false);

        }
        else
        {
            slots[1].transform.GetComponent<Button>().onClick.RemoveAllListeners();
            slots[1].transform.GetComponent<Button>().onClick.AddListener(() => UpgradeSpeed(Array.Find(speedUpgrades, element => element.isCompleted == false)));
            slots[1].transform.Find("Description").GetComponent<Text>().text = "Increase Speed to LVL " + Array.Find(speedUpgrades, element => element.isCompleted == false).upgradeLevel.ToString();
            slots[1].transform.Find("Cost").GetComponent<Text>().text = "$ " + Array.Find(speedUpgrades, element => element.isCompleted == false).upgradeCost.ToString();
        }

    }
    public void UpgradeHealth(Upgrades health)
    {
        if (PlayerScript.Player.cash >= health.upgradeCost)
        {
            health.isCompleted = true;
            AudioManager.instance.PlaySFX("Shop Purchase");
            PlayerScript.Player.maxHealth += health.upgradeModifier;
            PlayerScript.Player.cash -= health.upgradeCost;
            PlayerScript.Player.UpdateCash(-health.upgradeCost);
            PlayerScript.Player.UpdateHealth();
            RefreshHospitalUI();
        }
        else
        {
            AudioManager.instance.PlaySFX("Shop Denied");
            return;
        }

    }

    public void UpgradeSpeed(Upgrades speed)
    {
        if (PlayerScript.Player.cash >= speed.upgradeCost)
        {
            speed.isCompleted = true;
            AudioManager.instance.PlaySFX("Shop Purchase");
            PlayerScript.Player.speed += speed.upgradeModifier;
            PlayerScript.Player.cash -= speed.upgradeCost;
            PlayerScript.Player.UpdateCash(-speed.upgradeCost);
            PlayerScript.Player.UpdateHealth();
            RefreshHospitalUI();
        }
        else
        {
            AudioManager.instance.PlaySFX("Shop Denied");
            return;
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
