using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class playerInventory : MonoBehaviour, IIventory
{
    private List<Item> items = new List<Item>();
    public void addItem(Item item)
    {
        if (!containsItem(item))
        {
            items.Add(item);
            updateInventory();
        }
    }
    public void removeItem(Item item)
    {
        if (containsItem(item))
        {
            items.Remove(item);
            updateInventory();
        }
    }
    public bool containsItem(Item item)
    {
        return items.Contains(item);
    }

    private void updateInventory()
    {
        int i = 0;
        foreach(Image slot in gamemanager.instance.inventorySlots)
        {
            if (i < items.Count)
            {
                slot.sprite = items[i].sprite; ;
                i++;
            }
            else
            {
                slot.sprite = null;
            }
        }
    }
}
