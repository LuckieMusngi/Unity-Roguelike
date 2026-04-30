using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_attacks : MonoBehaviour
{
    private Transform player;
    public GameObject projectile;
    private Rigidbody2D rb;

    public float projspeed;
    public float reload_time;
    public float kb;
    public float inaccuracy;
    public float size;
    public int damage;
    public float projlife;

    private bool reloaded = true;
    public List<GameObject> proj;
    [SerializeField] private GameObject particle;
    public bool abilityStarted = false;
    public List<GameObject> hits;

    // Start is called before the first frame update
    private void Awake()
    {
        player = transform.parent;
        rb = player.GetComponent<Rigidbody2D>();
    }

    private void LateUpdate()
    {
        Utils.ClearGoNulls(proj);
        Utils.ClearGoNulls(hits);
    }
    private const int layer_mask = 1 << 0;

    public void TriggerPulled()
    {
        if (reloaded && !Physics2D.Raycast(transform.position, Vector2.right, 0.02f, layer_mask))
        {
            _ = StartCoroutine(Shoot());
        }
    }

    public void Teleport(Vector2 location)
    {
        _ = Instantiate(particle, player.position, transform.rotation);
        player.position = location;
        _ = Instantiate(particle, player.position, transform.rotation);
    }

    private IEnumerator Shoot()
    {
        reloaded = false;
        GameObject go = Instantiate(projectile, transform.position, transform.rotation);
        go.GetComponent<Rigidbody2D>().AddRelativeForce(Utils.GetVector(Random.Range(-inaccuracy, inaccuracy), 1) * projspeed);
        go.transform.localScale = Vector2.one * size;
        p_projectile pScript = go.GetComponent<p_projectile>();
        pScript.wep = this;
        pScript.i_damage = damage;
        pScript.lifetime = projlife;
        proj.Add(go);
        rb.AddRelativeForce(Vector2.left * kb);
        yield return new WaitForSeconds(reload_time);
        reloaded = true;
    }
    public IEnumerator StartAbility()
    {
        abilityStarted = true;
        yield return new WaitForEndOfFrame();
        yield return new WaitForEndOfFrame();
        abilityStarted = false;
    }
}
