using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private float splashRange;
    public float explosionRate;
    private int attackPower;

    void Start()
    {
        transform.localScale = new Vector3 (0,0,0);
    }

    public void Initialize(int attackPower, float splashRange)
    {
        this.attackPower = attackPower;
        this.splashRange = splashRange;
    }

    void Update()
    {
        if (transform.localScale.x < splashRange) {
            transform.localScale += new Vector3(explosionRate,explosionRate,explosionRate) * Time.deltaTime;
        }
        else {
            Destroy(gameObject); 
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            collision.gameObject.GetComponent<EnemyScript>().Attacked(attackPower);
        }

        if (collision.gameObject.tag == "Tilemap Colliders")
        {
            Destroy(gameObject);
        }
    }
}
