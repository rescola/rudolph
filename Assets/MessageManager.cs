using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

    public Text messageText; // Referencia al text
    public Canvas canvas; // canvas compartit
    public float defaultDuration = 5f;

    private Coroutine currentCoroutine;

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
            Debug.LogWarning("Canvas o text no estan asignats.");
        }
    }

    public void ShowMessageLines(string[] lines, float linePause)
    {
        if (canvas != null && messageText != null)
        {
            // Cancela cualsevol misatge previ
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }

            canvas.enabled = true;
            currentCoroutine = StartCoroutine(DisplayLines(lines, linePause));
        }
    }

    private IEnumerator DisplayLines(string[] lines, float linePause)
    {
        foreach (string line in lines)
        {
            Debug.Log($"Mostra linia: {line}"); // Depuració per verificar l'ordre
            messageText.text = line; // Mostra la linia actual
            yield return new WaitForSeconds(linePause); // Pausa avans de la linia seguent
        }

        ClearMessage(); // Neteja el misatge després de mostrat totes les linies
    }

    private void ClearMessage()
    {
        if (messageText != null)
        {
            messageText.text = "";
            canvas.enabled = false;
        }

        currentCoroutine = null;
    }
}
