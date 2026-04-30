using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PA_Teleport : MonoBehaviour
{
    private Player_attacks script;

    private void Awake()
    {
        script = GetComponentInParent<Player_attacks>();
    }
    private int index;
    private void Update()
    {
        if (script.abilityStarted)
        {
            index = script.proj.Count - 1;
            if (index != -1)
            {
                Teleport();
            }
            else
            {
                return;
            }
        }
    }

    private void Teleport()
    {
        if (script.proj[index] != null)
        {
            GameObject tpTo = script.proj[index];
            script.Teleport(tpTo.transform.position);
            Destroy(tpTo);
        }
    }
}