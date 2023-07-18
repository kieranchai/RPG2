using UnityEngine;

// KIERAN AND JOEL

public class EnemyBulletScript : MonoBehaviour
{
    private float attackPower;
    private float weaponRange;
    private Vector3 initialPosition;

    public void Initialize(float attackPower, float weaponRange)
    {
        this.attackPower = attackPower;
        this.weaponRange = weaponRange;
        this.initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if ((transform.position - initialPosition).magnitude >= this.weaponRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<PlayerScript>().TakeDamage(this.attackPower);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Tilemap Colliders") || collision.gameObject.CompareTag("Safe Area"))
        {
            Destroy(gameObject);
        }
    }
}
