using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class InventoryItemEditor : EditorWindow
{

    public InventoryItemList inventoryItemList;
    private int _viewIndex = 1;

    [MenuItem("Window/Inventory Item Editor %#e")]
    static void Init()
    {
        EditorWindow.GetWindow(typeof(InventoryItemEditor));
    }

    void OnEnable()
    {
        if (EditorPrefs.HasKey("ObjectPath"))
        {
            string objectPath = EditorPrefs.GetString("ObjectPath");
            inventoryItemList = AssetDatabase.LoadAssetAtPath(objectPath, typeof(InventoryItemList)) as InventoryItemList;
        }

    }

    void OnGUI()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("Inventory Item Editor", EditorStyles.boldLabel);
        if (inventoryItemList != null)
        {
            if (GUILayout.Button("Show Item List"))
            {
                EditorUtility.FocusProjectWindow();
                Selection.activeObject = inventoryItemList;
            }
        }
        if (GUILayout.Button("Open Item List"))
        {
            OpenItemList();
        }
        if (GUILayout.Button("New Item List"))
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = inventoryItemList;
        }
        GUILayout.EndHorizontal();

        if (inventoryItemList == null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(10);
            if (GUILayout.Button("Create New Item List", GUILayout.ExpandWidth(false)))
            {
                CreateNewItemList();
            }
            if (GUILayout.Button("Open Existing Item List", GUILayout.ExpandWidth(false)))
            {
                OpenItemList();
            }
            GUILayout.EndHorizontal();
        }

        GUILayout.Space(20);

        if (inventoryItemList != null)
        {
            GUILayout.BeginHorizontal();

            GUILayout.Space(10);

            if (GUILayout.Button("Prev", GUILayout.ExpandWidth(false)))
            {
                if (_viewIndex > 1)
                    _viewIndex--;
            }
            GUILayout.Space(5);
            if (GUILayout.Button("Next", GUILayout.ExpandWidth(false)))
            {
                if (_viewIndex < inventoryItemList.itemList.Count)
                {
                    _viewIndex++;
                }
            }

            GUILayout.Space(60);

            if (GUILayout.Button("Add Item", GUILayout.ExpandWidth(false)))
            {
                AddItem();
            }
            if (GUILayout.Button("Delete Item", GUILayout.ExpandWidth(false)))
            {
                DeleteItem(_viewIndex - 1);
            }

            GUILayout.EndHorizontal();
            if (inventoryItemList.itemList == null)
                Debug.Log("Inventory is empty");
            if (inventoryItemList.itemList.Count > 0)
            {
                GUILayout.BeginHorizontal();
                _viewIndex = Mathf.Clamp(EditorGUILayout.IntField("Current Item", _viewIndex, GUILayout.ExpandWidth(false)), 1, inventoryItemList.itemList.Count);
                //Mathf.Clamp (viewIndex, 1, inventoryItemList.itemList.Count);
                EditorGUILayout.LabelField("of   " + inventoryItemList.itemList.Count.ToString() + "  items", "", GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                inventoryItemList.itemList[_viewIndex - 1].itemName = EditorGUILayout.TextField("Item Name", inventoryItemList.itemList[_viewIndex - 1].itemName as string);
                inventoryItemList.itemList[_viewIndex - 1].itemIcon = EditorGUILayout.ObjectField("Item Icon", inventoryItemList.itemList[_viewIndex - 1].itemIcon, typeof(Texture2D), false) as Texture2D;
                inventoryItemList.itemList[_viewIndex - 1].itemObject = EditorGUILayout.ObjectField("Item Object", inventoryItemList.itemList[_viewIndex - 1].itemObject, typeof(Rigidbody), false) as Rigidbody;

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                inventoryItemList.itemList[_viewIndex - 1].isUnique = (bool)EditorGUILayout.Toggle("Unique", inventoryItemList.itemList[_viewIndex - 1].isUnique, GUILayout.ExpandWidth(false));
                inventoryItemList.itemList[_viewIndex - 1].isIndestructible = (bool)EditorGUILayout.Toggle("Indestructable", inventoryItemList.itemList[_viewIndex - 1].isIndestructible, GUILayout.ExpandWidth(false));
                inventoryItemList.itemList[_viewIndex - 1].isQuestItem = (bool)EditorGUILayout.Toggle("QuestItem", inventoryItemList.itemList[_viewIndex - 1].isQuestItem, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

                GUILayout.BeginHorizontal();
                inventoryItemList.itemList[_viewIndex - 1].isStackable = (bool)EditorGUILayout.Toggle("Stackable ", inventoryItemList.itemList[_viewIndex - 1].isStackable, GUILayout.ExpandWidth(false));
                inventoryItemList.itemList[_viewIndex - 1].destroyOnUse = (bool)EditorGUILayout.Toggle("Destroy On Use", inventoryItemList.itemList[_viewIndex - 1].destroyOnUse, GUILayout.ExpandWidth(false));
                inventoryItemList.itemList[_viewIndex - 1].encumbranceValue = EditorGUILayout.FloatField("Encumberance", inventoryItemList.itemList[_viewIndex - 1].encumbranceValue, GUILayout.ExpandWidth(false));
                GUILayout.EndHorizontal();

                GUILayout.Space(10);

            }
            else
            {
                GUILayout.Label("This Inventory List is Empty.");
            }
        }
        if (GUI.changed)
        {
            EditorUtility.SetDirty(inventoryItemList);
        }
    }

    void CreateNewItemList()
    {
        // There is no overwrite protection here!
        // There is No "Are you sure you want to overwrite your existing object?" if it exists.
        // This should probably get a string from the user to create a new name and pass it ...
        _viewIndex = 1;
        inventoryItemList = CreateInventoryItemList.Create();
        if (inventoryItemList)
        {
            inventoryItemList.itemList = new List<InventoryItems>();
            string relPath = AssetDatabase.GetAssetPath(inventoryItemList);
            EditorPrefs.SetString("ObjectPath", relPath);
        }
    }

    void OpenItemList()
    {
        string _absPath = EditorUtility.OpenFilePanel("Select Inventory Item List", "", "");
        if (_absPath.StartsWith(Application.dataPath))
        {
            string relPath = _absPath.Substring(Application.dataPath.Length - "Assets".Length);
            inventoryItemList = AssetDatabase.LoadAssetAtPath(relPath, typeof(InventoryItemList)) as InventoryItemList;
            if (inventoryItemList.itemList == null)
                inventoryItemList.itemList = new List<InventoryItems>();
            if (inventoryItemList)
            {
                EditorPrefs.SetString("ObjectPath", relPath);
            }
        }
    }

    void AddItem()
    {
        InventoryItems newItem = new InventoryItems();
        newItem.itemName = "New Item";
        inventoryItemList.itemList.Add(newItem);
        _viewIndex = inventoryItemList.itemList.Count;
    }

    void DeleteItem(int index)
    {
        inventoryItemList.itemList.RemoveAt(index);
    }
}