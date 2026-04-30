using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class E_projectile : MonoBehaviour
{
    public bool hit;
    [SerializeField] private Color color;
    [SerializeField] private GameObject particle;
    public float damage;

    private void Awake()
    {
        particle = (GameObject)Resources.Load("prefabs/particles/p_bounce");
        color = GetComponent<SpriteRenderer>().color;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        //6 is player
        GameObject obj = col.gameObject;
        if (obj.layer == 6)
        {
            obj.GetComponent<health>().TakeDamage(transform.position, damage, true);
        }
        GameObject p = Instantiate(particle, col.contacts[0].point, Quaternion.Euler(new Vector3(0, 0, Utils.GetAngle(transform.position, col.contacts[0].point))));
        ParticleSystem.MainModule main = p.GetComponent<ParticleSystem>().main;
        main.startColor = color;
        main.startSizeMultiplier = damage / 50;
        Destroy(gameObject);
    }
}
