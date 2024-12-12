using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<InventorySlot> slots = new List<InventorySlot>(); // Lista de slots del inventario

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddItemToInventory(InventoryItem item)
    {
        // Mira si esta ja a l'inventari l'objecte
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty() && slot.GetItem() == item) // compara amb l'existent
            {
                Debug.Log($"Ya no hay nada más aquí");
                return;
            }
        }
        // Itera slots per trobar un lliure
        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.AddItem(item); // Afageix a l'slot lliure
                Debug.Log($"Item {item.itemName} añadido al inventario.");
                return;
            }
        }

        Debug.LogWarning("No hay espacio en el inventario!");
    }
}
