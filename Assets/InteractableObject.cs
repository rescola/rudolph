using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string objectName = "Objete"; // Nom per defecte en cas que em deixi algun
    public InventoryItem itemToAdd; // Objecte que s'agefirà a l'inventari
    public string interactionMessage; // Missatge que dira el personatge
    public string emptyMessage = ""; // Missatge després de recollir l'objecte

    private bool hasBeenCollected = false; // Estat de l'objecte: recollit o no

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) // Clic dret
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (hasBeenCollected)
        {
            // Mostrar missatge si l'objecte ja ha estat recollit
            if (!string.IsNullOrEmpty(emptyMessage))
            {
                Debug.Log(emptyMessage);
                MessageManager.Instance.ShowMessage(emptyMessage, 3f);
            }
            return; // Finalitza aquí si l'objecte ja està recollit
        }
        if (!string.IsNullOrEmpty(interactionMessage))
        {
            Debug.Log(interactionMessage); // Missatge del personatge
            MessageManager.Instance.ShowMessage(interactionMessage, 3f);
        }
        if (itemToAdd != null)
        {
            InventoryManager.Instance.AddItemToInventory(itemToAdd); // posa l'intem a l'inventari
        }
        hasBeenCollected = true; // Marca l'objecte com recollit


    }

}