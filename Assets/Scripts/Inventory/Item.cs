using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu(fileName = "NewItem", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public rarity objectRarity;
    public float cost;
    public enum rarity
    {
        rare,
        mythical,
        legendary
    }  
}

