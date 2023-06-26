using UnityEditor;
using UnityEngine;
using System.IO;

public class CSVtoSO
{
    private static string characterCSVPath = "/Editor/CSVs/CharacterCSV.csv";
    private static string enemyCSVPath = "/Editor/CSVs/EnemyCSV.csv";
    private static string weaponCSVPath = "/Editor/CSVs/WeaponCSV.csv";
    private static string questCSVPath = "/Editor/CSVs/QuestCSV.csv";
    private static string dialogueCSVPath = "/Editor/CSVs/DialogueCSV.csv";
    private static string modifierCSVPath = "/Editor/CSVs/ModifierCSV.csv";

    [MenuItem("Utilities/Generate Characters")]
    public static void GenerateCharacters()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + characterCSVPath);

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
            character.maxHealth = int.Parse(splitData[2]);
            character.characterName = splitData[3];
            character.spritePath = splitData[4];

            AssetDatabase.CreateAsset(character, $"Assets/Resources/ScriptableObjects/Characters/{character.characterName}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Enemies")]
    public static void GenerateEnemies()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + enemyCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            if (splitData.Length != 6)
            {
                return;
            }

            Enemy enemy = ScriptableObject.CreateInstance<Enemy>();
            enemy.enemyId = int.Parse(splitData[0]);
            enemy.speed = float.Parse(splitData[1]);
            enemy.maxHealth = int.Parse(splitData[2]);
            enemy.enemyName = splitData[3];
            enemy.spritePath = splitData[4];
            enemy.equippedWeaponName = splitData[5];

            AssetDatabase.CreateAsset(enemy, $"Assets/Resources/ScriptableObjects/Enemies/{enemy.enemyName}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Weapons")]
    public static void GenerateWeapons()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + weaponCSVPath);

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
            weapon.attackPower = int.Parse(splitData[2]);
            weapon.spritePath = splitData[3];
            weapon.thumbnailPath = splitData[4];
            weapon.weaponType = splitData[5];
            weapon.cooldown = float.Parse(splitData[6]);
            weapon.weaponRange = float.Parse(splitData[7]);
            weapon.ammoCount = int.Parse(splitData[8]);
            weapon.cost = int.Parse(splitData[9]);
            weapon.reloadSpeed = float.Parse(splitData[10]);

            AssetDatabase.CreateAsset(weapon, $"Assets/Resources/ScriptableObjects/Weapons/{weapon.weaponName}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Quests")]
    public static void GenerateQuests()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + questCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            if (splitData.Length != 5)
            {
                return;
            }

            Quest quest = ScriptableObject.CreateInstance<Quest>();
            quest.questId = int.Parse(splitData[0]);
            quest.questType = splitData[1];
            quest.questAmount = int.Parse(splitData[2]);
            quest.questObject = splitData[3];
            quest.questReward = int.Parse(splitData[4]);


            AssetDatabase.CreateAsset(quest, $"Assets/Resources/ScriptableObjects/Quests/{quest.questId}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Dialogue")]
    public static void GenerateDialogue()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + dialogueCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            if (splitData.Length != 8)
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


            AssetDatabase.CreateAsset(dialogue, $"Assets/Resources/ScriptableObjects/Dialogue/{dialogue.dialogueId}.asset");
        }

        AssetDatabase.SaveAssets();
    }

    [MenuItem("Utilities/Generate Modifiers")]
    public static void GenerateModifiers()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + modifierCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(',');

            if (splitData.Length != 4)
            {
                return;
            }

            Modifier modifier = ScriptableObject.CreateInstance<Modifier>();
            modifier.modId = int.Parse(splitData[0]);
            modifier.xpNeeded = int.Parse(splitData[1]);
            modifier.speedMod = float.Parse(splitData[2]);
            modifier.apMod = float.Parse(splitData[3]);

            AssetDatabase.CreateAsset(modifier, $"Assets/Resources/ScriptableObjects/Modifiers/{modifier.modId}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}
