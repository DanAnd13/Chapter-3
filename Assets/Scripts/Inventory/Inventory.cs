using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [System.Serializable]
    public class ItemData
    {
        public string itemName;
    }

    [System.Serializable]
    public class InventoryData
    {
        public List<ItemData> items;
    }

    public List<Item> items = new List<Item>();
    [HideInInspector]
    public Dictionary<string, Item> itemDatabase = new Dictionary<string, Item>();
    private string _filePath;

    private void Awake()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "inventory.json");
        LoadItemDatabase();
    }
    private void Start()
    {
        LoadInventory();
    }
    public void AddItem(Item newItem)
    {
        items.Add(newItem);
        SaveInventory();
    }
    private void LoadItemDatabase()
    {
        Item[] itemAssets = Resources.LoadAll<Item>("");
        foreach (Item item in itemAssets)
        {
            if (!itemDatabase.ContainsKey(item.itemName))
            {
                itemDatabase.Add(item.itemName, item);
            }
        }
    }
    private void SaveInventory()
    {
        List<ItemData> itemDataList = new List<ItemData>();
        foreach (var item in items)
        {
            itemDataList.Add(new ItemData { itemName = item.itemName});
        }
        string json = JsonUtility.ToJson(new InventoryData { items = itemDataList }, true);
        File.WriteAllText(_filePath, json);
    }
    private void LoadInventory()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);
            string[] jsonValue = json.Split(' ');
            InventoryData loadedData = JsonUtility.FromJson<InventoryData>(json);
            items.Clear();
            foreach (var itemData in loadedData.items)
            {
                if (itemDatabase.TryGetValue(itemData.itemName, out Item item))
                {
                    items.Add(item);
                }
                else
                {
                    Debug.LogWarning($"Item '{itemData.itemName}' not found in item database.");
                }
            }
        }
    }
}
