using UnityEngine;

public class ResetPlayerPrefs : MonoBehaviour
{
    private void Start()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("[ResetPlayerPrefs] Todos los valores eliminados de PlayerPrefs.");
    }
}
