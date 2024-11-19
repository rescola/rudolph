using UnityEngine;

public class CanvasFollowCamera : MonoBehaviour
{
    public Camera mainCamera; // Referencia a la cámara principal
    public Vector3 offset = new Vector3(0, -2, 5); // Posición relativa respecto a la cámara

    void Update()
    {
        // Aseguramos que el Canvas se mantenga a una posición fija relativa a la cámara
        transform.position = mainCamera.transform.position + offset;

        // Aseguramos que el Canvas siempre mire a la cámara
        transform.LookAt(mainCamera.transform);
    }
}