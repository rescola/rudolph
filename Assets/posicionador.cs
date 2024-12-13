using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posicionador : MonoBehaviour
{
    public float velocidad = 1f;
    private Vector3 objetivo;
    private Animator animator;
    private bool door = false;
    private DoorController currentDoor; // Referencia a la porta actual

    public float maxY; // Limit de les Y (això soluciona problemes si jugador marca una paret)
    public float minY;
    public float distancia; //Distancia entre personatge y punt marcat

    public GameObject canvas;

    void Start()
    {
        objetivo = transform.position;
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();

        }
        if (Input.GetMouseButtonDown(0))
        {
            //Vector3 adjustedTarget;
            //objetivo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 clickedPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (clickedPosition.y < minY)
            {
                Debug.Log("Clic en la zona del inventari. Ignorat!.");
                return; // Ignora clics en el área del inventario
            }

            objetivo = clickedPosition;

            if (objetivo.y > maxY)
            {
                objetivo.y = maxY;
            }

            if (objetivo.y < minY)
            {
                return; //no fa re si estem tocant inventari
            }

            objetivo.z = 0; // Seguretat de les Z (ha de ser 0)

            Vector3 adjustedTarget;
            // Ajusta l'objectiu si està bloquejat
            if (AdjustTargetIfBlocked(objetivo, out adjustedTarget))
            {
                objetivo = adjustedTarget;
            }
            else if (IsPathBlocked(objetivo))
            {
                Debug.Log("Cami bloquejat. No es mourà.");
                objetivo = transform.position;
                animator.SetBool("isMoving", false); // Es para
            }
        }

        // Mou jugador cap al objectiu
        float distanciaAlObjeto = Vector3.Distance(transform.position, objetivo);
        if (distanciaAlObjeto > distancia)
        {
            transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidad * Time.deltaTime);
            animator.SetBool("isMoving", true);

            // Direcció del personatge
            if (objetivo.x > transform.position.x)
            {
                FixCanvasOrientation();
                transform.localScale = new Vector3(1, 1, 1); // Mou a la dreta
                FixCanvasOrientation2();
            }
            else
            {
                FixCanvasOrientation2();
                transform.localScale = new Vector3(-1, 1, 1); // Mou a l'esquerra
                FixCanvasOrientation();

            }

        }
        else
        {
            animator.SetBool("isMoving", false); // Es para
            if (door == true && currentDoor != null)
            {
                currentDoor.ToggleDoor(); //Crida al metode per obrir/tancar la porta
                door = false; // resetea l'estat
                currentDoor = null; // reseteja la referencia
            }
        }
    }

    // Metode per asegurar que el canvas sempre miri a la dreta (es pugui llegir)
    void FixCanvasOrientation()
    {
            canvas.transform.rotation = Quaternion.identity;

            Vector3 canvasScale = canvas.transform.localScale;
            canvasScale.x = -canvasScale.x;
            canvas.transform.localScale = canvasScale;
            //canvas.transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixCanvasOrientation2()
    {
        canvas.transform.rotation = Quaternion.identity;

        Vector3 canvasScale = canvas.transform.localScale;
        canvasScale.x = Mathf.Abs(canvasScale.x);
        canvas.transform.localScale = canvasScale;
        //canvas.transform.localScale = new Vector3(-1, 1, 1);
    }

    // Mètode públic per canviar l'objectiu de moviment (utilitzat en DoorController)
    public void SetTargetPosition(Vector3 newTarget, DoorController doorController)
    {
        objetivo = newTarget;
        if (objetivo.y > maxY)
        {
            objetivo.y = maxY;
        }
        objetivo.z = 0;
        Debug.Log("targetPosition final: " + objetivo);
        door = true;
        currentDoor = doorController; // Asigna la referencia de la porta
    }

    private bool IsPathBlocked(Vector3 targetPosition)
    {
        float stopRadius = 0.2f; // Radi de parada

        if (Vector3.Distance(transform.position, targetPosition) < stopRadius)
        {
            return true; // para dins el radi de parada
        }
        Vector3 direction = (targetPosition - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector3.Distance(transform.position, targetPosition));
        return hit.collider != null && hit.collider.CompareTag("Wall");

        // Direcció del Raycast (cap al punt objectiu)
        //Vector3 direction = (targetPosition - transform.position).normalized;

        // Llença un Raycast des del personatge
        //RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector3.Distance(transform.position, targetPosition));
        //if (hit.collider != null && hit.collider.CompareTag("Wall")) // Comprova si el Raycast col·lisiona amb una paret
        //{
        //Debug.Log("Camino bloqueado por pared: " + hit.collider.name);
        //return true; // El camí està bloquejat
        //}

        //return false; // El camí és lliure
    }

    private bool AdjustTargetIfBlocked(Vector3 targetPosition, out Vector3 adjustedPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;

        // Realitza el Raycast i ajusta l'objetivo si es necesari
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Vector3.Distance(transform.position, targetPosition));
        if (hit.collider != null && hit.collider.CompareTag("Wall"))
        {
            adjustedPosition = hit.point; // Ajusta l'objetivo al punt de colisió
            return true;
        }

        adjustedPosition = targetPosition; // sense bloqueix
        return false;
    }


}