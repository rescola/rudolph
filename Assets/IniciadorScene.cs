using UnityEngine;

public class SceneInitializer : MonoBehaviour
{
    private static SceneInitializer instance;

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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
