using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    public string messageToDisplay;

    private void Start()
    {
        ShowMessage(messageToDisplay);
    }

    private void ShowMessage(string message)
    {
        Debug.Log(message); 
        // TODO Misatge per afegir
    }
}
