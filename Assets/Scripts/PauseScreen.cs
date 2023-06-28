using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject statsPanel;
    private bool deathScreenDisplayed;

    private void Start()
    {
        this.deathScreenDisplayed = false;
        buttonPanel.transform.GetChild(0).gameObject.SetActive(true);
        PauseGame(false);
        GameControllerScript.GameController.gameOver = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && GameControllerScript.GameController.isAlive)
        {
            GameControllerScript.GameController.isPaused = !GameControllerScript.GameController.isPaused;
            PauseGame(GameControllerScript.GameController.isPaused);
        }
        if (GameControllerScript.GameController.gameOver && !deathScreenDisplayed) DeathScreen();
    }

    public void PauseGame(bool isPaused)
    {
        switch (isPaused)
        {
            case true:
                Time.timeScale = 0f;
                ShopController.shop.CloseShop();
                HospitalController.Hospital.CloseShop();
                buttonPanel.SetActive(true);
                statsPanel.SetActive(false);
                break;
            case false:
                Time.timeScale = 1;
                buttonPanel.SetActive(false);
                statsPanel.SetActive(false);
                GameControllerScript.GameController.isPaused = false;
                if (ShopController.shop.isOpen) ShopController.shop.OpenShop();
                if (HospitalController.Hospital.isOpen) HospitalController.Hospital.OpenShop();
                break;
        }
    }

    public void ViewStats()
    {
        statsPanel.SetActive(true);
        buttonPanel.SetActive(false);
    }

    public void QuitGame()
    {
        exportStats();
        GameControllerScript.GameController.isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void DeathScreen()
    {
        this.deathScreenDisplayed = true;
        Time.timeScale = 0f;
        ShopController.shop.CloseShop();
        HospitalController.Hospital.CloseShop();
        buttonPanel.SetActive(true);
        buttonPanel.transform.GetChild(1).gameObject.SetActive(false);
        statsPanel.SetActive(false);
    }

    public void exportStats()
    {
        string FileName = "StatsOutput"; // This var will be edited to FileName(1), FileName(2) and so on...
        string BaseFileName = "StatsOutput"; // To prevent cases like "FileName(12345)", we 'reset' it by having a "base name". 
        int i = 0;

        while (File.Exists($"{Application.persistentDataPath}/{FileName}.csv"))
        {
            i = i + 1;
            FileName = $"{BaseFileName}({i})";
        }

        string file = Application.persistentDataPath + $"/{FileName}.csv";
        TextWriter tw = new StreamWriter(file, false);
        tw.WriteLine("Enemies Killed, Damage Taken, Damage Dealt, Time Played (s), Experience Gained");
        tw.Close();

        tw = new StreamWriter(file, true);

        tw.WriteLine(AnalyticsController.Analytics.enemiesKilled.ToString() + "," + AnalyticsController.Analytics.damageTaken.ToString() + "," + AnalyticsController.Analytics.damageDealt.ToString() + "," + AnalyticsController.Analytics.timePlayed.ToString() + "," + AnalyticsController.Analytics.experienceGained.ToString());
        tw.Close();
    }

}
