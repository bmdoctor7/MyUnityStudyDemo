using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Game/BuildingData")]
public class BuildingData : ScriptableObject
{
    public string BuildingId;
    public string DisplayName;
    [TextArea] public string Description;
    public int Cost; // 建筑花费
    public GameObject Prefab;
    public Sprite Icon;
}