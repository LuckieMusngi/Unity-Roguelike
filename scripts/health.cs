using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class health : MonoBehaviour
{
    public bool invincible = false;
    public float i_hp;
    public float hp;
    [SerializeField] private float defense;
    [SerializeField] private GameObject p_death;
    [SerializeField] private GameObject p_hit;
    public Color color;
    public bool hit;

    private void Awake()
    {
        hp = i_hp;
        p_hit = (GameObject)Resources.Load("Prefabs/particles/p_hit");
        p_death = (GameObject)Resources.Load("Prefabs/particles/p_explosion");
        color = GetComponent<SpriteRenderer>().color;
    }

    public void TakeDamage(Vector2 pos, float damage, bool crit)
    {
        if (invincible) { return; }
        _ = StartCoroutine(Hit());
        GameObject hitp = Instantiate(p_hit, transform.position, Quaternion.Euler(0, 0, Utils.GetAngle(transform.position, pos)));
        ParticleSystem.MainModule ps = hitp.GetComponent<ParticleSystem>().main;
        ps.startSize = damage / 100;
        if (crit)
        {
            ps.startColor = color;
            damage *= 1.5f;
        }
        hp -= damage;
        if (hp <= 0)
        {
            Die();
        }
    }
    
    public IEnumerator Hit()
    {
        hit = true;
        yield return new WaitForEndOfFrame();
        hit = false;
    }

    public void Die()
    {
        GameObject deathp = Instantiate(p_death, transform.position, Quaternion.identity);
        ParticleSystem.MainModule deathps = deathp.GetComponent<ParticleSystem>().main;
        deathps.startColor = color;
        Destroy(gameObject);
    }
}