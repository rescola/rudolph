using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageManager : MonoBehaviour
{
    public static MessageManager Instance;

    public Text defaultMessageText; // Text per defecte (jugador)
    public Canvas defaultCanvas; // Canvas per defecte (jugador)
    public Image defaultBackground; // Fons per defecte (opcional)
    public float defaultDuration = 5f;
    private Canvas canvasToClear;
    private Text textToClear;
    private Image backgroundToClear;

    private Coroutine currentCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            Debug.Log("Instància del MessageManager creada.");
        }
        else
        {
            Debug.LogWarning("S'ha intentat crear una segona instància del MessageManager.");
            Destroy(gameObject);
        }
    }

    public void ShowMessage(string message, float duration = -1, Canvas targetCanvas = null, Text targetText = null, Image targetBackground = null)
    {
        Canvas canvasToUse = targetCanvas ?? defaultCanvas;
        Text textToUse = targetText ?? defaultMessageText;
        Image backgroundToUse = targetBackground ?? defaultBackground;


        if (canvasToUse != null && textToUse != null)
        {
            canvasToUse.enabled = true;
            textToUse.text = message;

            if (backgroundToUse != null)
            {
                backgroundToUse.color = new Color(0, 0, 0, 0.9f); // Negre gairebé opac
            }

            // Guarda els valors per a ClearMessage
            canvasToClear = canvasToUse;
            textToClear = textToUse;
            backgroundToClear = backgroundToUse;

            float delay = (duration > 0) ? duration : defaultDuration;

            CancelInvoke(nameof(ClearMessage));
            Invoke(nameof(ClearMessage), delay);
        }
        else
        {
            Debug.LogWarning("Canvas o text no estan assignats.");
        }
    }



    private void ClearMessage()
    {
        ClearMessage(canvasToClear, textToClear, backgroundToClear);
    }

    private void ClearMessage(Canvas targetCanvas, Text targetText, Image targetBackground)
    {
        Debug.Log($"Netejant missatge del Canvas: {targetCanvas?.name}, Text: {targetText?.text}, Background: {targetBackground?.name}");

        if (targetText != null)
        {
            targetText.text = ""; // Buida el text
        }

        if (targetCanvas != null)
        {
            targetCanvas.enabled = false; // Desactiva el Canvas completament
        }

        if (targetBackground != null)
        {
            targetBackground.color = new Color(0, 0, 0, 0); // Fa transparent el fons
        }
    }


    public void ShowMessageLines(string[] lines, float linePause, Canvas targetCanvas = null, Text targetText = null, Image targetBackground = null)
    {
        // Usa els valors per defecte si no es proporcionen
        Canvas canvasToUse = targetCanvas ?? defaultCanvas;
        Text textToUse = targetText ?? defaultMessageText;
        Image backgroundToUse = targetBackground ?? defaultBackground;

        if (canvasToUse != null && textToUse != null)
        {
            // Cancel·la qualsevol missatge previ
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
                currentCoroutine = null;
            }

            canvasToUse.enabled = true;

            // Mostra el fons negre si està assignat
            if (backgroundToUse != null)
            {
                backgroundToUse.color = new Color(0, 0, 0, 0.8f); // Negre semitransparent
            }

            currentCoroutine = StartCoroutine(DisplayLines(lines, linePause, canvasToUse, textToUse, backgroundToUse));
        }
    }

    private IEnumerator DisplayLines(string[] lines, float linePause, Canvas targetCanvas, Text targetText, Image targetBackground)
    {
        foreach (string line in lines)
        {
            Debug.Log($"Mostra línia: {line}");
            targetText.text = line;
            yield return new WaitForSeconds(linePause);
        }

        ClearMessage(targetCanvas, targetText, targetBackground); // Neteja el missatge després de mostrar totes les línies
    }
}
