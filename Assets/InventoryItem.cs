using UnityEngine;

[CreateAssetMenu(fileName = "New Inventory Item", menuName = "Inventory/Item")]
public class InventoryItem : ScriptableObject
{
    public string itemName; // Nombre del objeto
    public Sprite icon;     // Icono del objeto
    public string description; // Descripción del objeto
}

