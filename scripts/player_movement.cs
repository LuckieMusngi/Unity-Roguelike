using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.MoveRotation(Utils.GetAngle(transform.position, mos));
        Vector2 move = Vector2.zero;
        if (Input.GetKey("a"))
        {
            move += new Vector2(-1, 0);
        }
        if (Input.GetKey("w"))
        {
            move += new Vector2(0, 1);
        }
        if (Input.GetKey("s"))
        {
            move += new Vector2(0, -1);
        }
        if (Input.GetKey("d"))
        {
            move += new Vector2(1, 0);
        }
        if (move.x == 0 && move.y == 0)
        {
            rb.AddForce(move * speed * Time.deltaTime);
        }
        else
        {
            rb.AddForce(move * speed * Mathf.Sqrt(2) * Time.deltaTime);
        }
    }
}
