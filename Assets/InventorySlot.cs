using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IPointerClickHandler
{

    public Image icon;
    public Text countText; // Mostrar el comptador

    // Objecte actual a l'slot
    private InventoryItem currentItem;
    private int count = 0; // Nombre d'elements al slot

    // Metode per agregar elements
    public void AddItem(InventoryItem item, int amount = 1)
    {
        if (icon == null)
        {
            Debug.LogError($"El camp 'icon' del slot és NULL al InventorySlot del jocobjecte '{gameObject.name}'.");
            return;
        }
        if (item == null) 
        {
            Debug.LogWarning("Intenta afegir un objecte nul al slot");
            return;
        }
        if (currentItem == item && item.isStackable)
        {
            count += amount;
        }
        else if (currentItem == null)
        {
            if (item.icon == null)
            {
                Debug.LogError($"L'objecte {item.itemName} no té una icona assignada.");
                return;
            }
            currentItem = item;
            icon.sprite = item.icon;
            icon.enabled = true;
            icon.color = Color.white; // Sembla que si no es negre no pinta
            count = amount;
            //UpdateUI();
        }
    }

    private void UpdateUI()
    {
        if (countText != null)
        {
            if (currentItem != null && currentItem.isStackable)
            {
                countText.text = count.ToString();
                countText.enabled = true;
            }
            else
            {
                countText.text = "";
                countText.enabled = false;
            }
        }
        else
        {
            Debug.LogWarning("countText no està assignat a l'Inspector.");
        }
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

    public int GetCount()
    {
        return count; // Retorna el nombre d'items d'aquest slot
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

    public void RemoveAmount(int amount)
    {
        count -= amount;

        if (count <= 0)
        {
            ClearSlot(); // Elimina l'element completament si el comptador arriba a 0
        }
        else
        {
            UpdateUI(); // Actualitza la UI si encara queden elements
        }
    }

}
