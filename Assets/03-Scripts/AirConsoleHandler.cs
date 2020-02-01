using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;
using UnityEngine.UI;

public enum PlayerAction {
  LEFT,
  RIGHT,
  UP,
  DOWN,
  SELECT,
}

public enum PlayerType {
  MAIN,
  INSTRUMENT_ONE,
  INSTRUMENT_TWO,
  INSTRUMENT_THREE,
  INSTRUMENT_FOUR,
}

public interface IHostController {
  void onAction(int fromDeviceId, PlayerAction action);
  void sendPlayerType(int toDeviceId, PlayerType type);
  void sendHitFeedback(int toDeviceId, bool isSuccessful);
}

public class AirConsoleHandler : MonoBehaviour, IHostController
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
    
    public void onAction(int fromDeviceId, PlayerAction action) {
      Debug.Log($"Player {fromDeviceId} did action: {action}");
      // Add actions here
    }
    
    public void sendPlayerType(int toDeviceId, PlayerType type) {
      Debug.Log($"Sending player {toDeviceId} to type {type}");
      
      var message = new {
        player = type,
      };

      AirConsole.instance.Message(toDeviceId, "message");
    }
    
    public void sendHitFeedback(int toDeviceId, bool isSuccessful) {
      Debug.Log($"Sending Player {toDeviceId} success: {isSuccessful}");
      
      var message = new {
        hit = isSuccessful,
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
        chooseInstrumentForPlayer(newPlayer.GetComponent<ACInstrumentPlayer>());
        Debug.Log($"Device [{deviceID}] has been added as an instrument player");
    }

    private void chooseInstrumentForPlayer(ACInstrumentPlayer player)  {
        if (pianoHandler.numberofActivePlayers == 0)
        {
            player.insRhythmHandler = pianoHandler;
            pianoHandler.numberofActivePlayers++;
            pianoHandler.numberOfPlayers.text = "Players: " + pianoHandler.numberofActivePlayers.ToString();
            Debug.Log("Added a pianist");
        } else if (drumHandler.numberofActivePlayers == 0)
        {
            player.insRhythmHandler = drumHandler;
            drumHandler.numberofActivePlayers++;
            drumHandler.numberOfPlayers.text = "Players: " + drumHandler.numberofActivePlayers.ToString();
            Debug.Log("Added a drummer");
        } else if (bassHandler.numberofActivePlayers == 0)
        {
            player.insRhythmHandler = bassHandler;
            bassHandler.numberofActivePlayers++;
            bassHandler.numberOfPlayers.text = "Players: " + bassHandler.numberofActivePlayers.ToString();
            Debug.Log("Added a bassist");
        } else if (trumpetHandler.numberofActivePlayers == 0) {
            player.insRhythmHandler = trumpetHandler;
            trumpetHandler.numberofActivePlayers++;
            trumpetHandler.numberOfPlayers.text = "Players: " + trumpetHandler.numberofActivePlayers.ToString();
            Debug.Log("Added a trumpeter");
        } else
        {
            InstrumentRhythmHandler chosenhandler = allHandlers[Random.Range(0, 3)];
            player.insRhythmHandler = chosenhandler;
            chosenhandler.numberofActivePlayers++;
            chosenhandler.numberOfPlayers.text = "Players: " + chosenhandler.numberofActivePlayers.ToString();
            Debug.Log("Added a player to a random handler");

        }
        

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
