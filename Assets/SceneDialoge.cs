using UnityEngine;
using UnityEngine.UI;

public class SceneDialogue : MonoBehaviour
{
    public Text dialogueText; // Referencia al componente de texto en la UI
    public string messageToDisplay;
    public float displayDuration = 5f; // Duración del mensaje en pantalla

    private void Start()
    {
        ShowMessage(messageToDisplay);
    }

    private void ShowMessage(string message)
    {
        if (dialogueText != null)
        {
            dialogueText.text = message; // Mostrar el mensaje en la UI
            Invoke("ClearMessage", displayDuration); // Borrar el mensaje después del tiempo especificado
        }
        else
        {
            Debug.LogWarning("El componente de texto no está asignado.");
        }
    }

    private void ClearMessage()
    {
        if (dialogueText != null)
        {
            dialogueText.text = ""; // Limpiar el texto
        }
    }
}