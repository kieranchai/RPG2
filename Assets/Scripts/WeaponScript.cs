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

    private bool isCooldown;

    private void Awake()
    {
        this.weaponId = 0;
        this.weaponName = "HANDS";
        this.attackPower = 1;
        this.spritePath = "Sprites/Fists";

        SpriteRenderer weaponSprite = gameObject.GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>(this.spritePath);
        weaponSprite.sprite = sprite;

        RefreshColliders();
    }

    public void SetWeaponData(Weapon weaponData)
    {
        this.weaponId = weaponData.weaponId;
        this.weaponName = weaponData.weaponName;
        this.attackPower = weaponData.attackPower;

        //tentative
        if (this.weaponName == "pistol")
        {
            this.spritePath = "Sprites/Desert Eagle";
            SpriteRenderer weaponSprite = gameObject.GetComponent<SpriteRenderer>();
            Sprite sprite = Resources.Load<Sprite>(this.spritePath);
            weaponSprite.sprite = sprite;
            this.weaponType = "ranged";
        }

        RefreshColliders();
    }

    public void TryAttack()
    {
        if (!isCooldown)
        {
            switch (this.weaponType)
            {
                case "ranged":
                    StartCoroutine(RangedAttack());
                    break;
                default:
                    StartCoroutine(ForwardAttack());
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

    IEnumerator ForwardAttack()
    {
        isCooldown = true;
        Vector3 initialPosition = transform.localPosition;
        float elapsedTime = 0;

        // weapon cooldown
        float waitTime = 0.3f;

        var pos = transform.localPosition;

        // weapon range
        pos.x += 0.5f;

        while (elapsedTime < waitTime)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, pos, (elapsedTime / waitTime));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        transform.localPosition = initialPosition;
        isCooldown = false;
        yield return null;
    }

    IEnumerator RangedAttack()
    {
        isCooldown = true;

        // weapon cooldown
        float waitTime = 0.5f;

        GameObject bullet = Instantiate(Resources.Load<GameObject>("Prefabs/Bullet"), transform.position, Quaternion.identity);
        //weapon range
        bullet.GetComponent<BulletScript>().Initialize(this.attackPower, 5);
        //projectile speed
        bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * 300);

        yield return new WaitForSeconds(waitTime);

        isCooldown = false;
        yield return null;
    }
}
