using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    // Start is called before the first frame update
    public float explosionRadius;
    public float explosionRate;
    private int attackPower;
    void Start()
    {
        transform.localScale = new Vector3 (0,0,0);
    }

    public void Initialize(int attackPower)
    {
        this.attackPower = attackPower;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.localScale.x < explosionRadius) {
            transform.localScale += new Vector3(explosionRate,explosionRate,explosionRate)  * Time.deltaTime;
        }else{
            Destroy(gameObject); 
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
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
