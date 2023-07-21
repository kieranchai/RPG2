using UnityEngine;
using System;

// KIERAN AND JOEL

public class ModifierController : MonoBehaviour
{
    public static ModifierController Modifier { get; private set; }
    public float apMod;
    public float speedMod;
    public int xpNeeded;
    public int totalEnemies;
    public Modifier[] allMods;

    public bool isMaxLvl = false;

    private void Awake()
    {
        if (Modifier != null && Modifier != this)
        {
            Destroy(gameObject);
            return;
        }
        Modifier = this;
    }

    private void Start()
    {
        allMods = AssetManager.Assets.allModifiers.ToArray();
        Array.Sort(allMods, (a,b)=>a.modId-b.modId);
        this.apMod = allMods[0].apMod;
        this.speedMod = allMods[0].speedMod;
        this.xpNeeded = allMods[0].xpNeeded;
        this.totalEnemies = allMods[0].totalEnemies;
    }

    public void UpdateModifiers()
    {
        for(int i = 0; i < allMods.Length; i++)
        {
            if (PlayerScript.Player.playerLvl == allMods[i].modId)
            {
                this.apMod = allMods[i].apMod;
                this.speedMod = allMods[i].speedMod;
                this.totalEnemies = allMods[i].totalEnemies;
                this.xpNeeded = allMods[i].xpNeeded;
            } else if (PlayerScript.Player.playerLvl == allMods[^1].modId)
            {
                this.apMod = allMods[i].apMod;
                this.speedMod = allMods[i].speedMod;
                this.totalEnemies = allMods[i].totalEnemies;
                this.isMaxLvl = true;
            }
        }
    }
}
