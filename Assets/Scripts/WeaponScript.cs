using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public int weaponId;
    public string weaponName;
    public int attackPower;
    public string spritePath;
    public string weaponType;
    public float cooldown;
    public float weaponRange;
    public int maxAmmoCount;

    private bool isReloading = false;
    private bool limitAttack;

    public int currentAmmoCount;

    public void SetWeaponData(Weapon weaponData)
    {
        this.weaponId = weaponData.weaponId;
        this.weaponName = weaponData.weaponName;
        this.attackPower = weaponData.attackPower;
        this.spritePath = weaponData.spritePath;
        this.weaponType = weaponData.weaponType;
        this.cooldown = weaponData.cooldown;
        this.weaponRange = weaponData.weaponRange;
        this.maxAmmoCount = weaponData.ammoCount;
        this.currentAmmoCount = maxAmmoCount;

        SpriteRenderer weaponSprite = gameObject.GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>(this.spritePath);
        weaponSprite.sprite = sprite;

        RefreshColliders();
    }

    public void TryAttack()
    {
        if (!limitAttack && PlayerScript.Player.equippedWeapon)
        {
            if (!isReloading)
            {
                switch (this.weaponType)
                {
                    case "line":
                        StartCoroutine(LineAttack(this.cooldown, this.weaponRange));
                        break;
                    default:
                        break;
                }
            }
            else
            {
                StartCoroutine(Reload());
            }
        }
    }

    private void RefreshColliders()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
        StartCoroutine(SetColliderTrigger());

    }

    IEnumerator SetColliderTrigger()
    {
        yield return new WaitForFixedUpdate();
        GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    IEnumerator Reload()
    {
        this.currentAmmoCount = this.maxAmmoCount;
        //can add reload speed 5f
        yield return new WaitForSeconds(5f);
        isReloading = false;
        yield return null;
    }

    IEnumerator LineAttack(float cooldown, float weaponRange)
    {
        limitAttack = true;
        if (this.currentAmmoCount > 0)
        {
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"), transform.position, Quaternion.identity);
            bullet.GetComponent<BulletScript>().Initialize(this.attackPower, weaponRange);

            //can add Projectile Speed to CSV (600 here)
            bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * 600);

            --this.currentAmmoCount;
            yield return new WaitForSeconds(cooldown);
        }
        else
        {
            isReloading = true;
        }
        limitAttack = false;
        yield return null;
    }
}
