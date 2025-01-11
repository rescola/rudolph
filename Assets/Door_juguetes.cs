using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class DoorController : MonoBehaviour
{
    public Sprite closedDoorSprite;
    public Sprite openDoorSprite;

    public string DoorID; //indentificador unic per persistencia de l'estat obert/tancat

    public AudioClip openDoorSound;
    public AudioClip closeDoorSound;

    public bool needs_key = false; // Indica si es necesita una clau

    public bool isDoorOpen = false; // Estat de la porta

    public float interactionDistance = 2f; // Distancia per interactuar

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private GameObject player;
    private Posicionador playerMovement;

    private Vector3 targetPosition;


    public Canvas worldSpaceCanvas; // Canvas a World Space
    public Text messageText;

    public string keyNeededMessage = "Necesito una llave";
    public string targetSceneName = ""; // Nom de l'escena a carregar
    public Vector3 targetPlayerPosition; // Posició on apareixerà el jugador a l'escena de destinació

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<Posicionador>();

        if (player == null || playerMovement == null)
        {
            Debug.LogError("DoorController: No s'ha trobat el jugador o el component Posicionador.");
        }
        Debug.Log($"[DoorController] Porta {DoorID}: Estat inicial en l'inspector: {isDoorOpen}");
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError($"DoorController: No s'ha trobat un SpriteRenderer en {gameObject.name}");
        }
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError($"DoorController: No s'ha trobat un AudioSource en {gameObject.name}");
        }

        if (DoorManager.Instance != null)
        {
            DoorManager.Instance.RegisterDoor(this);
        }
        else
        {
            Debug.LogError("[DoorController] DoorManager no està configurat a l'escena!");
        }
        // Actualitzar visuals després del registre
        //UpdateDoorVisuals();

        //if (isDoorOpen)
        //{
        //    spriteRenderer.sprite = openDoorSprite;
        //}
        //else
        //{
        //    spriteRenderer.sprite = closedDoorSprite;
        //}

        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("DoorController: No s'ha trobat un GameObject amb la etiqueta 'Player'");
        }
        playerMovement = player.GetComponent<Posicionador>();
        if (playerMovement == null)
        {
            Debug.LogError("DoorController: No s'ha trobat un component 'Posicionador' en el jugador");
        }

        // Amaga canvas inicialment
        // worldSpaceCanvas.enabled = false;
    }

    public void UpdateDoorVisuals()
    {

        if (spriteRenderer == null)
        {
            Debug.LogError($"[DoorController] Porta {DoorID}: SpriteRenderer no localitzada.");
            return;
        }

        if (isDoorOpen)
        {
            if (openDoorSprite != null)
            {
                spriteRenderer.sprite = openDoorSprite;
                Debug.Log($"[DoorController] Porta {DoorID}: Visual actualitzada a OBERTA.");
            }
            else
            {
                Debug.LogError($"[DoorController] Porta {DoorID}: Sprite per la porta oberta no asignada.");
            }
        }
        else
        {
            if (closedDoorSprite != null)
            {
                spriteRenderer.sprite = closedDoorSprite;
                Debug.Log($"[DoorController] Porta {DoorID}: Visual actualitzada a TANCADA.");
            }
            else
            {
                Debug.LogError($"[DoorController] Porta {DoorID}: Sprite per la porta tancada no asignada.");
            }
        }
    }



    public void UnlockDoor()
    {
        needs_key = false;
        Debug.Log($"Porta {DoorID} desbloquejada.");
    }

    private void Update()
    {
        // Clic dret per obrir / tancar porta
        if (Input.GetMouseButtonDown(1))
        {
            if (Vector2.Distance(player.transform.position, transform.position) <= interactionDistance)
            {
                ToggleDoor(); // Si esta aprop, obre o tanca porta
            }
            else
            {
                // Si esta lluny, es mou cap a la porta
                if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) <= interactionDistance)
                {
                    targetPosition = transform.position;
                    playerMovement.SetTargetPosition(targetPosition, this);
                }
            }
        }

        // Clic esquerra per interactuar amb la porta (entra si esta oberta)
        if (Input.GetMouseButtonDown(0))
        {
            if (isDoorOpen && Vector2.Distance(player.transform.position, transform.position) <= interactionDistance)
            {
                // Raycast per veure si el click es sobre la porta
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    // Guarda l'estat correcte abans de canviar d'escena
                    DoorManager.Instance.SaveDoorState(this);
                    // Passa la posició al gestor global
                    PlayerPositionManager.targetPosition = targetPlayerPosition;
                    // Canvi escena
                    //DoorManager.Instance.SaveDoorStates(); //salva l'estat de les portes
                    Debug.Log($"Canviant a l'escena {targetSceneName} amb posició objectiu {targetPlayerPosition}");
                    SceneManager.LoadScene(targetSceneName);
                }
            }
        }
    }

    public void ToggleDoor()
    {
        if (!needs_key)
        {
            isDoorOpen = !isDoorOpen;

            // Actualitzar visuals
            UpdateDoorVisuals();

            // Guardar l'estat de la porta
            DoorManager.Instance.SaveDoorState(this);

            Debug.Log($"[DoorController] Porta {DoorID}: Estat canviat a {(isDoorOpen ? "OBERT" : "TANCAT")} i guardat.");

            // Reprodueix el so corresponent
            PlaySound(isDoorOpen ? openDoorSound : closeDoorSound);
        }
        else
        {
            // Si la porta necessita clau, comprovar si el jugador té l'objecte necessari
            InventoryManager inventoryManager = InventoryManager.Instance;
            if (inventoryManager != null && inventoryManager.HasItem("llave_santa"))
            {
                // Desbloquejar la porta
                needs_key = false;
                isDoorOpen = true;

                // Treure la clau de l'inventari
                inventoryManager.RemoveItemFromInventory("llave_santa", 1);

                // Mostrar missatge d'èxit
                ShowMessage("¡La puerta se ha abierto!");

                // Actualitzar visuals
                UpdateDoorVisuals();

                // Guardar l'estat de la porta
                DoorManager.Instance.SaveDoorState(this);

                Debug.Log($"[DoorController] Porta {DoorID} desbloquejada i oberta.");

                // Reprodueix el so d'obertura
                PlaySound(openDoorSound);
            }
            else
            {
                // Si no té la clau, mostrar el missatge necessari
                ShowMessage(keyNeededMessage);
            }
        }
    }



    private void ShowMessage(string message)
    {
        MessageManager.Instance.ShowMessage(message, 4f);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
        else if (clip == null)
        {
            Debug.LogWarning($"[DoorController] No s'ha assignat cap clip d'àudio per la porta {DoorID}");
        }
    }


}

