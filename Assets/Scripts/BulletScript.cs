using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    private int attackPower;
    private float weaponRange;
    private Vector3 initialPosition;

    public void Initialize(int attackPower, float weaponRange)
    {
        this.attackPower = attackPower;
        this.weaponRange = weaponRange;
        this.initialPosition = transform.localPosition;
    }

    private void Update()
    {
        if((transform.position - initialPosition).magnitude >= this.weaponRange)
        {
            Destroy(gameObject);
        }
    }
}
