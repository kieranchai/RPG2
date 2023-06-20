using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public int weaponId;
    public string weaponName;
    public int attackPower;

    private void Awake()
    {
        this.weaponId = 0;
        this.weaponName = "HANDS";
        this.attackPower = 1;
    }

    public void setWeaponData(Weapon weaponData)
    {
        this.weaponId = weaponData.weaponId;
        this.weaponName = weaponData.weaponName;
        this.attackPower = weaponData.attackPower;
    }

    public void doAttack()
    {

        Debug.Log("Damage for " + this.attackPower);
    }
}
