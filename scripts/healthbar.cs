using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class healthbar : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private ParticleSystem.MainModule ps;
    private ParticleSystem.EmissionModule em;
    private health hp;
    private Slider playerHealthBar;

    // Start is called before the first frame update
    void Awake()
    {
        hp = player.GetComponent<health>();
        em = GetComponent<ParticleSystem>().emission;
        ps = GetComponent<ParticleSystem>().main;
        playerHealthBar = GameObject.Find("PlayerHealthBar").GetComponent<Slider>();
    }

    private const float lowHP = 0.2f;

    // Update is called once per frame
    private void Update()
    {
        if (player != null)
        {
            transform.position = player.transform.position;
            if (hp.hp / hp.i_hp < lowHP)
            {
                ps.startColor = Color.red;
                em.rateOverTime = 40;
                ps.startSizeMultiplier = lowHP;
                playerHealthBar.value = 0;
            }
            else
            {
                em.rateOverTime = hp.hp / 2;
                ps.startSizeMultiplier = hp.hp / hp.i_hp / 5f;
                float reduction = hp.i_hp * lowHP;
                playerHealthBar.value = (hp.hp - reduction) / (hp.i_hp - reduction);
            }
        } else
        {
            playerHealthBar.value = 0;
        }
    }
}
