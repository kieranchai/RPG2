using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EnemyScript : MonoBehaviour
{
    public int enemyId;
    public string enemyName;
    public float maxHealth;
    public float speed;
    public string spritePath;
    public string equippedWeaponName;
    public float currentHealth;
    public int xpDrop;

    public Weapon equippedWeapon;

    public Animator anim;
    public Rigidbody2D rb;
    public SpriteRenderer enemySprite;
    [SerializeField] private ParticleSystem bloodParticles;

    public ContactFilter2D movementFilter;

    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public float collisionOffset;

    private float timer = 0;
    private float duration = 2f;

    private Vector3 playerLastSeenPos;

    private Vector3 targetDirection;
    private float changeDirectionCooldown;

    private Weapon[] allWeapons;

    private EnemyWeaponScript currWeapon;
    private Collider2D enemyColl;

    public enum EnemyState
    {
        CHASE,
        PATROL,
        ATTACK,
        DIED
    };

    EnemyState currentState = EnemyState.PATROL;

    private int weightedAmt = 0;
    public class Loot
    {
        public int cashAmt;
        public int weight;
        public Loot(int cashAmt, int weight)
        {
            this.cashAmt = cashAmt;
            this.weight = weight;
        }
    }
    List<Loot> availLoot = new List<Loot>();
    private string[] availLootString;

    [SerializeField] private AudioSource audioSource;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        enemyColl = GetComponent<Collider2D>();
        enemySprite = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        allWeapons = ShopController.shop.allWeapons;
        currWeapon = transform.GetChild(0).GetChild(0).GetComponent<EnemyWeaponScript>();
        SetEnemyData(Resources.LoadAll<Enemy>("ScriptableObjects/Enemies")[Random.Range(0, Resources.LoadAll<Enemy>("ScriptableObjects/Enemies").Length)]);
        audioSource.volume = 0.15f;
        this.currentState = EnemyState.PATROL;
        targetDirection = transform.up;
        changeDirectionCooldown = 0.5f;
        anim.SetBool("isWalking", true);
    }

    private void Update()
    {
        transform.GetChild(0).gameObject.transform.right = this.transform.up.normalized;
        switch (currentState)
        {
            case EnemyState.PATROL:
                Patrol();
                break;
            case EnemyState.CHASE:
                Chase();
                break;
            case EnemyState.ATTACK:
                Attack();
                break;
            case EnemyState.DIED:
                Died();
                break;
            default:
                break;
        }
    }

    private void LateUpdate()
    {
        SkinChoice();
    }

    public void SkinChoice()
    {
        if (enemySprite.sprite.name.Contains("Main"))
        {
            string spriteName = enemySprite.sprite.name;
            spriteName = spriteName.Replace("Main", enemyName);
            enemySprite.sprite = Resources.Load<Sprite>("Sprites/" + spriteName);
        }
    }

    public void SetEnemyData(Enemy enemyData)
    {
        this.enemyId = enemyData.enemyId;
        this.enemyName = enemyData.enemyName;
        this.maxHealth = enemyData.maxHealth;
        this.speed = enemyData.speed;
        this.spritePath = enemyData.spritePath;
        this.equippedWeaponName = enemyData.equippedWeaponName;
        this.xpDrop = enemyData.xpDrop;

        this.currentHealth = this.maxHealth;

        this.availLootString = enemyData.cashDrop.Split('#');
        for (int i = 0; i < this.availLootString.Length; i++)
        {
            string[] formattedAvailLootString = this.availLootString[i].Split('@');
            this.availLoot.Add(new Loot(int.Parse(formattedAvailLootString[0]), int.Parse(formattedAvailLootString[1])));
            this.weightedAmt += int.Parse(formattedAvailLootString[1]);
        }
        Sprite sprite = Resources.Load<Sprite>(this.spritePath);
        enemySprite.sprite = sprite;

        transform.GetChild(0).gameObject.SetActive(true);
        EquipWeapon(Array.Find(allWeapons, weapon => weapon.weaponName == this.equippedWeaponName));
    }

    public void EquipWeapon(Weapon weaponData)
    {
        this.equippedWeapon = weaponData;
        currWeapon.SetWeaponData(weaponData);
    }

    public void Patrol()
    {
        bool success = MoveEnemy(targetDirection);
        if (!success)
        {
            float angleChange = Random.Range(90f, 270f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            targetDirection = rotation * targetDirection;
            transform.up = targetDirection;
        }

        changeDirectionCooldown -= Time.deltaTime;
        if (changeDirectionCooldown <= 0)
        {
            float angleChange = Random.Range(-90f, 90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            targetDirection = rotation * targetDirection;
            transform.up = targetDirection;
            changeDirectionCooldown = Random.Range(1, 2);
        }

        //if sees player switch to chase
        if (CheckInSight())
        {
            playerLastSeenPos = PlayerScript.Player.transform.position;
            audioSource.clip = (AudioClip)Resources.Load($"Audio Clips/{this.enemyName}_Chase");
            audioSource.Play();
            this.currentState = EnemyState.CHASE;
        }
    }

    public void Chase()
    {
        if (CheckInSight())
        {
            playerLastSeenPos = PlayerScript.Player.transform.position;

            timer = 0;
            bool success = MoveEnemy(PlayerScript.Player.transform.position - transform.position);
            if (success)
            {
                transform.up = (PlayerScript.Player.transform.position - new Vector3(transform.position.x, transform.position.y));
            }

            if (CheckInRange())
            {
                this.currentState = EnemyState.ATTACK;
            }
        }
        else
        {
            if (Vector3.Distance(playerLastSeenPos, transform.position) < 0.05f)
            {
                //walk around
                changeDirectionCooldown -= Time.deltaTime;
                if (changeDirectionCooldown <= 0)
                {
                    float angleChange = Random.Range(90f, 270f);
                    Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
                    targetDirection = rotation * targetDirection;
                    transform.up = targetDirection;
                    changeDirectionCooldown = Random.Range(1, 2);
                }
            }
            else
            {
                bool success = MoveEnemy(playerLastSeenPos - transform.position);
                if (success)
                {
                    transform.up = (playerLastSeenPos - new Vector3(transform.position.x, transform.position.y));
                }
                else
                {
                    //walk around
                    changeDirectionCooldown -= Time.deltaTime;
                    if (changeDirectionCooldown <= 0)
                    {
                        float angleChange = Random.Range(90f, 270f);
                        Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
                        targetDirection = rotation * targetDirection;
                        transform.up = targetDirection;
                        changeDirectionCooldown = Random.Range(1, 2);
                    }

                }
            }
            timer += Time.deltaTime;
            if (timer >= duration)
            {
                audioSource.clip = (AudioClip)Resources.Load($"Audio Clips/{this.enemyName}_Patrol");
                audioSource.Play();

                this.currentState = EnemyState.PATROL;
            }
        }
    }

    public void Attacked(float damageTaken)
    {
        playerLastSeenPos = PlayerScript.Player.transform.position;
        AnalyticsController.Analytics.damageDealt += damageTaken;
        AnalyticsController.Analytics.CheckAchievements("DAMAGEDEALT");
        bloodParticles.Play();
        if (this.currentHealth - damageTaken > 0)
        {
            this.currentHealth -= damageTaken;

            audioSource.clip = (AudioClip)Resources.Load($"Audio Clips/{this.enemyName}_Hit");
            audioSource.Play();

            this.currentState = EnemyState.CHASE;
        }
        else
        {
            this.currentHealth -= damageTaken;

            audioSource.clip = (AudioClip)Resources.Load($"Audio Clips/{this.enemyName}_Death");
            audioSource.Play();

            EnemyDeath();
        }
    }

    public void EnemyDeath()
    {
        AnalyticsController.Analytics.enemiesKilled++;
        Spawner.currentEnemies--;

        //can instantiate money on ground oso then pick up
        int randomWeight = Random.Range(0, this.weightedAmt);
        for (int i = 0; i < availLoot.Count; i++)
        {
            randomWeight -= availLoot[i].weight;
            if (randomWeight < 0)
            {
                PlayerScript.Player.cash += availLoot[i].cashAmt;
                PlayerScript.Player.UpdateCash(availLoot[i].cashAmt);
                break;
            }
        }
        PlayerScript.Player.UpdateExperience(this.xpDrop);
        AnalyticsController.Analytics.CheckAchievements("KILL");

        this.currentState = EnemyState.DIED;
    }

    public void Died()
    {
        anim.enabled = false;
        enemyColl.enabled = false;
        enemySprite.sprite = Resources.Load<Sprite>($"Sprites/{this.enemyName}_Death");
        Destroy(gameObject, 4f);
    }

    public void Attack()
    {
        playerLastSeenPos = PlayerScript.Player.transform.position;
        if (!CheckInRange() || !CheckInSight())
        {
            this.currentState = EnemyState.CHASE;
        }

        transform.up = (PlayerScript.Player.transform.position - new Vector3(transform.position.x, transform.position.y));
        transform.GetChild(0).GetComponentInChildren<EnemyWeaponScript>().TryAttack();
        bool success = MoveEnemy(PlayerScript.Player.transform.position - transform.position);
        if (success)
        {
            transform.up = (PlayerScript.Player.transform.position - new Vector3(transform.position.x, transform.position.y));
        }

    }

    public bool CheckInRange()
    {
        if (Vector3.Distance(PlayerScript.Player.transform.position, transform.position) <= equippedWeapon.weaponRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool CheckInSight()
    {
        bool inRange = Vector3.Distance(PlayerScript.Player.transform.position, transform.position) <= equippedWeapon.weaponRange;
        float sightAngle = Vector2.Angle(PlayerScript.Player.transform.position - transform.position, transform.up);

        int mask1 = 1 << LayerMask.NameToLayer("Tilemap Colliders");
        int mask2 = 1 << LayerMask.NameToLayer("Safe Area");
        int combinedMask = mask1 | mask2;

        RaycastHit2D hit = Physics2D.Linecast(transform.position, PlayerScript.Player.transform.position, combinedMask);
        if (hit.collider != null)
        {
            return false;
        }

        return inRange && (sightAngle < 30f);
    }

    public bool MoveEnemy(Vector2 direction)
    {
        direction.Normalize();
        int count = rb.Cast(
            direction,
            movementFilter,
            castCollisions,
            speed * Time.fixedDeltaTime + collisionOffset);

        if (count == 0)
        {
            Vector2 moveVector = direction * speed * Time.fixedDeltaTime;

            rb.MovePosition(rb.position + moveVector);
            return true;
        }
        else
        {
            return false;
        }
    }

}
