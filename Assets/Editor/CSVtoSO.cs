using UnityEditor;
using UnityEngine;
using System.IO;

// KIERAN AND JOEL

public class CSVtoSO
{
    private static string characterCSVPath = "/Assets/Editor/CSVs/CharacterCSV.csv";
    private static string enemyCSVPath = "/Assets/Editor/CSVs/EnemyCSV.csv";
    private static string weaponCSVPath = "/Assets/Editor/CSVs/WeaponCSV.csv";
    private static string questCSVPath = "/Assets/Editor/CSVs/QuestCSV.csv";
    private static string dialogueCSVPath = "/Assets/Editor/CSVs/DialogueCSV.csv";
    private static string modifierCSVPath = "/Assets/Editor/CSVs/ModifierCSV.csv";
    private static string upgradesCSVPath = "/Assets/Editor/CSVs/UpgradesCSV.csv";
    private static string achievementCSVPath = "/Assets/Editor/CSVs/AchievementCSV.csv";
    private static string enemySpawnCSVPath = "/Assets/Editor/CSVs/EnemySpawnCSV.csv";

    [MenuItem("Utilities/Generate Characters")]
    public static void GenerateCharacters()
    {
        string[] allLines = File.ReadAllLines(System.IO.Directory.GetCurrentDirectory() + characterCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            if (splitData.Length != 5)
            {
                return;
            }

            Character character = ScriptableObject.CreateInstance<Character>();
            character.characterId = int.Parse(splitData[0]);
            character.speed = float.Parse(splitData[1]);
            character.maxHealth = float.Parse(splitData[2]);
            character.characterName = splitData[3];
            character.spritePath = splitData[4];

            AssetDatabase.CreateAsset(character, $"Assets/ScriptableObjects/Characters/{character.characterName}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Enemies")]
    public static void GenerateEnemies()
    {
        string[] allLines = File.ReadAllLines(System.IO.Directory.GetCurrentDirectory() + enemyCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            if (splitData.Length != 8)
            {
                return;
            }

            Enemy enemy = ScriptableObject.CreateInstance<Enemy>();
            enemy.enemyId = int.Parse(splitData[0]);
            enemy.enemyName = splitData[1];
            enemy.maxHealth = float.Parse(splitData[2]);
            enemy.speed = float.Parse(splitData[3]);
            enemy.spritePath = splitData[4];
            enemy.equippedWeaponName = splitData[5];
            enemy.xpDrop = int.Parse(splitData[6]);
            enemy.cashDrop = splitData[7];

            AssetDatabase.CreateAsset(enemy, $"Assets/ScriptableObjects/Enemies/{enemy.enemyName}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Weapons")]
    public static void GenerateWeapons()
    {
        string[] allLines = File.ReadAllLines(System.IO.Directory.GetCurrentDirectory() + weaponCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            if (splitData.Length != 11)
            {
                return;
            }

            Weapon weapon = ScriptableObject.CreateInstance<Weapon>();
            weapon.weaponId = int.Parse(splitData[0]);
            weapon.weaponName = splitData[1];
            weapon.attackPower = float.Parse(splitData[2]);
            weapon.spritePath = splitData[3];
            weapon.thumbnailPath = splitData[4];
            weapon.weaponType = splitData[5];
            weapon.cooldown = float.Parse(splitData[6]);
            weapon.weaponRange = float.Parse(splitData[7]);
            weapon.ammoCount = int.Parse(splitData[8]);
            weapon.cost = int.Parse(splitData[9]);
            weapon.reloadSpeed = float.Parse(splitData[10]);

            AssetDatabase.CreateAsset(weapon, $"Assets/ScriptableObjects/Weapons/{weapon.weaponName}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Quests")]
    public static void GenerateQuests()
    {
        string[] allLines = File.ReadAllLines(System.IO.Directory.GetCurrentDirectory() + questCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            if (splitData.Length != 6)
            {
                return;
            }

            Quest quest = ScriptableObject.CreateInstance<Quest>();
            quest.questId = int.Parse(splitData[0]);
            quest.questType = splitData[1];
            quest.questAmount = splitData[2];
            quest.questObject = splitData[3];
            quest.xpReward = int.Parse(splitData[4]);
            quest.cashReward = int.Parse(splitData[5]);


            AssetDatabase.CreateAsset(quest, $"Assets/ScriptableObjects/Quests/{quest.questId}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Dialogue")]
    public static void GenerateDialogue()
    {
        string[] allLines = File.ReadAllLines(System.IO.Directory.GetCurrentDirectory() + dialogueCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            if (splitData.Length != 9)
            {
                return;
            }

            Dialogue dialogue = ScriptableObject.CreateInstance<Dialogue>();
            dialogue.dialogueId = int.Parse(splitData[0]);
            dialogue.portraitSpritePath = splitData[1];
            dialogue.speakerName = splitData[2];
            dialogue.dialogueText = splitData[3];
            dialogue.action1Name = splitData[4];
            dialogue.action2Name = splitData[5];
            dialogue.action1DialogueId = int.Parse(splitData[6]);
            dialogue.action2DialogueId = int.Parse(splitData[7]);
            dialogue.dialogueType = splitData[8];


            AssetDatabase.CreateAsset(dialogue, $"Assets/ScriptableObjects/Dialogue/{dialogue.dialogueId}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Modifiers")]
    public static void GenerateModifiers()
    {
        string[] allLines = File.ReadAllLines(System.IO.Directory.GetCurrentDirectory() + modifierCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            if (splitData.Length != 5)
            {
                return;
            }

            Modifier modifier = ScriptableObject.CreateInstance<Modifier>();
            modifier.modId = int.Parse(splitData[0]);
            modifier.xpNeeded = int.Parse(splitData[1]);
            modifier.speedMod = float.Parse(splitData[2]);
            modifier.apMod = float.Parse(splitData[3]);
            modifier.totalEnemies = int.Parse(splitData[4]);

            AssetDatabase.CreateAsset(modifier, $"Assets/ScriptableObjects/Modifiers/{modifier.modId}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Upgrades")]
    public static void GenerateUpgrades()
    {
        string[] allLines = File.ReadAllLines(System.IO.Directory.GetCurrentDirectory() + upgradesCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            if (splitData.Length != 5)
            {
                return;
            }

            Upgrades upgrades = ScriptableObject.CreateInstance<Upgrades>();
            upgrades.upgradeId = int.Parse(splitData[0]);
            upgrades.upgradeType = splitData[1];
            upgrades.upgradeLevel = int.Parse(splitData[2]);
            upgrades.upgradeModifier = float.Parse(splitData[3]);
            upgrades.upgradeCost = int.Parse(splitData[4]);
            upgrades.isCompleted = false;
            
            AssetDatabase.CreateAsset(upgrades, $"Assets/ScriptableObjects/Upgrades/{upgrades.upgradeId}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Achievments")]
    public static void GenerateAchievements()
    {

        string[] allLines = File.ReadAllLines(System.IO.Directory.GetCurrentDirectory() + achievementCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            if (splitData.Length != 5)
            {
                return;
            }

            Achievement achievement = ScriptableObject.CreateInstance<Achievement>();
            achievement.achId = int.Parse(splitData[0]);
            achievement.achName = splitData[1];
            achievement.achDesc = splitData[2];
            achievement.achType = splitData[3];
            achievement.achValue = splitData[4];
            achievement.isCompleted = false;
            AssetDatabase.CreateAsset(achievement, $"Assets/ScriptableObjects/Achievement/{achievement.achId}.asset");
        }
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Enemy Spawn")]
    public static void GenerateEnemySpawn()
    {

        string[] allLines = File.ReadAllLines(System.IO.Directory.GetCurrentDirectory() + enemySpawnCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            if (splitData.Length != 2)
            {
                return;
            }

            EnemySpawn enemySpawn = ScriptableObject.CreateInstance<EnemySpawn>();
            enemySpawn.enemySpawnId = int.Parse(splitData[0]);
            enemySpawn.spawnLocation = splitData[1];
            AssetDatabase.CreateAsset(enemySpawn, $"Assets/ScriptableObjects/Enemy Spawn/{enemySpawn.enemySpawnId}.asset");
        }
        AssetDatabase.SaveAssets();
    }
}
