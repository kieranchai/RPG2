using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionScript : MonoBehaviour
{
    private float splashRange;
    public float explosionRate;
    private float attackPower;
    [SerializeField] private AudioSource audioSource;

    void Start()
    {
        transform.localScale = new Vector3 (0,0,0);
    }

    public void Initialize(float attackPower, float splashRange)
    {
        this.attackPower = attackPower;
        this.splashRange = splashRange;
        audioSource.volume = AudioManager.sfxVol;
        AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
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
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<EnemyScript>().Attacked(attackPower);
        }

        if (collision.gameObject.CompareTag("Tilemap Colliders"))
        {
            Destroy(gameObject);
        }
    }
}
