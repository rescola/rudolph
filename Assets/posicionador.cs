using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Posicionador : MonoBehaviour
{
    public float velocidad = 1f;
    private Vector3 objetivo;
    private Animator animator;
    private bool door = false;
    private DoorController currentDoor;

    public float maxY;
    public float minY;
    public float distancia;
    public GameObject canvas;

    private void Awake()
    {
        // Subscripció a l'esdeveniment de canvi d'escena
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Cancel·la la subscripció a l'esdeveniment
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        objetivo = transform.position;
        animator = GetComponent<Animator>();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (PlayerPositionManager.targetPosition != Vector3.zero)
        {
            Debug.Log($"Reposicionant jugador a la nova escena a: {PlayerPositionManager.targetPosition}");
            transform.position = PlayerPositionManager.targetPosition;
            // Reinicia l'objectiu perque no camini
            objetivo = transform.position;
            //FixCanvasOrientation();
            PlayerPositionManager.targetPosition = Vector3.zero; // Restableix la posició
        }
        else
        {
            Debug.Log("Cap posició objectiu trobada en carregar l'escena.");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetMouseButtonDown(0))
        {
            Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (clickedPosition.y < minY)
            {
                Debug.Log("Clic en la zona del inventari. Ignorat.");
                return;
            }

            objetivo = clickedPosition;

            if (objetivo.y > maxY)
            {
                objetivo.y = maxY;
            }

            objetivo.z = 0;

            Vector3 adjustedTarget;
            if (AdjustTargetIfBlocked(objetivo, out adjustedTarget))
            {
                objetivo = adjustedTarget;
            }
            else if (IsPathBlocked(objetivo))
            {
                Debug.Log("Cami bloquejat. No es mourà.");
                objetivo = transform.position;
                animator.SetBool("isMoving", false);
            }
        }

        float distanciaAlObjeto = Vector3.Distance(transform.position, objetivo);
        if (distanciaAlObjeto > distancia)
        {
            transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidad * Time.deltaTime);
            animator.SetBool("isMoving", true);

            if (objetivo.x > transform.position.x)
            {
                FixCanvasOrientation();
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                FixCanvasOrientation2();
                transform.localScale = new Vector3(-1, 1, 1);
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
            if (door && currentDoor != null)
            {
                currentDoor.ToggleDoor();
                door = false;
                currentDoor = null;
            }
        }
    }

    void FixCanvasOrientation()
    {
        canvas.transform.rotation = Quaternion.identity;
        Vector3 canvasScale = canvas.transform.localScale;
        canvasScale.x = -Mathf.Abs(canvasScale.x);
        canvas.transform.localScale = canvasScale;
    }

    void FixCanvasOrientation2()
    {
        canvas.transform.rotation = Quaternion.identity;
        Vector3 canvasScale = canvas.transform.localScale;
        canvasScale.x = Mathf.Abs(canvasScale.x);
        canvas.transform.localScale = canvasScale;
    }

    public void SetTargetPosition(Vector3 newTarget, DoorController doorController)
    {
        objetivo = newTarget;
        if (objetivo.y > maxY)
        {
            objetivo.y = maxY;
        }
        objetivo.z = 0;
        Debug.Log($"targetPosition final: {objetivo}");
        door = true;
        currentDoor = doorController;
    }

    private bool IsPathBlocked(Vector3 targetPosition)
    {
        float stopRadius = 0.2f;

        if (Vector3.Distance(transform.position, targetPosition) < stopRadius)
        {
            return true;
        }

        Vector3 direction = (targetPosition - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector3.Distance(transform.position, targetPosition));
        return hit.collider != null && hit.collider.CompareTag("Wall");
    }

    private bool AdjustTargetIfBlocked(Vector3 targetPosition, out Vector3 adjustedPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector3.Distance(transform.position, targetPosition));
        if (hit.collider != null && hit.collider.CompareTag("Wall"))
        {
            adjustedPosition = hit.point;
            return true;
        }

        adjustedPosition = targetPosition;
        return false;
    }
}

public static class PlayerPositionManager
{
    public static Vector3 targetPosition = Vector3.zero;
}
