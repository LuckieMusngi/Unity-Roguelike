using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerDamageScript : MonoBehaviour
{
    public float damage;
    public void Begin()
    {
        health script = GetComponentInParent<health>();
        Vector2 dir = (transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f))).normalized * 2;
        script.TakeDamage(dir, damage, true);
        transform.GetComponentInParent<Rigidbody2D>().AddForce(dir * -50);
        Destroy(gameObject);
    }
}