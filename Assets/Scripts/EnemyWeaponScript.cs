using System.Collections;
using UnityEngine;

public class EnemyWeaponScript : MonoBehaviour
{
    public int weaponId;
    public string weaponName;
    public float attackPower;
    public string spritePath;
    public string weaponType;
    public float cooldown;
    public float weaponRange;
    public int maxAmmoCount;
    public float reloadSpeed;

    private bool isReloading;
    private bool limitAttack;

    public int currentAmmoCount;

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        audioSource.volume = AudioManager.sfxVol;
    }

    public void SetWeaponData(Weapon weaponData)
    {
        isReloading = false;
        this.weaponId = weaponData.weaponId;
        this.weaponName = weaponData.weaponName;
        this.attackPower = weaponData.attackPower;
        this.spritePath = weaponData.spritePath;
        this.weaponType = weaponData.weaponType;
        this.cooldown = weaponData.cooldown;
        this.weaponRange = weaponData.weaponRange;
        this.maxAmmoCount = weaponData.ammoCount;
        this.reloadSpeed = weaponData.reloadSpeed;
        this.currentAmmoCount = 0;

        SpriteRenderer weaponSprite = gameObject.GetComponent<SpriteRenderer>();
        Sprite sprite = AssetManager.Assets.GetSprite(this.spritePath);
        weaponSprite.sprite = sprite;
    }

    private void Update()
    {
        if (this.currentAmmoCount == 0 && isReloading == false)
        {
            isReloading = true;
            StartCoroutine(Reload());
        }
    }
    public void TryAttack()
    {
        if (!limitAttack)
        {
            if (!isReloading)
            {
                switch (this.weaponType)
                {
                    case "line":
                        StartCoroutine(LineAttack(this.cooldown, this.weaponRange));
                        break;
                    case "spread":
                        StartCoroutine(SpreadAttack(this.cooldown, this.weaponRange));
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
        yield return null;
    }

    IEnumerator LineAttack(float cooldown, float weaponRange)
    {
        limitAttack = true;
        if (this.currentAmmoCount > 0)
        {
            float spread = Random.Range(-5.0f, 5.0f); // enemy inaccuracy
            audioSource.clip = AssetManager.Assets.GetAudioClip($"{this.weaponName}_Fire");
            audioSource.Play();
            GameObject bullet = Instantiate(AssetManager.Assets.GetPrefab("Enemy Bullet"), transform.position, Quaternion.Euler(0, 0, spread));
            bullet.GetComponent<EnemyBulletScript>().Initialize(this.attackPower, weaponRange);
            //can add Projectile Speed to CSV (600 here)
            bullet.GetComponent<Rigidbody2D>().AddRelativeForce(transform.right * 600);

            --this.currentAmmoCount;
            yield return new WaitForSeconds(cooldown);
        }
        limitAttack = false;
        yield return null;
    }

    IEnumerator SpreadAttack(float cooldown, float weaponRange)
    {
        int count = 5; //bullet count
        limitAttack = true;
        if (this.currentAmmoCount > 0)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3 pos = new Vector3(transform.position.x + Random.Range(-0.2f, 0.2f), transform.position.y, transform.position.z); // random x pos
                float spread = Random.Range(-10.0f, 10.0f);
                GameObject bullet = Instantiate(AssetManager.Assets.GetPrefab("Enemy Bullet"), pos, Quaternion.Euler(0, 0, spread));
                bullet.GetComponent<EnemyBulletScript>().Initialize(this.attackPower, weaponRange);
                bullet.GetComponent<Rigidbody2D>().AddRelativeForce(transform.right * 600);
            }
            audioSource.clip = AssetManager.Assets.GetAudioClip($"{this.weaponName}_Fire");

            audioSource.Play();
            --this.currentAmmoCount;
            PlayerScript.Player.RefreshUI();
            yield return new WaitForSeconds(cooldown);
        }
        limitAttack = false;
        yield return null;
    }
}
