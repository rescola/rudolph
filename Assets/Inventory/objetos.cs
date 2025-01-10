using UnityEngine;

[CreateAssetMenu(fileName = "New Carta", menuName = "Inventory/carta")]
public class Letter : InventoryItem
{
    public string texto1;
    public string texto2;
    public string texto3;
    public string texto4;
    public string texto5;

    public string targetDoorID; //porta que obrirà

    public override void UseItem()
    {
        Debug.Log("llegint carta...");

        // Guarda els textos en un array
        string[] textos = { texto1, texto2, texto3, texto4, texto5 };

        // Filtra els text buits
        string[] textosFiltrados = System.Array.FindAll(textos, texto => !string.IsNullOrEmpty(texto));

        // Crida al MessageManager per mostrar les linies una per una
        if (textosFiltrados.Length > 0)
        {
            MessageManager.Instance.ShowMessageLines(textosFiltrados, 7f); // Pausa entre texts
        }
        else
        {
            Debug.LogWarning("La carta no te contingut.");
        }
        // Desbloqueja porta despres de llegir
        UnlockDoor(targetDoorID);


    }

    private void UnlockDoor(string doorID)
    {
        DoorController door = DoorManager.Instance.GetDoorByID(doorID);
        if (door != null)
        {
            door.needs_key = false; // Desbloqueja la porta
            Debug.Log($"Porta {doorID} desbloquejada.");
        }
        else
        {
            Debug.LogWarning($"No es pot trobar porta amb el ID {doorID}. Mira que el ID sigui correcte i que la porta estigui registrada.");
        }
    }
}
