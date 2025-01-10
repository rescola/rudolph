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

                // TODO apareixer visuals de les boles
                Debug.Log("Mostrant boles de l'arbre");
            }
            else
            {
                MessageManager.Instance.ShowMessage(string.Format(progressMessage, currentCount, totalBolas), 3f);
            }
        }
    }
}
