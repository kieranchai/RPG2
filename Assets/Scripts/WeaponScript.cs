using System.Collections;
using UnityEngine;

public class WeaponScript : MonoBehaviour
{
    public int weaponId;
    public string weaponName;
    public int attackPower;
    public string spritePath;
    public string thumbnailPath;
    public string weaponType;
    public float cooldown;
    public float weaponRange;
    public int cost;
    public int maxAmmoCount;
    public float reloadSpeed;

    private bool isReloading;
    private bool limitAttack;

    public int currentAmmoCount;

    public void SetWeaponData(Weapon weaponData)
    {
        if (isReloading)
        {
            StopAllCoroutines();
        }

        this.weaponId = weaponData.weaponId;
        this.weaponName = weaponData.weaponName;
        this.attackPower = weaponData.attackPower;
        this.spritePath = weaponData.spritePath;
        this.thumbnailPath = weaponData.thumbnailPath;
        this.weaponType = weaponData.weaponType;
        this.cooldown = weaponData.cooldown;
        this.weaponRange = weaponData.weaponRange;
        this.maxAmmoCount = weaponData.ammoCount;
        this.cost = weaponData.cost;
        this.reloadSpeed = weaponData.reloadSpeed;

        // Force the gun to reload in Update upon equipping 
        isReloading = false;
        this.currentAmmoCount = 0;

        SpriteRenderer weaponSprite = gameObject.GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>(this.spritePath);
        weaponSprite.sprite = sprite;
    }

    private void Update()
    {
        if(PlayerScript.Player.equippedWeapon && this.currentAmmoCount == 0 && isReloading == false)
        {
            isReloading = true;
            PlayerScript.Player.ReloadAmmoUI();
            StartCoroutine(Reload());
        }
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
        }
    }

    IEnumerator Reload()
    {
        yield return new WaitForSeconds(this.reloadSpeed);
        this.currentAmmoCount = this.maxAmmoCount;
        isReloading = false;
        PlayerScript.Player.RefreshUI();
        yield return null;
    }

    IEnumerator LineAttack(float cooldown, float weaponRange)
    {
        limitAttack = true;
        if (this.currentAmmoCount > 0)
        {
            GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"), transform.position, transform.rotation);
            bullet.GetComponent<BulletScript>().Initialize(this.attackPower, weaponRange);

            //can add Projectile Speed to CSV (600 here)
            bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * 600);

            --this.currentAmmoCount;
            PlayerScript.Player.RefreshUI();
            yield return new WaitForSeconds(cooldown);
        }
        limitAttack = false;
        yield return null;
    }
}
