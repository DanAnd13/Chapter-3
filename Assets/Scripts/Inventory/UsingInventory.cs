using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UsingInventory : MonoBehaviour
{
    public Inventory items;
    public GameObject textForTaking;
    private float _rayDistance = 3f;
    private void Start()
    {
        
    }
    private void Update()
    {
        Vector3 rayOrigin = Camera.main.transform.position;
        Vector3 rayDirection = Camera.main.transform.forward;

        DrawRaycast(rayOrigin, rayDirection);
    }
    private void DrawRaycast(Vector3 rayOrigin, Vector3 rayDirection)
    {
        Debug.DrawLine(rayOrigin, rayOrigin + rayDirection * _rayDistance, Color.red, 2.0f);
        if (Physics.Raycast(rayOrigin, rayDirection, out RaycastHit hitInfo, _rayDistance))
        {
            textForTaking.SetActive(true);

            if (Input.GetMouseButtonDown(0))
            {
                TakeItemsOnButtonClick(hitInfo);
            }
        }
        else
        {
            textForTaking.SetActive(false);
        }
    }
    public void TakeItemsOnButtonClick(RaycastHit hitInfo)
    {
        string hitObjectName = hitInfo.collider.gameObject.name;
        if (items.itemDatabase.ContainsKey(hitObjectName))
        {
            hitInfo.collider.gameObject.SetActive(false);
            items.AddItem(items.itemDatabase[hitObjectName]);
        }
    }
}
