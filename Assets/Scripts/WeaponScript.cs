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

    private bool isCooldown;

    private void Start()
    {
       SetWeaponData(Resources.Load<Weapon>("ScriptableObjects/Weapons/hands"));
       RefreshColliders();
    }

    public void SetWeaponData(Weapon weaponData)
    {
        this.weaponId = weaponData.weaponId;
        this.weaponName = weaponData.weaponName;
        this.attackPower = weaponData.attackPower;
        this.spritePath = weaponData.spritePath;
        this.weaponType = weaponData.weaponType;
        this.cooldown = weaponData.cooldown;
        this.weaponRange = weaponData.weaponRange;

        SpriteRenderer weaponSprite = gameObject.GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>(this.spritePath);
        weaponSprite.sprite = sprite;

        RefreshColliders();
    }

    public void TryAttack()
    {
        if (!isCooldown)
        {
            switch (this.weaponType)
            {
                case "melee_forward":
                    StartCoroutine(MeleeForwardAttack(this.cooldown, this.weaponRange));
                    break;
                case "ranged":
                    StartCoroutine(RangedAttack(this.cooldown, this.weaponRange));
                    break;
                default:
                    break;
            }
        }
    }

    private void RefreshColliders()
    {
        Destroy(GetComponent<PolygonCollider2D>());
        gameObject.AddComponent<PolygonCollider2D>();
        GetComponent<PolygonCollider2D>().isTrigger = true;
    }

    IEnumerator MeleeForwardAttack(float cooldown, float weaponRange)
    {
        isCooldown = true;
        Vector3 initialPosition = transform.localPosition;
        float elapsedTime = 0;
        var pos = transform.localPosition;
        pos.x += weaponRange;

        while (elapsedTime < cooldown)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, pos, (elapsedTime / cooldown));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = initialPosition;
        isCooldown = false;
        yield return null;
    }

    IEnumerator RangedAttack(float cooldown, float weaponRange)
    {
        isCooldown = true;
        GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"), transform.position, Quaternion.identity);
        bullet.GetComponent<BulletScript>().Initialize(this.attackPower, weaponRange);
        //can add Projectile Speed to CSV (600 here)
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * 600);
        yield return new WaitForSeconds(cooldown);
        isCooldown = false;
        yield return null;
    }
}
