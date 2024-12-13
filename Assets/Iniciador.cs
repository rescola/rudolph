using UnityEngine;

public class SceneInitializer2 : MonoBehaviour
{
    private void Start()
    {
        DoorManager.Instance.LoadDoorStates();
    }
}
