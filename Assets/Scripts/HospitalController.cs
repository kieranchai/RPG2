using System;
using UnityEngine;
using UnityEngine.UI;

public class HospitalController : MonoBehaviour
{
    public static HospitalController hospital { get; private set; }

    public bool isOpen;
    public bool healthUpgraded;
    public bool speedUpgraded;

    public float healthupgradeAmount;
    public float speedupgradeAmount;
    public int healthupgradeCost;
    public int speedupgradeCost;
    private GameObject[] slots;
    [SerializeField] private GameObject hospitalPanel;
    private void Awake()
    {
        if (hospital != null && hospital != this)
        {
            Destroy(gameObject);
            return;
        }
        hospital = this;

        speedupgradeAmount = 1; //hardcode
        healthupgradeAmount = 10;
        healthupgradeCost = 100;
        speedupgradeCost = 100;

        slots = new GameObject[hospitalPanel.transform.childCount];
        for (int i = 0; i < hospitalPanel.transform.childCount; i++)
        {
            slots[i] = hospitalPanel.transform.GetChild(i).gameObject;
        }
        setHospitalData();
    }

    public void OpenShop()
    {
        hospitalPanel.SetActive(true);
    }

    public void CloseShop()
    {
        hospitalPanel.SetActive(false);
    }

    public void setHospitalData()
    {
        slots[0].transform.GetComponent<Button>().onClick.AddListener(() => upgradeHealth(healthupgradeAmount,healthupgradeCost));
        slots[0].transform.Find("Description").GetComponent<Text>().text = "Increase Max Health by " + healthupgradeAmount.ToString();
        slots[0].transform.Find("Cost").GetComponent<Text>().text = "$ " + healthupgradeCost.ToString();

        slots[1].transform.GetComponent<Button>().onClick.AddListener(() => upgradeSpeed(speedupgradeAmount,speedupgradeCost));
        slots[1].transform.Find("Description").GetComponent<Text>().text = "Increase Speed by " + speedupgradeAmount.ToString();
        slots[1].transform.Find("Cost").GetComponent<Text>().text = "$ " + speedupgradeCost.ToString();
    }

    public void upgradeHealth(float amount, int cost) {
        PlayerScript.Player.maxHealth += amount;
        PlayerScript.Player.cash -= cost;
        PlayerScript.Player.UpdateCash(-cost);
        PlayerScript.Player.UpdateHealth();

        healthUpgraded = true;

        slots[0].transform.GetComponent<Button>().onClick.RemoveAllListeners();
        slots[0].transform.Find("Health Upgrade").GetComponent<Text>().text = "Max Health Upgraded";
        slots[0].transform.Find("Description").GetComponent<Text>().text = "Max Health Increased To " + PlayerScript.Player.maxHealth.ToString();
        slots[0].transform.Find("Health Upgrade").GetComponent<Text>().color = new Color(0.8018f,0.1021f,0.1021f,1f);
        slots[0].transform.Find("Description").GetComponent<Text>().color = new Color(0.8018f,0.1021f,0.1021f,1f);
        slots[0].transform.Find("Cost").gameObject.SetActive(false);

        Debug.Log("max health upgraded to " + PlayerScript.Player.maxHealth);
    }

    public void upgradeSpeed(float amount, int cost) {
        PlayerScript.Player.speed += amount;
        PlayerScript.Player.cash -= cost;
        PlayerScript.Player.UpdateCash(-cost);

        speedUpgraded = true;

        slots[1].transform.GetComponent<Button>().onClick.RemoveAllListeners();
        slots[1].transform.Find("Speed Upgrade").GetComponent<Text>().text = "Max Speed Upgraded";
        slots[1].transform.Find("Description").GetComponent<Text>().text = "Speed Increased To " + PlayerScript.Player.speed.ToString();
        slots[1].transform.Find("Speed Upgrade").GetComponent<Text>().color = new Color(0.8018f,0.1021f,0.1021f,1f);
        slots[1].transform.Find("Description").GetComponent<Text>().color = new Color(0.8018f,0.1021f,0.1021f,1f);
        slots[1].transform.Find("Cost").gameObject.SetActive(false);

        Debug.Log("speed upgraded to " + PlayerScript.Player.speed);
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
