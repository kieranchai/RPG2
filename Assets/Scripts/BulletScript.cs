using UnityEngine;

// KIERAN AND JOEL

public class BulletScript : MonoBehaviour
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyScript>().Attacked(this.attackPower);
            Destroy(gameObject);
        }

        if (collision.gameObject.CompareTag("Tilemap Colliders") || collision.gameObject.CompareTag("Safe Area"))
        {
            Destroy(gameObject);
        }
    }
}
