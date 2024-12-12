using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string objectName = "Objeto"; // Nom per defecte en cas que em deixi algun
    public InventoryItem itemToAdd; // Objecte que s'agefirà a l'inventari
    public string interactionMessage; // Missatge que dira el personatge

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1)) // Clic dret
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (!string.IsNullOrEmpty(interactionMessage))
        {
            Debug.Log(interactionMessage); // Missatge del personatge
        }
        if (itemToAdd != null)
        {
            InventoryManager.Instance.AddItemToInventory(itemToAdd); // posa l'intem a l'inventari
        }

        
    }

}