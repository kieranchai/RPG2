using System;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreen : MonoBehaviour
{
    [SerializeField] private GameObject buttonPanel;
    [SerializeField] private GameObject statsPanel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GameControllerScript.GameController.isPaused = !GameControllerScript.GameController.isPaused;
            PauseGame(GameControllerScript.GameController.isPaused);
        }
    }

    public void PauseGame(bool isPaused)
    {
        switch (isPaused)
        {
            case true:
                Time.timeScale = 0f;
                ShopController.shop.CloseShop();
                buttonPanel.SetActive(true);
                statsPanel.SetActive(false);
                break;
            case false:
                Time.timeScale = 1;
                buttonPanel.SetActive(false);
                statsPanel.SetActive(false);
                GameControllerScript.GameController.isPaused = false;
                if (ShopController.shop.isOpen) ShopController.shop.OpenShop();
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

    public void exportStats()
    {
        string file = Application.dataPath + "/Editor/CSVs/StatsOutput.csv";
        TextWriter tw = new StreamWriter(file, false);
        tw.WriteLine("Enemies Killed, Damage Taken, Damage Dealt, Time Played (s)");
        tw.Close();

        tw = new StreamWriter(file, true);

        tw.WriteLine(AnalyticsController.Analytics.enemiesKilled.ToString() + "," + AnalyticsController.Analytics.damageTaken.ToString() + "," + AnalyticsController.Analytics.damageDealt.ToString() + "," + AnalyticsController.Analytics.timePlayed.ToString());
        tw.Close();
    }

}
