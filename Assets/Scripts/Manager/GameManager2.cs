using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager2 : SingletonMonoBase<GameManager2>
{
    private GameManager2(){}
    
    public List<GameObject> buildingsList = new List<GameObject>();


    private void Update()
    {
        if (Input.GetKeyDown("space"))
        {
            EnemyManager.Instance.SpawnZonesByTag("Spawn1");
        }
    }


    private void Start()
    {
        GameObject building = GameObject.FindGameObjectWithTag("Building");
        if (building)
        {
            buildingsList.Add(building);
        }
        //EnemyManager.Instance.SpawnZonesByTag("Spawn1");
    }
}
