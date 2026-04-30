using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class chaser : MonoBehaviour
{
    private Rigidbody2D rb;
    private Transform player;
    private health hp;
    [SerializeField] private float start_speed;
    [SerializeField] private float added_speed;
    [SerializeField] private float speed;
    private float rotation;
    public float damage;

    private void Start()
    {
        speed = start_speed;
        hp = GetComponent<health>();
        player = GameObject.Find("Player").transform;
        rb = GetComponent<Rigidbody2D>();
    }
    private void Update()
    {
        rotation += speed;
        rb.MoveRotation(rotation);
        if (player != null)
        {
            rb.AddForce((player.position - transform.position).normalized * speed);
        }
        if (hp.hit)
        {
            speed = start_speed + (added_speed * (hp.i_hp - hp.hp) / hp.i_hp);
        }
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //6 is player
        if (col.gameObject.layer == 6)
        {
            col.gameObject.GetComponent<health>().TakeDamage(transform.position, damage, true);
            hp.Die();
        }
    }
}
