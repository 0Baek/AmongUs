using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AmongUsRoomplayer : NetworkRoomPlayer
{
    private static AmongUsRoomplayer myRoomPlayer;
    public static AmongUsRoomplayer MyRoomPlayer
    {
        get
        {
            if (myRoomPlayer==null)
            {
                var players = FindObjectsOfType<AmongUsRoomplayer>();
                foreach (var player in players)
                {
                    if (player.isOwned)
                    {
                        myRoomPlayer = player;
                    }
                }
            }
            return myRoomPlayer;
        }
    }
    //동기화
    [SyncVar(hook =nameof(SetPlayerColor_Hook))]
    public EPlayerColor playerColor;
    public void SetPlayerColor_Hook(EPlayerColor oldColor,EPlayerColor newColor)
    {
        LobbyUIManager.instance.CustumizeUI.UpdateUnSelectColorBtn(oldColor);
        LobbyUIManager.instance.CustumizeUI.UpdateSelectColorBtn(newColor);
      
    }





    [SyncVar]
    public string nickname;
    public CharacterMover myCharacter;


    [Command]
    public void CmdSetNickname(string nick)
    {
        // 중복 체크
        if (!IsNicknameDuplicate(nick))
        {
            nickname = nick;
            myCharacter.nickname = nick;
        }
        // 중복일 경우 처리 (예: 에러 메시지 전송 또는 다른 조치)
        else
        {
            //중복
           
            connectionToClient.Disconnect();
            
        }
    }
    private bool IsNicknameDuplicate(string newNickname)
    {
        var players = FindObjectsOfType<AmongUsRoomplayer>();
        foreach (var player in players)
        {
            // 자신의 경우 무시
            if (player == this)
                continue;

            // 중복된 닉네임이 있는지 확인
            if (player.nickname == newNickname)
            {
                return true;
            }
        }
        return false;
    }





        [SyncVar(hook = nameof(OnConnectionChanged))]
        public bool connection;

        [Command]
        public void CmdSetConnection(bool isConnected)
        {
            connection = isConnected;
        }

        void OnConnectionChanged(bool oldValue, bool newValue)
        {
            connection = newValue;
        }



    public void Start()
    {
        base.Start();
      
        if (isServer)
        {
            SpawnLobbyPlayerCharacter();
            LobbyUIManager.instance.ActiveStartButton();
        }
        if (isLocalPlayer)
        {
            CmdSetNickname(PlayerSettings.nickname);
        }
        LobbyUIManager.instance.GameRoomPlayerCount.UpdatePlayerCount();
    }
    private void OnDestroy()
    {
        if (LobbyUIManager.instance!=null)
        {
            LobbyUIManager.instance.GameRoomPlayerCount.UpdatePlayerCount();
            LobbyUIManager.instance.CustumizeUI.UpdateUnSelectColorBtn(playerColor);
        }
    }
 /*   [Command]
    public void CmdSetNickname(string nick)
    {
        nickname = nick;
        lobbyPlayerCharacter.nickname = nick;
    }*/


    //클라이언트에서 함수를 호출하면 함수 동작이 서버에서 실행되게하는 어트리뷰트
    [Command]
    public void CmdSetPlayerColor(EPlayerColor color)
    {
        playerColor = color;
        myCharacter.playercolor = color;
    }

    private void SpawnLobbyPlayerCharacter()
    {
        var  roomSlots = (NetworkRoomManager.singleton as RoomManager).roomSlots;
        EPlayerColor color = EPlayerColor.Red;
        for (int i = 0; i < (int)EPlayerColor.Lime+1; i++)
        {
            bool isFindSameColor = false;
            foreach (var roomPlayer in roomSlots)
            {
                var amongUsRoomPlayer = roomPlayer as AmongUsRoomplayer;
                if (amongUsRoomPlayer.playerColor == (EPlayerColor)i && roomPlayer.netId != netId)
                {
                    isFindSameColor = true;
                    break;
                }
            }
            if (!isFindSameColor)
            {
                color = (EPlayerColor)i;
                break;
            }
        }
        playerColor = color;

        var spawnPosition = FindObjectOfType<SpawnPositions>();
        int index = spawnPosition.Index;
        Vector3 spawnPos = FindObjectOfType<SpawnPositions>().GetSpawnPosition();
       
        var playerCharacter = Instantiate(RoomManager.singleton.spawnPrefabs[0], spawnPos, Quaternion.identity).GetComponent<LobbyCharacterMover>();
        if (index < 5)
        {
            playerCharacter.transform.localScale = new Vector3(0.5f, 0.5f, 1f);
        }
        else
        {
            playerCharacter.transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
        }
        //클라이언트들에게 이 게임 오브젝트가 소환되었음을 알림 
        //conn 접속한 플레이어의 정보 
        NetworkServer.Spawn(playerCharacter.gameObject, connectionToClient);
        playerCharacter.ownerNetld = netId;
        playerCharacter.playercolor = color;

        /*
         
            Vector3 spawnPos = FindObjectOfType<SpawnPositions>().GetSpawnPosition();

        var player = Instantiate(spawnPrefabs[0],spawnPos,Quaternion.identity);

        //클라이언트들에게 이 게임 오브젝트가 소환되었음을 알림 
        //conn 접속한 플레이어의 정보 
        NetworkServer.Spawn(player, conn);
         
         */

    }
  
}
