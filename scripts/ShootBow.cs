using System.Collections;
using UnityEngine;

public class ShootBow : MonoBehaviour
{
    float drawback = 0.6f;

    private const float rof = 0.1f;
    
    private const float force = 2500;
    [SerializeField] private Object arrow;
    public bool finished = false;
    private LineRenderer line;
    private Transform barrel;

    public bool still;

    private int moveLine, stayLine = 0;
    
    private const int layermask = 1 >> 0;

    private void Awake()
    {
        arrow = Resources.Load("Prefabs/projectiles/arrow");
        line = GetComponent<LineRenderer>();
        barrel = transform.GetChild(0);
    }

    private void Update()
    {
        if (moveLine > 0)
        {
            Lazer(Color.white);
        }
        else if (stayLine > 0)
        {
            Lazer(Color.red);
        }
        else if (line.enabled)
        {
            line.enabled = false;
        }
    }

    private void Lazer(Color color)
    {
        if (!line.enabled)
        {
            line.enabled = true;
        }
        RaycastHit2D hit = Physics2D.Raycast(barrel.position, transform.right, 50f, layermask);
        line.SetPosition(0, barrel.position);
        line.SetPosition(1, hit.point);
        line.startColor = color;
    }

    public IEnumerator Shoot(int amount, float draw_speed, float pullback_wait)
    {
        finished = false;
        float pWait = amount * rof;
        _ = StartCoroutine(DoLazers(pWait, draw_speed, pullback_wait));
        for (int i = 0; i < amount; i++)
        {
            _ = StartCoroutine(Fire(draw_speed, pullback_wait));
            yield return new WaitForSeconds(rof);
        }

        yield return new WaitForSeconds(draw_speed + pullback_wait + 0.02f);
        yield return new WaitForEndOfFrame();
        Debug.Log("finished");
        finished = true;
    }

    private IEnumerator DoLazers(float perArrowWait, float draw_speed, float wait)
    {
        //do white line while aiming
        moveLine++;
        yield return new WaitForSeconds(draw_speed);
        moveLine--;

        still = true;

        //do red line while shooting
        stayLine++;
        yield return new WaitForSeconds(wait + perArrowWait);
        stayLine--;

        still = false;
    }

    private IEnumerator Fire(float draw_speed, float wait)
    {
        GameObject proj = (GameObject)Instantiate(arrow, transform);

        for (float i = 0; i < draw_speed; i += Time.deltaTime)
        {
            yield return new WaitForEndOfFrame();
            proj.transform.localPosition = drawback * i * Vector2.left / draw_speed;
        }

        yield return new WaitForSeconds(wait);

        Rigidbody2D prb = proj.AddComponent<Rigidbody2D>();
        prb.gravityScale = 0;
        prb.AddRelativeForce(Vector2.right * force);
        yield return new WaitForSeconds(0.02f);
        proj.GetComponent<PolygonCollider2D>().enabled = true;
        Debug.Log("shot");
    }
}
