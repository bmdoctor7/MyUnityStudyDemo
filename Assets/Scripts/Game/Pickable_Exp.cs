using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickable_Exp : MonoBehaviour
{
    public float addExp = 10f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayController player = other.GetComponent<PlayController>();
            if (player)
            {
                player.currentExp += addExp;
                if(!Exp_Pool.Instance.isEmpty()) 
                {
                    Exp_Pool.Instance.ReleaseExp(this.gameObject);
                }
                else
                {
                    Destroy(this.gameObject);
                }
            }
        }
    }
}
