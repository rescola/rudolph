using UnityEngine;
using UnityEngine.UI;

public class SceneDialogue : MonoBehaviour
{
    public Text dialogueText; // Text associat a aquest personatge
    public Canvas dialogueCanvas; // Canvas associat a aquest personatge
    public string messageToDisplay;
    public float displayDuration = 5f;

    private void Start()
    {
        // Envia el canvas i el text específics
        MessageManager.Instance.ShowMessage(messageToDisplay, displayDuration, dialogueCanvas, dialogueText);
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