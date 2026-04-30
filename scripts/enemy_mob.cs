using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy_mob : MonoBehaviour
{
    [SerializeField] private float turnspeed;
    private GameObject target;
    private Rigidbody2D rb;
    [SerializeField] private float distance;
    [SerializeField] private float speed;
    private Object projectile;
    private health h_script;
    [SerializeField] private float kb, reload_time, projspeed;
    private const int layer_mask = 1 << 0;
    private Transform barrel;

    //put in fixedupdate if using the rotatetowards thing (it slowly rotates rather than snap)
    //rb.SetRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(Utils.GetAngle(transform.position, target.transform.position), Vector3.forward), turnspeed * Time.deltaTime;
    private void Start()
    {
        projectile = Resources.Load("Prefabs/projectiles/shot");
        h_script = GetComponent<health>();
        barrel = transform.GetChild(0);
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player");
        _ = StartCoroutine(nameof(RepeatShoot));
    }

    private Vector2 relPos;
    private void FixedUpdate()
    {
        rb.SetRotation(Quaternion.RotateTowards(transform.rotation, Quaternion.AngleAxis(Utils.GetAngle(transform.position, target.transform.position), Vector3.forward), turnspeed));
    }
    private void Update()
    {
        if (target != null)
        {
            relPos = target.transform.position - transform.position;
            //rb.SetRotation(Utils.GetAngle(transform.position, target.transform.position));
            rb.AddForce((Vector2.Distance(transform.position, target.transform.position) >= distance ? speed : -speed) * Time.deltaTime * relPos.normalized);
            if (h_script.hit)
            {
                rb.AddRelativeForce(Random.Range(-1, 2) * 2 * speed * Vector2.up);
                _ = StartCoroutine(nameof(Shootdelay));
            }
        }
    }

    private void Shoot()
    {
        GameObject proj = (GameObject)Instantiate(projectile, barrel.transform.position, transform.rotation);
        proj.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.right * projspeed);
        rb.AddRelativeForce(Vector2.left * kb);
        Destroy(proj, 3);
    }

    IEnumerator Shootdelay()
    {
        yield return new WaitForSeconds(0.2f);
        Shoot();
    }
    IEnumerator RepeatShoot()
    {
        while (true)
        {
            yield return new WaitForSeconds(reload_time);
            while (Physics2D.Raycast(transform.position, relPos, Vector2.Distance(transform.position, target.transform.position), layer_mask))
            {
                yield return new WaitForEndOfFrame();
            }
            Shoot();
        }
    }
}