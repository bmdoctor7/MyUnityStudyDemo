using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()][System.Serializable]
public class InventoryData : ScriptableObject
{
    public List<SlotData> slotsList;
}
