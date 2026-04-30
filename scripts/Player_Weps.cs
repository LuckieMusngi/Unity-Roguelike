using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Weps : MonoBehaviour
{
    public int current = 1;
    private Player_attacks[] weps;

    private void Awake()
    {
        weps = transform.GetComponentsInChildren<Player_attacks>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        _ = StartCoroutine(WepSwitch());
    }

    private void Update()
    {
        if (Input.GetMouseButton(0))
        {
            weps[current - 1].TriggerPulled();
        }
        if (Input.GetMouseButton(1))
            //Input.GetMouseButtonDown(1)
        {
            _ = StartCoroutine(weps[current - 1].StartAbility());
        }
    }

    private IEnumerator WepSwitch()
    {
        int opp = current == 1 ? 2 : 1;
        yield return new WaitUntil(() => Input.GetKeyDown(opp.ToString()));
        current = opp;
        _ = StartCoroutine(WepSwitch());
    }
}