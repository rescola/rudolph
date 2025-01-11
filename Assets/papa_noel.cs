using UnityEngine;
using UnityEngine.UI;

public class SantaPuzzle : MonoBehaviour
{
    public Sprite tiedSantaSprite; // Sprite de Papà Noel lligat
    public Sprite untiedSantaSprite; // Sprite de Papà Noel deslligat
    public string tiedMessage = "¡ESTO ES UN SECUESTRO! ¡NO, ESPERA, YO SOY LA VÍCTIMA! BUSCA ALGO PARA CORTAR LAS CUERDAS...."; // Missatge inicial
    public string untiedMessage = "¡MUCHÍSIMAS GRACIAS! AHORA QUE ESTOY LIBRE, DEBEMOS RESCATAR A RUDOLF... PERO ESO SERÁ EN LA AVENTURA COMPLETA. ¡LA DEMO TERMINA AQUÍ! ¡GRACIAS POR JUGAR!"; // Missatge final
    public InventoryItem scissorsItem; // L'ítem de les tisores
    public Canvas santaCanvas; // Canvas per als missatges
    public Text santaText; // Text dels missatges
    public Image santaBackground; // Fons específic per a Papà Noel

    private bool isFreed = false; // Estat de Papà Noel (lligat o deslligat)
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("No s'ha trobat el component SpriteRenderer.");
            return;
        }

        // Comença amb el sprite de Papà Noel lligat
        spriteRenderer.sprite = tiedSantaSprite;

        // Configura el fons com a transparent al principi
        if (santaBackground != null)
        {
            santaBackground.color = new Color(0, 0, 0, 0); // Fons transparent
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) // Clic dret
        {
            Debug.Log("Clic dret detectat.");
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider.gameObject == this.gameObject)
            {
                Debug.Log("Papà Noel clicat.");
                Interact();
            }
        }
    }

    private void Interact()
    {
        if (isFreed)
        {
            Debug.Log("Papà Noel ja està deslligat.");
            MessageManager.Instance.ShowMessage(untiedMessage, 5f, santaCanvas, santaText, santaBackground);
        }
        else
        {
            Debug.Log($"Comprovant tisores a l'inventari: {InventoryManager.Instance.HasItem(scissorsItem.itemName)}");
            if (InventoryManager.Instance.HasItem(scissorsItem.itemName))
            {
                Debug.Log("Tisores detectades. Papà Noel es deslliga.");
                isFreed = true;
                spriteRenderer.sprite = untiedSantaSprite;

                InventoryManager.Instance.RemoveItemFromInventory(scissorsItem.itemName, 1);
                Debug.Log("Tisores eliminades de l'inventari.");

                MessageManager.Instance.ShowMessage(untiedMessage, 5f, santaCanvas, santaText, santaBackground);
            }
            else
            {
                Debug.Log("No tens tisores. Mostrant missatge inicial.");
                MessageManager.Instance.ShowMessage(tiedMessage, 5f, santaCanvas, santaText, santaBackground);
            }
        }
    }
}
