using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.AddressableAssets;

public class AssetManager : MonoBehaviour
{
    public static AssetManager Assets { get; private set; }

    [SerializeField] private Scrollbar scroll;
    [SerializeField] private int stallTime = 2;

    public List<Character> allCharacters;
    public List<Weapon> allWeapons;
    public List<Dialogue> allDialogue;
    public List<Quest> allQuests;
    public List<Enemy> allEnemies;
    public List<EnemySpawn> allEnemySpawns;
    public List<Achievement> allAchievements;
    public List<Modifier> allModifiers;
    public List<Upgrades> allUpgrades;

    [SerializeField] private List<Sprite> characterSprites;
    [SerializeField] private List<Sprite> uiSprites;
    [SerializeField] private List<Sprite> weaponSprites;
    [SerializeField] private List<AudioClip> allAudioClips;
    [SerializeField] private List<GameObject> allPrefabs;

    private readonly int totalAssetsToLoad = 14;

    void Awake()
    {
        if (Assets != null && Assets != this)
        {
            StartCoroutine(FakeLoad());
            Destroy(gameObject);
            return;
        }
        Assets = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        StartCoroutine(LoadAssets());
    }

    IEnumerator LoadAssets()
    {
        int isDone = 0;

        var loadCharacter = Addressables.LoadAssetsAsync<Character>("Characters", (character) =>
        {
            allCharacters.Add(character);
        });

        loadCharacter.Completed += (operation) =>
        {
            ++isDone;
            scroll.size += (float) 1 / totalAssetsToLoad;
        };

        var loadWeapon = Addressables.LoadAssetsAsync<Weapon>("Weapons", (weapon) =>
        {
            allWeapons.Add(weapon);
        });

        loadWeapon.Completed += (operation) =>
        {
            ++isDone;
            scroll.size += (float) 1 / totalAssetsToLoad;
        };

        var loadDialogue = Addressables.LoadAssetsAsync<Dialogue>("Dialogue", (dialogue) =>
        {
            allDialogue.Add(dialogue);
        });

        loadDialogue.Completed += (operation) =>
        {
            ++isDone;
            scroll.size += (float) 1 / totalAssetsToLoad;
        };

        var loadQuest = Addressables.LoadAssetsAsync<Quest>("Quests", (quest) =>
        {
            allQuests.Add(quest);
        });

        loadQuest.Completed += (operation) =>
        {
            ++isDone;
            scroll.size += (float) 1 / totalAssetsToLoad;
        };

        var loadEnemy = Addressables.LoadAssetsAsync<Enemy>("Enemies", (enemy) =>
        {
            allEnemies.Add(enemy);
        });

        loadEnemy.Completed += (operation) =>
        {
            ++isDone;
            scroll.size += (float) 1 / totalAssetsToLoad;
        };

        var loadEnemySpawn = Addressables.LoadAssetsAsync<EnemySpawn>("EnemySpawns", (enemySpawn) =>
        {
            allEnemySpawns.Add(enemySpawn);
        });

        loadEnemySpawn.Completed += (operation) =>
        {
            ++isDone;
            scroll.size += (float) 1 / totalAssetsToLoad;
        };

        var loadAchievement = Addressables.LoadAssetsAsync<Achievement>("Achievements", (achievement) =>
        {
            allAchievements.Add(achievement);
        });

        loadAchievement.Completed += (operation) =>
        {
            ++isDone;
            scroll.size += (float) 1 / totalAssetsToLoad;
        };

        var loadModifier = Addressables.LoadAssetsAsync<Modifier>("Modifiers", (modifier) =>
        {
            allModifiers.Add(modifier);
        });

        loadModifier.Completed += (operation) =>
        {
            ++isDone;
            scroll.size += (float) 1 / totalAssetsToLoad;
        };

        var loadUpgrade = Addressables.LoadAssetsAsync<Upgrades>("Upgrades", (upgrade) =>
        {
            allUpgrades.Add(upgrade);
        });

        loadUpgrade.Completed += (operation) =>
        {
            ++isDone;
            scroll.size += (float) 1 / totalAssetsToLoad;
        };

        var loadCharacterSprites = Addressables.LoadAssetsAsync<Sprite>("CharacterSprites", (sprite) =>
        {
            characterSprites.Add(sprite);
        });

        loadCharacterSprites.Completed += (operation) =>
        {
            ++isDone;
            scroll.size += (float)1 / totalAssetsToLoad;
        };

        var loadWeaponSprites = Addressables.LoadAssetsAsync<Sprite>("WeaponSprites", (sprite) =>
        {
            weaponSprites.Add(sprite);
        });

        loadWeaponSprites.Completed += (operation) =>
        {
            ++isDone;
            scroll.size += (float)1 / totalAssetsToLoad;
        };

        var loadUISprites = Addressables.LoadAssetsAsync<Sprite>("UISprites", (sprite) =>
        {
            uiSprites.Add(sprite);
        });

        loadUISprites.Completed += (operation) =>
        {
            ++isDone;
            scroll.size += (float)1 / totalAssetsToLoad;
        };

        var loadPrefabs = Addressables.LoadAssetsAsync<GameObject>("Prefabs", (gameobject) =>
        {
            allPrefabs.Add(gameobject);
        });

        loadPrefabs.Completed += (operation) =>
        {
            ++isDone;
            scroll.size += (float)1 / totalAssetsToLoad;
        };

        var loadAudioClips = Addressables.LoadAssetsAsync<AudioClip>("AudioClips", (audioclip) =>
        {
            allAudioClips.Add(audioclip);
        });

        loadAudioClips.Completed += (operation) =>
        {
            ++isDone;
            scroll.size += (float)1 / totalAssetsToLoad;
        };


        yield return new WaitUntil(() => isDone == totalAssetsToLoad);
        yield return new WaitForSeconds(stallTime);
        SceneManager.LoadScene("Character Selection");
    }

    public Sprite GetSprite(string spritePath)
    {
        string[] formatPath = spritePath.Split('/');
        switch (formatPath[0])
        {
            case "CharacterSprites":
                return characterSprites.Find(x => x.name == formatPath[1]);
            case "WeaponSprites":
                return weaponSprites.Find(x => x.name == formatPath[1]);
            case "UISprites":
                return uiSprites.Find(x => x.name == formatPath[1]);
            default:
                break;
        };

        return null;
    }

    public AudioClip GetAudioClip(string audioName)
    {
        return allAudioClips.Find(x => x.name == audioName);
    }

    public GameObject GetPrefab(string prefabName)
    {
        return allPrefabs.Find(x => x.name == prefabName);
    }

    IEnumerator FakeLoad()
    {
        float timer = 0;
        while (timer < stallTime)
        {
            timer += Time.deltaTime;
            scroll.size = (timer / stallTime);
            yield return null;
        }
        SceneManager.LoadScene("Character Selection");
    }
}
