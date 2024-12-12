using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    public string messageToDisplay; // Mensaje que el personaje dirá

    private void Start()
    {
        // Ejecutar el mensaje al iniciar la escena
        ShowMessage(messageToDisplay);
    }

    private void ShowMessage(string message)
    {
        Debug.Log(message); // Mostrar el mensaje en la consola (puedes personalizarlo para tu UI)
        // Aquí puedes agregar lógica para mostrarlo en la UI del juego
    }
}
