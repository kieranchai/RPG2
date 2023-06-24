using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class EnemyScript : MonoBehaviour
{
    public int enemyId;
    public string enemyName;
    public int maxHealth;
    public float speed;
    public string spritePath;
    public string equippedWeaponName;
    public int currentHealth;

    public Weapon equippedWeapon;

    public Rigidbody2D rb;
    public SpriteRenderer enemySprite;

    [SerializeField] private Enemy testEnemy;

    public ContactFilter2D movementFilter;

    private List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    public float collisionOffset;

    private float timer = 0;
    private float duration = 3f;

    private Vector3 playerLastSeenPos;

    private Vector3 targetDirection;
    private float changeDirectionCooldown;
    private float patrolSpeed;
    private float chaseSpeed;

    private Weapon[] allWeapons;

    public enum EnemyState
    {
        CHASE,
        PATROL,
        ATTACK
    };

    EnemyState currentState = EnemyState.PATROL;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        allWeapons = ShopController.shop.allWeapons;
        SetEnemyData(testEnemy);
        this.currentState = EnemyState.PATROL;
        targetDirection = transform.up;
        changeDirectionCooldown = 1f;
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
            default:
                break;
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
        this.currentHealth = this.maxHealth;

        this.patrolSpeed = this.speed / 3;
        this.chaseSpeed = this.speed;

        enemySprite = gameObject.GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>(this.spritePath);
        enemySprite.sprite = sprite;

        transform.GetChild(0).gameObject.SetActive(true);
        EquipWeapon(Array.Find(allWeapons, weapon => weapon.weaponName == this.equippedWeaponName));
    }

    public void EquipWeapon(Weapon weaponData)
    {
        this.equippedWeapon = weaponData;
        transform.GetChild(0).GetChild(0).GetComponent<EnemyWeaponScript>().SetWeaponData(weaponData);
    }

    public void Patrol()
    {
        //add wandering ai
        this.speed = this.patrolSpeed;
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
            float angleChange = Random.Range(-90f,90f);
            Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
            targetDirection = rotation * targetDirection;
            transform.up = targetDirection;
            changeDirectionCooldown = Random.Range(1, 5);
        }

        //if sees player switch to chase
        if (CheckInSight())
        {
            playerLastSeenPos = PlayerScript.Player.transform.position;
            this.currentState = EnemyState.CHASE;
        }
    }

    public void Chase()
    {
        this.speed = this.chaseSpeed;
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
                Debug.Log("At last seen");
                //walk around
                changeDirectionCooldown -= Time.deltaTime;
                if (changeDirectionCooldown <= 0)
                {
                    float angleChange = Random.Range(90f, 270f);
                    Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
                    targetDirection = rotation * targetDirection;
                    transform.up = targetDirection;
                    changeDirectionCooldown = Random.Range(1, 5);
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
                    Debug.Log("Stuck on wall");
                    //walk around
                    changeDirectionCooldown -= Time.deltaTime;
                    if (changeDirectionCooldown <= 0)
                    {
                        float angleChange = Random.Range(90f, 270f);
                        Quaternion rotation = Quaternion.AngleAxis(angleChange, transform.forward);
                        targetDirection = rotation * targetDirection;
                        transform.up = targetDirection;
                        changeDirectionCooldown = Random.Range(1, 5);
                    }

                }
            }
            timer += Time.deltaTime;
            if (timer >= duration)
            {
                Debug.Log("Gave Up");
                this.currentState = EnemyState.PATROL;
            }
        }
    }

    public void Attacked(int damageTaken)
    {
        if (this.currentHealth - damageTaken >= 1)
        {
            this.currentHealth -= damageTaken;
            this.currentState = EnemyState.CHASE;
        }
        else
        {
            this.currentHealth -= damageTaken;
            Destroy(gameObject);
        }
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

        RaycastHit2D hit = Physics2D.Linecast(transform.position, PlayerScript.Player.transform.position, 1 << LayerMask.NameToLayer("Tilemap Colliders"));
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
