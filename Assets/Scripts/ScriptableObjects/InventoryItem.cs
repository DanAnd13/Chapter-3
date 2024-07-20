using UnityEngine;
using System.Collections;

[System.Serializable]
public class InventoryItem
{
    public string itemName = "New Item";
    public float[] itemPosition = new float[3];
    public string itemRarity = null;
    public float itemCost = 0;
}