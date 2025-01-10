using UnityEngine;

public class ChristmasTree : MonoBehaviour
{
    public InventoryItem bolaItem;
    public InventoryItem keyItem; // Clau que es donarà al jugador
    public int totalBolas = 12;

    public string successMessage = "Vaya! Ja estan les boles a l'arbre! Ben fet, pren aquesta clau!";
    public string progressMessage = "Encara falten boles a l'arbre! Tens {0} de {1}.";
    public string alreadyCompleteMessage = "L'arbre ja està complet!";

    private bool puzzleCompleted = false;

    public GameObject bolesDecoratives; // GameObject amb les boles decoratives

    private void Start()
    {
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


    private void OnMouseDown()
    {
        Debug.Log("Entro a l'script de l'arbre!");
        if (Input.GetMouseButtonDown(0)) // Clic dret
        {

            if (puzzleCompleted)
            {
                MessageManager.Instance.ShowMessage(alreadyCompleteMessage, 3f);
                return;
            }

            int currentCount = InventoryManager.Instance.GetItemCount(bolaItem.itemName);
            if (currentCount >= totalBolas)
            {
                puzzleCompleted = true;

                // Elimina les boles de l'inventari
                InventoryManager.Instance.RemoveItemFromInventory(bolaItem.itemName, currentCount);

                // Dona la clau
                InventoryManager.Instance.AddItemToInventory(keyItem);
                MessageManager.Instance.ShowMessage(successMessage, 5f);

                // Marca l'arbre com completat
                PlayerPrefs.SetInt("arbreDecorat", 1);
                PlayerPrefs.Save();

                // Activa les boles decoratives
                bolesDecoratives.SetActive(true);
            }
            else
            {
                // Mostra el progrés
                int remaining = totalBolas - currentCount;
                string message = string.Format(progressMessage, currentCount, totalBolas);
                MessageManager.Instance.ShowMessage(message, 3f);
            }
        }
    }
}
