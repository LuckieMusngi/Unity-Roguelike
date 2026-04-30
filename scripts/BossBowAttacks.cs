using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBowAttacks : MonoBehaviour
{
    private Transform target;
    private Rigidbody2D rb;
    private ShootBow shootScript;
    private float speed = 15000;
    private health health;
    [SerializeField]private GameObject staticBow;
    public BossBowDroneScript[] drones;
    [SerializeField] private GameObject drone;

    // Start is called before the first frame update
    void Awake()
    {
        health = GetComponent<health>();
        shootScript = GetComponent<ShootBow>();
        rb = GetComponent<Rigidbody2D>();
        target = GameObject.Find("Player").transform;
    }

    private const int droneAmount = 3;
    
    private void Start()
    {
        MakeDrones(3);
    }

    // Update is called once per frame
    void Update()
    {
        if (!shootScript.still && target != null)
        {
            rb.SetRotation(Utils.GetAngle(transform.position, target.transform.position));
            if (health.hit)
            {
                RandoJump();
            }
        }
        if (Input.GetKeyDown("e"))
        {
            _ = StartCoroutine(MultiJumpShoot(5));
        }
        if (Input.GetKeyDown("q"))
        {
            _ = StartCoroutine(Spiral(72, 5));
        }
        if (Input.GetKeyDown("r"))
        {
            ShootDrones();
        }
    }

    private void MakeDrones(int amount)
    {
        drones = new BossBowDroneScript[amount];
        for (int i = 0; i < droneAmount; i++)
        {
            drones[i] = Instantiate(drone).GetComponent<BossBowDroneScript>();
            drones[i].boss = transform;
            drones[i].degreeOffset = 360 / amount * i;
        }
    }

    private void ShootDrones()
    {
        foreach (var drone in drones)
        {
            Debug.Log($"drone amount is {drones.Length}");
            _ = StartCoroutine(drone.GetComponent<ShootBow>().Shoot(1, 0.8f, 0.5f));
        }
    }

    private void Surround(int amount, int distance)
    {
        int offset = Random.Range(0, 90);
        for (int i = 1; i <= amount; i++)
        {
            SummonAround((360 * i / amount) + offset, 3, distance, target.position);
        }
    }

    private IEnumerator Spiral(int bowAmount, int distance)
    {
        int offset = Random.Range(0, 360);
        target.position = transform.parent.position;
        for (int i = 0; i < bowAmount; i++)
        {
            float degrees = (360 * i / bowAmount) + offset;
            SummonAround(degrees , 3, distance, transform.parent.position);
            SummonAround(degrees + 90, 3, distance, transform.parent.position);
            yield return new WaitForSeconds(0.05f);
        }
    }

    private void SummonAround(float degrees, int shotAmount, int distance, Vector2 position)
    {
        Vector2 bowPosition = position + Utils.GetVector(degrees, distance);
        GameObject bow = SummonStaticBows(bowPosition, position);
        _ = StartCoroutine(bow.GetComponent<StaticBowScript>().Shoot(shotAmount));
    }

    private IEnumerator MultiJumpShoot(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            yield return new WaitForSeconds(0.5f);
            JumpShoot();
            Surround(Random.Range(3, 6), 3);
            yield return new WaitUntil(() => shootScript.finished);
        }
    }

    private GameObject SummonStaticBows(Vector2 bowPosition, Vector2 targetPosition)
    {
        return Instantiate(staticBow, bowPosition, Quaternion.Euler(0, 0, Utils.GetAngle(bowPosition, targetPosition)));
    }

    private void JumpShoot()
    {
        DirJump();
        _ = StartCoroutine(shootScript.Shoot(1, 0.8f, 0.2f));
    }

    private void DirJump()
    {
        Vector2 dir = Utils.GetVector(Utils.GetAngle(transform.position, target.position) + Random.Range(-90, 91), speed);
        rb.AddForce(dir);
    }

    private void RandoJump()
    {
        Vector2 dir = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized * speed / 2;
        rb.AddForce(dir);
    }
}