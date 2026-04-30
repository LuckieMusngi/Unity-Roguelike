using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bow_Script : MonoBehaviour
{
    [SerializeField] private Transform target;
    private ShootBow shootScript;
    private Rigidbody2D rb;
    private health health;
    private const float speed = 1500;

    private void Awake()
    {
        target = GameObject.Find("Player").transform;
        shootScript = GetComponent<ShootBow>();
        health = GetComponent<health>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        _ = StartCoroutine(Reload());
    }

    private void Update()
    {
        if (!shootScript.still && target != null)
        {
            rb.SetRotation(Utils.GetAngle(transform.position, target.transform.position));
            if (health.hit)
            {
                Jump();
            }
        }
    }

    private IEnumerator Reload()
    {
        Jump();
        yield return new WaitForSeconds(1);
        Jump();
        _ = StartCoroutine(shootScript.Shoot(1, 2, 0.5f));
        yield return new WaitUntil(() => shootScript.finished);
        _ = StartCoroutine(Reload());
    }

    private void Jump()
    {
        Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speed;
        rb.AddForce(dir);
    }
}
