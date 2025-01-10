using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory/Item")]
public abstract class InventoryItem : ScriptableObject
{
    public string itemName; // Nom de l'objecte
    public Sprite icon;     // Icone 
    public string description;

    public bool isStackable = false; // Si l'objecte es apilable
    public int maxStack = 99;        // Límit màxim d'objectes apilables

    // Metode abstracte per executar accions
    public abstract void UseItem();
}

