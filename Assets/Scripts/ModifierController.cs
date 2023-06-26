using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifierController : MonoBehaviour
{
    public static ModifierController Modifier { get; private set; }
    public float apMod;
    public float speedMod;
    public int respectLvl;
    public int xpNeeded;
    public Modifier[] allMods;
    private void Awake()
    {
        if (Modifier != null && Modifier != this)
        {
            Destroy(gameObject);
            return;
        }
        Modifier = this;
    }

    private void Start() {
        apMod = 1;
        speedMod = 1;
        respectLvl = 1;
        xpNeeded = 100;
        allMods = Resources.LoadAll<Modifier>("ScriptableObjects/Modifiers");
    }
    public void UpdateModifiers() {
        foreach (Modifier mod in allMods) {
            if (PlayerScript.Player.playerLvl == mod.modId) {
                this.apMod = mod.apMod;
                this.speedMod = mod.speedMod;
                this.xpNeeded = mod.xpNeeded;
            }else Debug.Log ("max lvl hit");
        }
    }
}
