using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsingInventory : MonoBehaviour
{
    public InventoryList list;
    public GameObject textForTaking;
    private float _rayDistance = 3f;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;

        DrawRaycast(rayOrigin, rayDirection);
    }
    void DrawRaycast(Vector3 rayOrigin, Vector3 rayDirection)
    {
        Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * _rayDistance, Color.red, 2.0f);
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, _rayDistance))
        {
            textForTaking.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                TakeItemsOnButtonClick(hitInfo);
                hitInfo.collider.gameObject.SetActive(false);
            }
        }
        else
        {
            textForTaking.SetActive(false);
        }
    }
    public void TakeItemsOnButtonClick(RaycastHit hitInfo)
    {
        InventoryItem inventory = new InventoryItem();
        ObjectsParams objectRarity = hitInfo.collider.gameObject.GetComponent<ObjectsParams>();
        inventory.itemName = hitInfo.collider.gameObject.name;
        inventory.itemPosition = new float[3];
        inventory.itemPosition[0] = hitInfo.transform.position.x;
        inventory.itemPosition[1] = hitInfo.transform.position.y;
        inventory.itemPosition[2] = hitInfo.transform.position.z;
        inventory.itemRarity = objectRarity.objectRarity.ToString();
        inventory.itemCost = objectRarity.cost;
        list._items.Add(inventory);
    }
}
