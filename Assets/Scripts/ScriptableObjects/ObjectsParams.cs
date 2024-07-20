using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsParams : MonoBehaviour
{
    public enum rarity
    {
        rare,
        mythical,
        legendary
    }
    public float cost;
    public rarity objectRarity;
}
