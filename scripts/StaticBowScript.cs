using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticBowScript : MonoBehaviour
{
    [SerializeField] private GameObject proj;
    [SerializeField] private LineRenderer line;
    public float draw_speed = 0.5f;

    public GameObject target;
    private ShootBow shoot;
    
    // Start is called before the first frame update
    void Awake()
    {
        shoot = GetComponent<ShootBow>();
        target = GameObject.Find("Player");
    }

    public IEnumerator Shoot(int shotAmount)
    {
        Coroutine cor = StartCoroutine(shoot.Shoot(shotAmount, draw_speed, 0.5f));
        yield return new WaitUntil(() => shoot.finished);
        Destroy(gameObject);
    }
}
