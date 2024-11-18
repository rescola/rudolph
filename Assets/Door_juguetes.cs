using UnityEngine;
using UnityEngine.SceneManagement;  // Per canviar escenes

public class DoorController : MonoBehaviour
{
    public Sprite closedDoorSprite; 
    public Sprite openDoorSprite;   

    public AudioClip openDoorSound; // revisar audio ja que no va
    public AudioClip closeDoorSound;

    // Estat porta
    private bool isDoorOpen = false; // Indica si la porta esta oberta/tancada

    // Distancia d'interaccio
    public float interactionDistance; // Distancia per iteracturar amb la porta

    private SpriteRenderer spriteRenderer;
    private AudioSource audioSource;
    private GameObject player;
    private Posicionador playerMovement;  // Referencia al script de moviment

    private Vector3 targetPosition;  // Posicio a la que s'ha de moure el player

    private void Start()
    {
        // Components necesaris
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();

        // Configurar sprite inicial com porta tancada
        spriteRenderer.sprite = closedDoorSprite;

        // Obtenir el objecte del jugador
        player = GameObject.FindGameObjectWithTag("Player");

        // Obtenir el script de moviment del jugador
        playerMovement = player.GetComponent<Posicionador>();
    }

    private void Update()
    {
        // Clic dret del mouse
        if (Input.GetMouseButtonDown(1))
        {
            // Comproba si el jugador esta aprop de la porta cuan fa clic dret
            if (Vector2.Distance(player.transform.position, transform.position) <= interactionDistance)
            {
                ToggleDoor();  // Si esta aprop obra / tanca directament
            }
            else
                // Nomes es moura si el click esta prop de la porta
                if (Vector2.Distance(Camera.main.ScreenToWorldPoint(Input.mousePosition), transform.position) <= interactionDistance)
            {
                // Si esta lluny es moura cap a la porta
                targetPosition = transform.position;
                Debug.Log("targetPosition crida: " + targetPosition);
                playerMovement.SetTargetPosition(targetPosition, this);  // Mou el player
            }
        }
        // Clic esquerra
        if (Input.GetMouseButtonDown(0))
        {
            // Si la porta esta oberta i fa click esquerra
            if (isDoorOpen && Vector2.Distance(player.transform.position, transform.position) <= interactionDistance)
            {
                // utilitcem un Raycast per veure si el click es sobre la porta
                RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    // Camvia l'escena
                    SceneManager.LoadScene("Sala_juguetes");
                }
                    
            }
        }
    }

    public void ToggleDoor()
    {
        // Cambia estat porta
        isDoorOpen = !isDoorOpen;

        // Actualiza el sprite segon el estat de la porta i fa soroll
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

    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Reprodueix el so una vegada
        }
    }
}