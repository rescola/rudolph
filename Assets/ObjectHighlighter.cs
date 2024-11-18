using UnityEngine;
using UnityEngine.UI;

public class ObjectHighlighter : MonoBehaviour
{
    public Text objectNameText;

    void Update()
    {
        // Obté la posició del mouse
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Mira desde la posició del mouse que hi ha sota
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            // Mira si l'objecte te un component `InteractableObject`
            InteractableObject interactable = hit.collider.GetComponent<InteractableObject>();
            if (interactable != null)
            {
                objectNameText.text = interactable.objectName; // Mostra el nom
                return;
            }
        }

        // Si no hi ha objecte borra el text
        objectNameText.text = "";
    }
}
