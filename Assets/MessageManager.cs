using UnityEngine;
using UnityEngine.UI;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

    public Text messageText; // Referencia al text
    public Canvas canvas; // canvas compartit
    public float defaultDuration = 5f;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ShowMessage(string message, float duration = -1)
    {
        if (canvas != null && messageText != null)
        {
            canvas.enabled = true;
            messageText.text = message;

            // Si no s'especifica la durada, agafa la predeterminada
            float delay = (duration > 0) ? duration : defaultDuration;
            CancelInvoke(); // Evita misatjes simultanis
            Invoke("ClearMessage", delay);
        }
        else
        {
            Debug.LogWarning("Canvas o texto no están asignados.");
        }
    }

    private void ClearMessage()
    {
        if (messageText != null)
        {
            messageText.text = "";
            canvas.enabled = false;
        }
    }
}
