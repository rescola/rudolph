using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{

    public Image icon;

    // Objecte actual a l'slot
    private InventoryItem currentItem;

    // Metode per agregar elements
    public void AddItem(InventoryItem item)
    {
        if (item == null)
        {
            Debug.LogWarning("Intenta afegir un objecte nul al slot");
            return;
        }

        currentItem = item;
        icon.sprite = item.icon;
        icon.color = Color.white; // Sembla que si no es negre no pinta
        icon.enabled = true; // Mostrar l'icono
    }

    // Metode per netejar l'slot (TODO)
    public void ClearSlot()
    {
        currentItem = null; 
        icon.sprite = null; 
        icon.enabled = false;
        GetComponent<Image>().color = Color.black; // torna a posar negre
    }

    // Metode al fer click en l'slot
    public void OnSlotClicked()
    {
        if (currentItem != null)
        {
            Debug.Log($"Has seleccionado: {currentItem.name}");
            // TODO: Aqui implementar que fer amb l'element seleccionat
        }
        else
        {
            Debug.Log("Este slot está vacío.");
        }
    }

    // Metode per veure si esta buit el slot
    public bool IsEmpty()
    {
        return currentItem == null;
    }

    public InventoryItem GetItem()
    {
        return currentItem; // Retorna l'objecte actual de l'slot
    }
}
