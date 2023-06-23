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

    float timer = 0;
    float duration = 3f;

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

        SetEnemyData(testEnemy);
        this.currentState = EnemyState.PATROL;
    }

    private void Update()
    {
        transform.GetChild(0).gameObject.transform.right = this.transform.up.normalized;
        Debug.Log(currentState);
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

        enemySprite = gameObject.GetComponent<SpriteRenderer>();
        Sprite sprite = Resources.Load<Sprite>(this.spritePath);
        enemySprite.sprite = sprite;

        transform.GetChild(0).gameObject.SetActive(true);
        EquipWeapon(Array.Find(ShopController.shop.allWeapons, weapon => weapon.weaponName == this.equippedWeaponName));
    }

    public void EquipWeapon(Weapon weaponData)
    {
        this.equippedWeapon = weaponData;
        transform.GetChild(0).GetChild(0).GetComponent<EnemyWeaponScript>().SetWeaponData(weaponData);
    }

    public void Patrol()
    {
        //add wandering ai
        
        //if sees player switch to chase
        if (CheckInSight())
        {
            this.currentState = EnemyState.CHASE;
        }
    }

    public void Chase()
    {

        bool success = MoveEnemy(PlayerScript.Player.transform.position - transform.position);
        if (success)
        {
            transform.up = (PlayerScript.Player.transform.position - new Vector3(transform.position.x, transform.position.y));
        }

        if (!success)
        {
            //TRY GO AROUND WALL or just make him rotate and wander around then switch back to patrol
            /*            success = MoveEnemy(new Vector2(1, 0));
                        if (!success)
                        {
                            success = MoveEnemy(new Vector2(-1,0));
                            if (!success)
                            {
                                success = MoveEnemy(new Vector2(0,1));
                                if (!success)
                                {
                                    success = MoveEnemy(new Vector2(0, -1));
                                }
                            }
                        }*/
        }

        if (CheckInSight())
        {
            timer = 0;
            if (CheckInRange())
            {
                this.currentState = EnemyState.ATTACK;
            }
        }

        timer += Time.deltaTime;
        if (timer >= duration)
        {
            this.currentState = EnemyState.PATROL;
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
        if (!CheckInSight() || !CheckInRange())
        {
            this.currentState = EnemyState.CHASE;
        }

        transform.up = (PlayerScript.Player.transform.position - new Vector3(transform.position.x, transform.position.y));
        transform.GetChild(0).GetComponentInChildren<EnemyWeaponScript>().TryAttack();
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
        Debug.DrawRay(transform.position, (PlayerScript.Player.transform.position - transform.position).normalized * equippedWeapon.weaponRange, Color.magenta);
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
