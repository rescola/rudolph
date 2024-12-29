using UnityEngine;
using UnityEngine.SceneManagement; // Necessari per utilitzar el SceneManager

public class CameraFollow : MonoBehaviour
{
    public Transform player; // El transform del personatge
    public float smoothSpeed = 0.125f; // Velocitat de suavitzat
    public Vector3 offset = new Vector3(0, 0, -10); // Offset per defecte // Desplaçament de la càmara respecte al jugador
    //public float minX, maxX; // Limits de la camara en el eix X
    //public float minY, maxY; // Limits de la camara en el eix Y (normalment bloquejat minY=maxY)

    private float minX, maxX, minY, maxY;

    private void Start()
    {
        // Configurar inicialment els límits de la primera escena
        LoadLimitsCamara();

        // Subscriure's a l'event per detectar el canvi d'escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Desubscriure's de l'event quan es destrueixi l'objecte
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Actualitzar els límits de la càmera en carregar una nova escena
        LoadLimitsCamara();
    }

    private void LoadLimitsCamara()
    {
        LimitsCamara limits = FindObjectOfType<LimitsCamara>();
        if (limits != null)
        {
            minX = limits.minX;
            maxX = limits.maxX;
            minY = limits.minY;
            maxY = limits.maxY;
            Debug.Log($"[CameraFollow] Límits carregats: minX={minX}, maxX={maxX}, minY={minY}, maxY={maxY}");
        }
        else
        {
            Debug.LogWarning("[CameraFollow] No s'ha trobat un LimitsCamara a l'escena. Límits no configurats.");
        }
    }

    void LateUpdate()
    {
        if (player != null)
        {

            //obté la posició desitjada
            Vector3 desiredPosition = player.position + offset;

            //Limita la posició X de la camara dins dels marc definit
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);

            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);

            //Suavitza el moviment de la camara
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

            //Asigna la posició suavitzada a la camara
            transform.position = smoothedPosition;
        }
        
    }
}
