using UnityEngine;

public class CanvasFollowCamera : MonoBehaviour
{
    public Camera mainCamera; // Referencia a la camara principal
    public Vector3 offset = new Vector3(0, -2, 5); // Posició relativa respecte a la camara

    void Update()
    {
        // Asegurem que el Canvas es mantingui a una posició fixa relativa a la camara
        transform.position = mainCamera.transform.position + offset;

        // Asegurem que el canvas sempre miri a la camara
        transform.LookAt(mainCamera.transform);
    }
}