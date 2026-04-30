using UnityEngine;
using System.Collections;

public class p_projectile : MonoBehaviour
{
    public int i_damage;
    private float damage;
    public float lifetime;
    [SerializeField] private GameObject p_bounce;
    [SerializeField] private GameObject p_hit;
    [SerializeField] private GameObject end;
    private float i_size;
    public float critDistance;
    public Vector2 startPos;
    public bool hit = false;
    public Player_attacks wep;

    private void Start()
    {
        startPos = transform.position;
        i_size = transform.localScale.x;
        damage = i_damage;
        _ = StartCoroutine(Die());
    }
    // Update is called once per frame
    void Update()
    {
        transform.localScale -= Time.deltaTime * i_size * Vector3.one / lifetime;
        damage -= i_damage / lifetime * Time.deltaTime;
    }
    private IEnumerator Die()
    {
        yield return new WaitForSeconds(lifetime);
        End();
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //8 is enemy layer
        if (col.gameObject.layer == 8)
        {
            GameObject hitEnemy = col.gameObject;
            bool crit = Mathf.Abs(Vector2.Distance(transform.position, startPos)) < critDistance;
            //GameObject hit = Instantiate(p_hit, col.contacts[0].point, Quaternion.Euler(new Vector3(0, 0, Utils.GetAngle(transform.position, col.contacts[0].point))));
            hitEnemy.GetComponent<health>().TakeDamage(transform.position, damage, crit);
            wep.hits.Add(hitEnemy);
            _ = StartCoroutine(Hit());
        } else
        {
            _ = Instantiate(p_bounce, col.contacts[0].point, Quaternion.Euler(new Vector3(0, 0, Utils.GetAngle(transform.position, col.contacts[0].point))));
        }
    }

    public void End()
    {
        _ = Instantiate(p_bounce, transform.position, Quaternion.Euler(Vector3.up * 90));
        Destroy(gameObject);
    }

    private IEnumerator Hit()
    {
        hit = true;
        yield return new WaitForEndOfFrame();
        hit = false;
    }
}
