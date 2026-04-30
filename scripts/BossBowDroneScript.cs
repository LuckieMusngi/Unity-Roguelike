using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBowDroneScript : MonoBehaviour
{
    public Transform target;
    public Transform boss;
    private Rigidbody2D rb;
    public float degreeOffset;
    private float degrees;
    private ShootBow shootScript;

    private void Awake()
    {
        shootScript = transform.GetComponent<ShootBow>();
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = transform.GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (!shootScript.still && target != null)
        {
            rb.SetRotation(Utils.GetAngle(transform.position, target.transform.position));
            degrees += Time.deltaTime * 100;
            transform.position = Utils.GetVector(degrees + degreeOffset, 2f) + (Vector2)boss.position;
        }
    }
}
