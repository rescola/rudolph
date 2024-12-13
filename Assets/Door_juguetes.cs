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

    private void Start()
    {
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

        if (isDoorOpen)
        {
            spriteRenderer.sprite = openDoorSprite;
        }
        else
        {
            spriteRenderer.sprite = closedDoorSprite;
        }

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

        if (DoorManager.Instance != null)
        {
            // Registra la porta actual al DoorManager
            DoorManager.Instance.RegisterDoor(this);

        }
        else
        {
            Debug.LogError("DoorController: DoorManager.Instance es null. Mira que DoorManager estigui a l'escena.");
        }

        // Amaga canvas inicialment
        // worldSpaceCanvas.enabled = false;
    }

    public void UpdateDoorVisuals() //al carregar escena cridar a aquesta funcio per pintar les portes segons l'estat
    {
        spriteRenderer.sprite = isDoorOpen ? openDoorSprite : closedDoorSprite;
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
                    // Canvi escena
                    DoorManager.Instance.SaveDoorStates(); //salva l'estat de les portes
                    SceneManager.LoadScene(targetSceneName);
                }
            }
        }
    }

    public void ToggleDoor()
    {
        if (!needs_key)
        {
            isDoorOpen = !isDoorOpen; // Canviar estat porta

            if (isDoorOpen)
            {
                spriteRenderer.sprite = openDoorSprite;
                PlaySound(openDoorSound);
            }
            else
            {
                spriteRenderer.sprite = closedDoorSprite;
                PlaySound(closeDoorSound);
            }
        }
        else
        {
            ShowMessage(keyNeededMessage);
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }


    private void ShowMessage(string message)
    {
        //worldSpaceCanvas.enabled = true;
        //messageText.text = message;

        // Misatge desapareix despres de 4 segons
        //StartCoroutine(HideMessageAfterDelay(4f));
        MessageManager.Instance.ShowMessage(message, 4f);
    }

    private IEnumerator HideMessageAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        //worldSpaceCanvas.enabled = false; // Ocultar el Canvas
    }
}

