using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class AirConsoleHandler : MonoBehaviour
{

    public Dictionary<int, ACInstrumentPlayer> players = new Dictionary<int, ACInstrumentPlayer>();
    public MainPlayer mainPlayer;
    public int primaryDeviceID = -1;
    public GameObject acInstrumentPlayerPrefab;

    void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onReady += OnReady;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
    }

    void OnReady(string code)
    {
        //Since people might be coming to the game from the AirConsole store once the game is live, 
        //I have to check for already connected devices here and cannot rely only on the OnConnect event 
        List<int> connectedDevices = AirConsole.instance.GetControllerDeviceIds();
        foreach (int deviceID in connectedDevices)
        {
            AddNewPlayer(deviceID);
        }
    }

    void OnMessage(int from, JToken data)
    {
        Debug.Log("message: " + data);
        if (from == primaryDeviceID)
        {
            mainPlayer.ButtonInput(data["action"].ToString());
            return;
        }
        //When I get a message, I check if it's from any of the devices stored in my device Id dictionary
        if (players.ContainsKey(from) && data["action"] != null)
        {
            //I forward the command to the relevant player script, assigned by device ID
            players[from].ButtonInput(data["action"].ToString());
        }
    }

    private void AddNewPlayer(int deviceID)
    {
        //if we haven't set up a primary yet, this person gets to be the primary
        if (deviceID == primaryDeviceID)
        {
            //why are you trying to add the primary second time, stop
            return;
        }
        if (primaryDeviceID == -1)
        {
            //you're the new rainbow dash
            primaryDeviceID = deviceID;
            Debug.Log($"Device [{primaryDeviceID}] is the new primary");
            return;

        }
        if (players.ContainsKey(deviceID))
        {
            return;
        }

        //Instantiate player prefab, store device id + player script in a dictionary
        GameObject newPlayer = Instantiate(acInstrumentPlayerPrefab) as GameObject;
        players.Add(deviceID, newPlayer.GetComponent<ACInstrumentPlayer>());
        players[deviceID].deviceTracker = deviceID;
        Debug.Log($"Device [{deviceID}] has been added as an instrument player");
    }

    void OnConnect(int device)
    {
        AddNewPlayer(device);
    }

    void OnDisconnect(int device)
    {
        removePlayer(device);
    }

    private void removePlayer(int deviceID)
    {
        if (deviceID == primaryDeviceID)
        {
            //make a backup player the new primary somehow
        }
    }
}
