using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;
using System;

public enum PlayerAction {
  LEFT,
  RIGHT,
  UP,
  DOWN,
  SELECT,
}

public enum PlayerType {
  MAIN,
  PIANO,
  DRUM,
  BASS,
  TRUMPET,
  UNKNOWN,
}

public class AirConsoleHandler : MonoBehaviour
{

    public Dictionary<int, ACInstrumentPlayer> players = new Dictionary<int, ACInstrumentPlayer>();
    public MainPlayer mainPlayer;
    public int primaryDeviceID = -1;
    public GameObject acInstrumentPlayerPrefab;
    public InstrumentRhythmHandler pianoHandler;
    public InstrumentRhythmHandler drumHandler;
    public InstrumentRhythmHandler bassHandler;
    public InstrumentRhythmHandler trumpetHandler;
    private List<InstrumentRhythmHandler> allHandlers;

    void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onReady += OnReady;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDisconnect += OnDisconnect;
        allHandlers = new List<InstrumentRhythmHandler>();
        allHandlers.Add(pianoHandler);
        allHandlers.Add(drumHandler);
        allHandlers.Add(bassHandler);
        allHandlers.Add(trumpetHandler);
    }

    public void FakeStart()
    {
        
        try
        {
            
                List<int> connectedDevices = AirConsole.instance.GetControllerDeviceIds();
                foreach (int deviceID in connectedDevices)
                {
                    AddNewPlayer(deviceID);
                }
            
        } catch (Exception e)
        {
            Debug.Log("Air console wasn't ready, not calling initial setup");
        }
        
    }

    public void onAction(int fromDeviceId, PlayerAction action) {
      Debug.Log($"Player {fromDeviceId} did action: {action}");
      // Add actions here
    }
    
    public void sendPlayerType(int toDeviceId, PlayerType type) {
      Debug.Log($"Sending player {toDeviceId} to type {type}");
      
      var message = new {
        message_type = "PLAYER_TYPE",
        player_type = type.ToString(),
      };

      AirConsole.instance.Message(toDeviceId, message);
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
            sendPlayerType(deviceID, PlayerType.MAIN);
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
        PlayerType playerType = chooseInstrumentForPlayer(newPlayer.GetComponent<ACInstrumentPlayer>());
        sendPlayerType(deviceID, playerType);
        Debug.Log($"Device [{deviceID}] has been added as an instrument player");
    }

    private PlayerType chooseInstrumentForPlayer(ACInstrumentPlayer player)  {
        if (pianoHandler.numberofActivePlayers == 0)
        {
            player.insRhythmHandler = pianoHandler;
            pianoHandler.numberofActivePlayers++;
            pianoHandler.numberOfPlayers.text = "Players: " + pianoHandler.numberofActivePlayers.ToString();
            pianoHandler.startAudio();
            Debug.Log("Added a pianist");
            return PlayerType.PIANO;
        } else if (drumHandler.numberofActivePlayers == 0)
        {
            player.insRhythmHandler = drumHandler;
            drumHandler.numberofActivePlayers++;
            drumHandler.numberOfPlayers.text = "Players: " + drumHandler.numberofActivePlayers.ToString();
            drumHandler.startAudio();
            Debug.Log("Added a drummer");
            return PlayerType.DRUM;
        } else if (bassHandler.numberofActivePlayers == 0)
        {
            player.insRhythmHandler = bassHandler;
            bassHandler.numberofActivePlayers++;
            bassHandler.numberOfPlayers.text = "Players: " + bassHandler.numberofActivePlayers.ToString();
            bassHandler.startAudio();
            Debug.Log("Added a bassist");
            return PlayerType.BASS;
        } else if (trumpetHandler.numberofActivePlayers == 0) {
            player.insRhythmHandler = trumpetHandler;
            trumpetHandler.numberofActivePlayers++;
            trumpetHandler.numberOfPlayers.text = "Players: " + trumpetHandler.numberofActivePlayers.ToString();
            trumpetHandler.startAudio();
            Debug.Log("Added a trumpeter");
            return PlayerType.TRUMPET;
        } else
        {
            InstrumentRhythmHandler chosenhandler = allHandlers[UnityEngine.Random.Range(0, 4)];
            player.insRhythmHandler = chosenhandler;
            chosenhandler.numberofActivePlayers++;
            chosenhandler.numberOfPlayers.text = "Players: " + chosenhandler.numberofActivePlayers.ToString();
            Debug.Log("Added a player to a random handler");

        }
        return PlayerType.UNKNOWN;
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
