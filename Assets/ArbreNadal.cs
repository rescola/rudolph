using UnityEngine;
using UnityEngine.UI;

public class ChristmasTree : MonoBehaviour
{
    public InventoryItem bolaItem; // Referència a l'item "bola"
    public InventoryItem keyItem; // Clau que es donarà al jugador
    public int totalBolas = 7; // Nombre total de boles necessàries

    public GameObject bolesDecoratives; // GameObject amb les boles decoratives
    public Canvas elfCanvas; // Canvas de l'elf
    public Text elfText; // Text de l'elf

    public string successMessage = "Vaya! Ya tienes todas las bolas! Como te prometí aquí tienes la llave para ver a Papa Noel!";
    public string progressMessage = "Tienes que encontrar {0} bolas más";
    public string alreadyCompleteMessage = "El arbol ya esta decorado!";

    private bool puzzleCompleted = false;

    private LayerMask interactableLayer;

    private void Start()
    {
        // Configura el LayerMask per l'interacció
        interactableLayer = LayerMask.GetMask("Interactable");

        // Comprovar si l'arbre ja ha estat decorat
        if (PlayerPrefs.GetInt("arbreDecorat", 0) == 1) // 1 = decorat
        {
            puzzleCompleted = true;
            bolesDecoratives.SetActive(true); // Activa les boles decoratives
        }
        else
        {
            bolesDecoratives.SetActive(false); // Assegura que les boles estiguin desactivades inicialment
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Clic dret
        {
            HandleTreeInteraction();
        }
    }

    private void HandleTreeInteraction()
    {
        // Llança un Raycast des de la càmera cap al punt del clic
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, interactableLayer);

        // Dibuixa el raycast per depuració
        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);

        if (hit.collider != null)
        {
            Debug.Log($"Raycast ha detectat: {hit.collider.gameObject.name}");

            if (hit.collider.gameObject == gameObject) // Si el Raycast col·lideix amb l'arbre
            {
                if (puzzleCompleted)
                {
                    MessageManager.Instance.ShowMessage(alreadyCompleteMessage, 3f, elfCanvas, elfText);
                }
                else
                {
                    ProcessPuzzleLogic();
                }
            }
        }
        else
        {
            Debug.Log("Raycast no detecta cap objecte.");
        }
    }

    private void ProcessPuzzleLogic()
    {
        // Obtenir el nombre de boles a l'inventari
        int currentCount = InventoryManager.Instance.GetItemCount(bolaItem.itemName);

        if (currentCount >= totalBolas)
        {
            puzzleCompleted = true;

            // Treure les boles de l'inventari
            InventoryManager.Instance.RemoveItemFromInventory(bolaItem.itemName, totalBolas);

            // Dona la clau al jugador
            InventoryManager.Instance.AddItemToInventory(keyItem);

            // Marca l'arbre com completat
            PlayerPrefs.SetInt("arbreDecorat", 1);
            PlayerPrefs.Save();

            // Activa les boles decoratives
            bolesDecoratives.SetActive(true);

            // Mostra el missatge d'èxit a l'elf
            MessageManager.Instance.ShowMessage(successMessage, 5f, elfCanvas, elfText);
        }
        else
        {
            // Mostra el progrés al canvas de l'elf
            int remaining = totalBolas - currentCount;
            string message = string.Format(progressMessage, remaining);
            MessageManager.Instance.ShowMessage(message, 3f, elfCanvas, elfText);
        }
    }
}
