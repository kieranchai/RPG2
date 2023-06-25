
using UnityEngine;

public class RocketScript : MonoBehaviour
{
    private int attackPower;
    private float weaponRange;
    private Vector3 initialPosition;
    public float splashRange;

    public void Initialize(int attackPower, float weaponRange)
    {
        this.attackPower = attackPower;
        this.weaponRange = weaponRange;
        this.initialPosition = transform.localPosition;
    }
    private void Explode()
    {
        GameObject explosion = Instantiate(Resources.Load<GameObject>("Prefabs/Explosion"), transform.position, transform.rotation);
        explosion.GetComponent<ExplosionScript>().Initialize(attackPower);
        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if ((transform.position - initialPosition).magnitude >= this.weaponRange)
        {
            Explode();

        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // collision.gameObject.GetComponent<EnemyScript>().Attacked(this.attackPower);
            Explode();
        }

        if (collision.gameObject.tag == "Tilemap Colliders")
        {
            Explode();
        }
    }
}

