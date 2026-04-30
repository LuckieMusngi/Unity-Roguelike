using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PP_Stack : MonoBehaviour
{
    private Object marker;
    private Player_attacks script;
    [SerializeField] private List<MarkerDamageScript> markers;

    private void Awake()
    {
        marker = Resources.Load("Prefabs/MarkerDamage");
        script = GetComponentInParent<Player_attacks>();
    }

    private void Update()
    {
        if (0 != script.hits.Count)
        {
            for (int i = 0; i < script.hits.Count; i++)
            {
                AddStack(script.hits[i]);
            }
            script.hits.Clear();
        }
        if (script.abilityStarted)
        {
            if (markers.Count != 0)
            {
                foreach (MarkerDamageScript item in markers)
                {
                    if (item != null)
                    {
                        item.Begin();
                    }
                }
                markers.Clear();
            }
        }
    }



    private void AddStack(GameObject go)
    {
        if (go != null)
        {
            GameObject stack = (GameObject)Instantiate(marker, go.transform.position, Quaternion.identity, go.transform);
            if (stack != null)
            {
                MarkerDamageScript mScript = stack.GetComponent<MarkerDamageScript>();
                mScript.damage = script.damage / 2.5f;
                markers.Add(mScript);
            }
        }
    }
}