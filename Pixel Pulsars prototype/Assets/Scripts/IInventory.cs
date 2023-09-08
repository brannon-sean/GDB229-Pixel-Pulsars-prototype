using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IIventory
{
    public void addItem(Item item, int ammount);
    public void removeItem(Item item);
    public bool containsItem(Item item);
}
