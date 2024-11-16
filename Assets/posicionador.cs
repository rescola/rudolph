using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class posicionador : MonoBehaviour
{
    public float velocidad = 1f;
    private Vector3 objetivo;
    private Animator animator;

    void Start()
    {
        objetivo = transform.position;

        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            objetivo = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            objetivo.z = 0;
        }

        float distanciaAlObjeto = Vector3.Distance(transform.position, objetivo);


        transform.position = Vector3.MoveTowards(transform.position, objetivo, velocidad * Time.deltaTime);


        if (distanciaAlObjeto > 0.2f)
        {
            animator.SetBool("isMoving", true);

            if (objetivo.x > transform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1); // mou a la dreta
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1); // mou a la dreta invertint imatge
            }
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

    }
}
