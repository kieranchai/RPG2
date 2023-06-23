using UnityEngine;
using System;
using System.Collections.Generic;

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

    private bool inRange;

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
    }

    private void Update()
    {
        switch (currentState)
        {
            case EnemyState.PATROL:
                Patrol();
                break;
            case EnemyState.CHASE:
                Chase();
                CheckInRange();
                break;
            case EnemyState.ATTACK:
                Attack();
                CheckInRange();
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
    }

    public void Chase()
    {
        transform.up = (PlayerScript.Player.transform.position - new Vector3(transform.position.x, transform.position.y));

        bool success = MoveEnemy(PlayerScript.Player.transform.position - transform.position);
        if (!success)
        {
            Debug.Log("WALL");
            //GO AROUND WALL
        }

        if (inRange)
        {
            this.currentState = EnemyState.ATTACK;
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
        transform.GetChild(0).GetComponentInChildren<EnemyWeaponScript>().TryAttack();

        //need to check if enemy sees player also not just range cos now if u stand beside also considered in range
        if (!inRange)
        {
            this.currentState = EnemyState.CHASE;
        }
    }

    public void CheckInRange()
    {
        if (Vector3.Distance(PlayerScript.Player.transform.position, transform.position) <= equippedWeapon.weaponRange)
        {
            inRange = true;
        } else
        {
            inRange = false;
        }
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
