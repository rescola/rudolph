using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; // El transform del personatge
    public float smoothSpeed = 0.125f; // Velocitat de suavitzat
    public Vector3 offset; // Desplaçament de la càmara respecte al jugador
    public float minX, maxX; // Limits de la camara en el eix X
    public float minY, maxY; // Limits de la camara en el eix Y (normalment bloquejat minY=maxY)

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
