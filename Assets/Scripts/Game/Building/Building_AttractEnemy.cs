using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Building_AttractEnemy : BuildingBase
{
    private void Awake()
    {
        GameManager2.Instance.buildingsList.Add(this.gameObject);
        hpText = this.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        hpSlider = this.gameObject.GetComponentInChildren<Slider>();
    }

    private void Start()
    {
        UpdateHpui();
    }

    
    
    
}
