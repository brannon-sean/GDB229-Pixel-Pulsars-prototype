using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject
{
    public string itemName;
    public Sprite sprite;
    public int price;
    public string description;
    public int speed;
    public int damage;
    public int jumps;
    public int health;
    public float attackSpeed;
    public float lifeSteal;
    public int shootDistance;
}