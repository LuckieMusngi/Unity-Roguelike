using System.Collections;
using UnityEngine;

public class room_script : MonoBehaviour
{
    public int enemy_amount = 6;
    private Object spawn;
    private Roomgen script;
    private const int away = 3;

    private void Awake()
    {
        script = GetComponent<Roomgen>();
        spawn = Resources.Load("Prefabs/Spawn");
    }

    private void Start()
    {
        script.MakeBlocks();
        _ = StartCoroutine(nameof(WaitClear));

        //makes enemy things
        for (int i = 0; i < enemy_amount; i++)
        {
            GameObject spawn_c = (GameObject)Instantiate(spawn,
                new Vector2(Random.Range(away, script.roomx - away),
                Random.Range(away, script.roomy - away)) + (Vector2)transform.position + new Vector2Int(-script.roomx / 2, -script.roomy / 2), Quaternion.identity);
            spawn_c.transform.parent = transform;
        }
    }

    private bool started = false;
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!started && col.gameObject.layer == 6)
        {
            started = true;
            script.Close();
            Spawn[] scripts = transform.GetComponentsInChildren<Spawn>();
            for (int i = 0; i < scripts.Length; i++)
            {
                scripts[i].Begin();
            }
        }
    }
    IEnumerator WaitClear()
    {
        yield return new WaitForEndOfFrame();
        //it's 1 here because of the minimap child
        while (transform.childCount != 1)
        {
            yield return new WaitForEndOfFrame();
        }
        script.Open();
    }

}