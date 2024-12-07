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
            // Si apretem ESC tanca l'aplicació TODO canvas amb menu
            Application.Quit();

        }
        if (Input.GetMouseButtonDown(0))
        {
            objetivo = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            if (objetivo.y > maxY)
            {
                objetivo.y = maxY;
            }

            objetivo.z = 0; // Seguretat de les Z (ha de ser 0)
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
}