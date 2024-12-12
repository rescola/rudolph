using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("Emma_room");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
