using System.Collections;
using UnityEngine;

// KIERAN AND JOEL

public class WeaponScript : MonoBehaviour
{
    public int weaponId;
    public string weaponName;
    public float attackPower;
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

    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        audioSource.volume = AudioManager.sfxVol;
    }

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
        Sprite sprite = AssetManager.Assets.GetSprite(this.spritePath);
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
                    case "spread":
                        StartCoroutine(SpreadAttack(this.cooldown, this.weaponRange));
                        break;
                    case "explosive":
                        StartCoroutine(ExplosiveAttack(this.cooldown, this.weaponRange));
                        break;
                    default:
                        break;
                }
            }
        }
    }

    IEnumerator Reload()
    {
        audioSource.clip = AssetManager.Assets.GetAudioClip("Reload");
        audioSource.Play();
        yield return new WaitForSeconds(this.reloadSpeed);
        audioSource.Stop();
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
            GameObject bullet = Instantiate(AssetManager.Assets.GetPrefab("Bullet"), transform.position, transform.rotation);
            audioSource.clip = AssetManager.Assets.GetAudioClip($"{this.weaponName}_Fire");
            audioSource.Play();
            bullet.GetComponent<BulletScript>().Initialize(this.attackPower * ModifierController.Modifier.apMod, weaponRange);
            //can add Projectile Speed to CSV (600 here)
            bullet.GetComponent<Rigidbody2D>().AddForce(transform.right * 600);

            --this.currentAmmoCount;
            PlayerScript.Player.RefreshUI();
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
                GameObject bullet = Instantiate(AssetManager.Assets.GetPrefab("Bullet"), pos, Quaternion.Euler(0, 0, spread));
                bullet.GetComponent<BulletScript>().Initialize(this.attackPower, weaponRange);
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

    IEnumerator ExplosiveAttack(float cooldown, float weaponRange)
    {
        limitAttack = true;
        if (this.currentAmmoCount > 0)
        {
            GameObject rocket = Instantiate(AssetManager.Assets.GetPrefab("Rocket"), transform.position, transform.rotation);
            rocket.GetComponent<RocketScript>().Initialize(this.attackPower, weaponRange);
            audioSource.clip = AssetManager.Assets.GetAudioClip($"{this.weaponName}_Fire");
            audioSource.Play();
            //can add Projectile Speed to CSV (600 here)
            rocket.GetComponent<Rigidbody2D>().AddForce(transform.right * 600);
            --this.currentAmmoCount;
            PlayerScript.Player.RefreshUI();
            yield return new WaitForSeconds(cooldown);
        }
        limitAttack = false;
        yield return null;
    }

}
