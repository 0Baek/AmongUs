using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class GameRoomPlayerCount : NetworkBehaviour
{
    [SyncVar]
    private int minPlayer;
    [SyncVar]
    private int maxPlayer;

    [SerializeField]
    private Text playerCountText;
    public void UpdatePlayerCount()
    {
      
        var players = FindObjectsOfType<AmongUsRoomplayer>();
        bool isStartable = players.Length >= minPlayer;
        playerCountText.color = isStartable ? Color.white : Color.red;
        playerCountText.text = string.Format("{0}/{1}", players.Length,maxPlayer);
       
        LobbyUIManager.instance.SetInteractableStartButton(isStartable);
    }
    private void Start()
    {
        if (isServer)
        {
            var manager = NetworkManager.singleton as RoomManager;
            minPlayer = manager.minPlayerCount;
            maxPlayer = manager.maxConnections;
        }
    }
}
