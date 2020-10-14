using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    public GameObject arrowPrefab;
    public void Shoot(float time) 
    {
        var obj = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        obj.TryGetComponent(out Rigidbody rb);
        if (rb == null) return;

        var dir = transform.forward;

        rb.velocity = dir * 200;

        Destroy(obj, 1);
    }
}