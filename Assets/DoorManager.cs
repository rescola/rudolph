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

            // Detectar si es la primera Execució
            if (!PlayerPrefs.HasKey("FirstRun"))
            {

                foreach (var doorID in doors.Keys)
                {
                    PlayerPrefs.DeleteKey($"{doorID}_isOpen");
                    Debug.Log($"[DoorManager] Clau borrada: {doorID}_isOpen");
                }

                PlayerPrefs.SetInt("FirstRun", 1);
                PlayerPrefs.Save();
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("[DoorManager] Carregant estat de les portes...");
        LoadDoorStates();
    }



    public void RegisterDoor(DoorController door)
    {
        if (!doors.ContainsKey(door.DoorID))
        {
            doors.Add(door.DoorID, door);
            Debug.Log($"[DoorManager] Porta registrada: {door.DoorID}");
        }
        else
        {
            Debug.Log($"[DoorManager] Porta amb ID {door.DoorID} ja està registrada...");
            doors[door.DoorID] = door; // Assegura que la referència sigui correcta
        }

        if (PlayerPrefs.HasKey($"{door.DoorID}_isOpen"))
        {
            door.isDoorOpen = PlayerPrefs.GetInt($"{door.DoorID}_isOpen", 0) == 1;
            Debug.Log($"[PlayerPrefs] Estat carregat per la porta {door.DoorID}: {door.isDoorOpen}");
        }
        else
        {
            Debug.LogWarning($"[PlayerPrefs] No s'ha trobat estat guardat per la porta {door.DoorID}. Usant estat predeterminat.");
        }

        // Actualitzar visuals després de carregar l'estat
        door.UpdateDoorVisuals();
    }



    private void LoadDoorState(DoorController door)
    {
        if (PlayerPrefs.HasKey($"{door.DoorID}_isOpen"))
        {
            door.isDoorOpen = PlayerPrefs.GetInt($"{door.DoorID}_isOpen", 0) == 1;
            Debug.Log($"[PlayerPrefs] Estat carregat per la porta {door.DoorID}: {door.isDoorOpen}");
        }
        else
        {
            // Si no hi ha estat guardat, fer servir l'estat de l'inspector
            PlayerPrefs.SetInt($"{door.DoorID}_isOpen", door.isDoorOpen ? 1 : 0);
            PlayerPrefs.Save();
            Debug.Log($"[PlayerPrefs] Estat per defecte de la porta {door.DoorID} guardat: {door.isDoorOpen}");
        }
    }



    public void SaveDoorState(DoorController door)
    {
        PlayerPrefs.SetInt($"{door.DoorID}_isOpen", door.isDoorOpen ? 1 : 0);
        PlayerPrefs.Save();
        Debug.Log($"[DoorManager] Estat de la porta {door.DoorID} guardat: {door.isDoorOpen}");
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
            if (door != null)
            {
                PlayerPrefs.SetInt($"{door.DoorID}_isOpen", door.isDoorOpen ? 1 : 0);
                Debug.Log($"[DoorManager] Estat de la porta {door.DoorID} guardat: {door.isDoorOpen}");
            }
            else
            {
                Debug.LogWarning($"[DoorManager] Porta null detectada durant el guardat.");
            }
        }
        PlayerPrefs.Save();
        Debug.Log("[DoorManager] Tots els estats de les portes guardats.");
    }


    public void LoadDoorStates()
    {
        foreach (var door in doors.Values)
        {
            if (PlayerPrefs.HasKey($"{door.DoorID}_isOpen"))
            {
                door.isDoorOpen = PlayerPrefs.GetInt($"{door.DoorID}_isOpen", 0) == 1;
                Debug.Log($"[PlayerPrefs] Estat cargat per la porta {door.DoorID}: {door.isDoorOpen}");
            }
            else
            {
                Debug.LogWarning($"[PlayerPrefs] No s'ha trobat un estat guardat de la porta {door.DoorID}. Es fara servir la de defecte.");
            }

            // Actualizar visuales
            door.UpdateDoorVisuals();
        }
        Debug.Log("[DoorManager] Tots els estats de les portes sincronitzats.");
    }

}
