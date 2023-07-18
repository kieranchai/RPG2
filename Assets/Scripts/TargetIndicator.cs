using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// KIERAN AND JOEL

public class TargetIndicator : MonoBehaviour
{
    public Transform target;
    private float hideDistance = 9.5f;

    private void Update()
    {
        var dir = target.position - transform.position;
        if (dir.magnitude < hideDistance)
        {
            SetChildrenActive(false);
        }
        else
        {
            SetChildrenActive(true);
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        }

    }

    void SetChildrenActive(bool value)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(value);
        }
    }
}
