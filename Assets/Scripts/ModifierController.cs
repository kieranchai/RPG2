using UnityEngine;
using System;

public class ModifierController : MonoBehaviour
{
    public static ModifierController Modifier { get; private set; }
    public float apMod;
    public float speedMod;
    public int respectLvl;
    public int xpNeeded;
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
        apMod = 1;
        speedMod = 1;
        respectLvl = 1;
        xpNeeded = 100;
        allMods = Resources.LoadAll<Modifier>("ScriptableObjects/Modifiers");
        Array.Sort(allMods, (a,b)=>a.modId-b.modId);
    }

    public void UpdateModifiers()
    {
        for(int i = 0; i < allMods.Length; i++)
        {
            if (PlayerScript.Player.playerLvl == allMods[i].modId)
            {
                this.apMod = allMods[i].apMod;
                this.speedMod = allMods[i].speedMod;
                this.xpNeeded = allMods[i].xpNeeded;
            } else if (PlayerScript.Player.playerLvl == allMods[allMods.Length-1].modId)
            {
                this.isMaxLvl = true;
            }
        }
    }
}
