using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Inventory", menuName = "Inventory/List", order = 1)]
public class InventoryList : ScriptableObject
{
    public List<InventoryItem> _items = new List<InventoryItem>();
}
