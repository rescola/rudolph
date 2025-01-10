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

    public int GetItemCount(string itemName)
    {
        int totalCount = 0;

        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty() && slot.GetItem().itemName == itemName)
            {
                totalCount += slot.GetCount(); // Suma el nombre d'items al comptador
            }
        }

        return totalCount; // Retorna el total d'items trobats
    }

    public void AddItemToInventory(InventoryItem item, int amount = 1)
    {
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty() && slot.GetItem() == item && item.isStackable)
            {
                slot.AddItem(item, amount);
                Debug.Log($"Afegit {amount} {item.itemName}(s).");
                return;
            }
        }

        foreach (InventorySlot slot in slots)
        {
            if (slot.IsEmpty())
            {
                slot.AddItem(item, amount);
                Debug.Log($"Item {item.itemName} afegit al inventari.");
                return;
            }
        }

        Debug.LogWarning("No hi ha espai a l'inventari!");
    }

    public void RemoveItemFromInventory(string itemName, int amount)
    {
        foreach (InventorySlot slot in slots)
        {
            if (!slot.IsEmpty() && slot.GetItem().itemName == itemName)
            {
                int currentCount = slot.GetCount();

                if (currentCount > amount)
                {
                    // Resta la quantitat especificada
                    slot.RemoveAmount(amount);
                    return;
                }
                else
                {
                    // Si la quantitat és igual o inferior, elimina completament l'objecte
                    amount -= currentCount;
                    slot.ClearSlot();
                }

                if (amount <= 0)
                    return; // Ja hem eliminat totes les boles necessàries
            }
        }

        Debug.LogWarning($"No s'han trobat suficients {itemName} a l'inventari per eliminar.");
    }

}
