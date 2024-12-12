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
        // Verifica si el ítem ya está en el inventario
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty() && slot.GetItem() == item) // Compara con el objeto existente
            {
                Debug.Log($"Ya no hay nada más aquí");
                return; // No añade duplicados
            }
        }
        // Busca un slot vacío
        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.AddItem(item); // Añade el objeto al slot
                Debug.Log($"Item {item.itemName} añadido al inventario.");
                return;
            }
        }

        Debug.LogWarning("No hay espacio en el inventario!");
    }
}
