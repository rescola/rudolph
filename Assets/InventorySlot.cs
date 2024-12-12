using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    // Imagen del ícono del slot
    public Image icon;

    // Objeto actualmente en el slot
    private InventoryItem currentItem;

    // Método para agregar elementos al slot
    public void AddItem(InventoryItem item)
    {
        if (item == null)
        {
            Debug.LogWarning("Intentando agregar un objeto nulo al slot.");
            return;
        }

        currentItem = item; // Guardar objeto
        icon.sprite = item.icon; // Actualizar el ícono
        icon.color = Color.white; // Sembla que si no es negre no pinta
        icon.enabled = true; // Mostrar el ícono
    }

    // Método para limpiar el slot
    public void ClearSlot()
    {
        currentItem = null; // Eliminar la referencia al objeto
        icon.sprite = null; // Eliminar el ícono
        icon.enabled = false; // Ocultar el ícono
        GetComponent<Image>().color = Color.black;
    }

    // Método llamado al hacer clic en el slot
    public void OnSlotClicked()
    {
        if (currentItem != null)
        {
            Debug.Log($"Has seleccionado: {currentItem.name}");
            // TODO: Implementar algo que hacer con el objeto seleccionado
        }
        else
        {
            Debug.Log("Este slot está vacío.");
        }
    }

    // Método para verificar si el slot está vacío
    public bool IsEmpty()
    {
        return currentItem == null;
    }

    public InventoryItem GetItem()
    {
        return currentItem; // Devuelve el objeto actual en el slot
    }
}
