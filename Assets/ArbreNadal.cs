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

    public string successMessage = "Vaya! Ya tienes todas las bolas! Como te prometí aquí tienes la llave para ver a Noel";
    public string progressMessage = "Tienes que encontrar {0} bolas más";
    public string alreadyCompleteMessage = "El arbol ya esta decorado!";

    private bool puzzleCompleted = false;

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
        if (puzzleCompleted)
        {
            // Mostra el missatge de l'elf dient que ja està decorat
            MessageManager.Instance.ShowMessage(alreadyCompleteMessage, 3f, elfCanvas, elfText);
            return;
        }

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
