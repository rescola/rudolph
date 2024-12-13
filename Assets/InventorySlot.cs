using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
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
    /*
    public void OnSlotClicked()
    {
        if (currentItem != null)
        {
            if (Input.GetMouseButtonDown(1)) // Botón derecho
            {
                Debug.Log($"Usando el objeto: {currentItem.itemName}");
                currentItem.UseItem();
            }
            else if (Input.GetMouseButtonDown(0)) // Botón izquierdo
            {
                Debug.Log($"Descripción del objeto: {currentItem.description}");
                MessageManager.Instance.ShowMessage(currentItem.description, 3f);
            }
        }
        else
        {
            Debug.Log("Este slot está vacío.");
        }
    }
    */

    // Metode per veure si esta buit el slot
    public bool IsEmpty()
    {
        return currentItem == null;
    }

    public InventoryItem GetItem()
    {
        return currentItem; // Retorna l'objecte actual de l'slot
    }
    public void OnPointerClick(PointerEventData eventData) // segon intent, l'altre no funcionava
    {
        if (currentItem != null)
        {
            if (eventData.button == PointerEventData.InputButton.Right) // Clic dret
            {
                Debug.Log($"Utilitzant l'objecte: {currentItem.itemName}");
                currentItem.UseItem();
            }
            else if (eventData.button == PointerEventData.InputButton.Left) // Clic esquerre
            {
                Debug.Log($"Descripció de l'objecte: {currentItem.description}");
                MessageManager.Instance.ShowMessage(currentItem.description, 3f);
            }
        }
        else
        {
            Debug.Log("Aquest slot està buit.");
        }
    }
}
