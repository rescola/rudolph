using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName; // Nom de l'objecte
    public Sprite icon;     // Icone 
    public string description;
}

