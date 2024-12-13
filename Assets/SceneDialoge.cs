using UnityEngine;
using UnityEngine.UI;

public class SceneDialogue : MonoBehaviour
{
    public Text dialogueText; // Referencia al component de text a la UI
    public string messageToDisplay;
    public float displayDuration = 5f; // Duració del missatge a la pantalla

    private void Start()
    {
        MessageManager.Instance.ShowMessage(messageToDisplay, displayDuration);
    }

    private void ShowMessage(string message)
    {
        if (dialogueText != null)
        {
            dialogueText.text = message; // Mostrar el misatge a la UI
            Invoke("ClearMessage", displayDuration); // Borra el misatge després del temps asignat
        }
        else
        {
            Debug.LogWarning("El component de text no està asignat.");
        }
    }

    private void ClearMessage()
    {
        if (dialogueText != null)
        {
            dialogueText.text = ""; // Neteja el text
        }
    }
}