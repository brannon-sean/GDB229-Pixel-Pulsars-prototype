using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class playerInventory : MonoBehaviour, IIventory
{
    private List<KeyValuePair<Item, int>> items = new List<KeyValuePair<Item, int>>();

    public void addItem(Item item, int ammount=1)
    {
        if (!containsItem(item))
        {
            items.Add(new KeyValuePair<Item, int>(item, 1));
            updateInventory();
        }
        else
        {
            KeyValuePair<Item, int> existingItem = items.Find(kv => kv.Key == item);
            items.Remove(existingItem);
            items.Add(new KeyValuePair<Item, int>(item, existingItem.Value + 1));
            updateInventory();
        }
    }

    public void removeItem(Item item, int ammount = 1)
    {
        if (containsItem(item))
        {
            KeyValuePair<Item, int> existingItem = items.Find(kv => kv.Key == item);
            items.Remove(existingItem);
            items.Add(new KeyValuePair<Item, int>(item, existingItem.Value - ammount));
            updateInventory();
        }
    }

    public bool containsItem(Item item)
    {
        return items.Exists(kv => kv.Key == item);
    }

    public bool hasEnough(Item item, int amountRequired=0)
    {
        if (containsItem(item))
        {
            KeyValuePair<Item, int> existingItem = items.Find(kv => kv.Key == item);
            
            if(existingItem.Value >= amountRequired)
            {
                return true;
            }
        }
        return false;
    }

    private void updateInventory()
    {
        int i = 0;
        foreach (Image slot in gamemanager.instance.inventoryItems)
        {
            if (i < items.Count)
            {
                Item item = items[i].Key;
                slot.sprite = item.sprite;
                TextMeshProUGUI count = slot.GetComponentInChildren<TextMeshProUGUI>();
                if (count != null)
                {
                    int itemCount = items[i].Value;
                    count.text = itemCount.ToString();
                }
                i++;
            }
            else
            {
                slot.sprite = null;
            }
        }
    }
}