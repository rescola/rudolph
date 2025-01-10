using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string objectID; // Identificador únic per a l'objecte
    public string objectName = "Objete"; // Nom per defecte en cas que em deixi algun
    public InventoryItem itemToAdd; // Objecte que s'agefirà a l'inventari
    public string interactionMessage; // Missatge que dira el personatge
    public string emptyMessage = ""; // Missatge després de recollir l'objecte

    private bool hasBeenCollected = false; // Estat de l'objecte: recollit o no
    public bool disappearOnCollect = false; // Si desapareix l'objecte de l'escena al recollir-lo

    private void Start()
    {
        // Comprova si l'objecte ja ha estat recollit
        if (PlayerPrefs.GetInt(objectID, 0) == 1) // 1 = recollit
        {
            hasBeenCollected = true;
            if (disappearOnCollect)
            {
                gameObject.SetActive(false); // Desactiva l'objecte si ja va ser recollit
            }
        }
    }

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

        // Guarda l'estat de l'objecte
        PlayerPrefs.SetInt(objectID, 1);
        PlayerPrefs.Save();

        // Desactiva l'objecte de l'escena
        if (disappearOnCollect)
        {
            gameObject.SetActive(false); // Desactiva el GameObject
        }


    }

}