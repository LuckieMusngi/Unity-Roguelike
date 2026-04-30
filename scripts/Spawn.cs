using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    [SerializeField] private Object[] enemies;
    [SerializeField] private float speed;
    private const int e_folder = 3;
    private bool beginning = true;

    private void Awake()
    {
        enemies = new Object[e_folder];
        enemies = Resources.LoadAll("Prefabs/enemies/lvl_1");
    }

    private void Start()
    {
        _ = StartCoroutine(nameof(Vibe));
    }

    private IEnumerator Vibe()
    {
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        beginning = false;
        GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speed);

    }
    public void Begin()
    {
        GameObject enemy = (GameObject)Instantiate(enemies[Random.Range(0, e_folder)], transform.position, Quaternion.identity);
        enemy.transform.parent = transform.parent;
        Destroy(gameObject);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (beginning)
        {
            transform.position += Vector3.right * 0.2f;
        }
    }


}
