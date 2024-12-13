using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public static DoorManager Instance;

    private Dictionary<string, DoorController> doors = new Dictionary<string, DoorController>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterDoor(DoorController door)
    {
        if (!doors.ContainsKey(door.DoorID))
        {
            doors.Add(door.DoorID, door);
            Debug.Log($"Porta registrada: {door.DoorID}");
        }
        else
        {
            Debug.LogWarning($"La porta amb el ID {door.DoorID} ja està registrada.");
        }
    }

    public DoorController GetDoorByID(string doorID)
    {
        doors.TryGetValue(doorID, out DoorController door);
        return door;
    }

    public void SaveDoorStates()
    {
        foreach (var door in doors.Values)
        {
            PlayerPrefs.SetInt($"{door.DoorID}_isOpen", door.isDoorOpen ? 1 : 0);
        }
    }

    public void LoadDoorStates()
    {
        foreach (var door in doors.Values)
        {
            door.isDoorOpen = PlayerPrefs.GetInt($"{door.DoorID}_isOpen", 0) == 1;
            door.UpdateDoorVisuals();
        }
    }
}
