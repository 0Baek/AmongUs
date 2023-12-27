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
    //����ȭ
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
        // �ߺ� üũ
        if (!IsNicknameDuplicate(nick))
        {
            nickname = nick;
            myCharacter.nickname = nick;
        }
        // �ߺ��� ��� ó�� (��: ���� �޽��� ���� �Ǵ� �ٸ� ��ġ)
        else
        {
            //�ߺ�
           
            connectionToClient.Disconnect();
            
        }
    }
    private bool IsNicknameDuplicate(string newNickname)
    {
        var players = FindObjectsOfType<AmongUsRoomplayer>();
        foreach (var player in players)
        {
            // �ڽ��� ��� ����
            if (player == this)
                continue;

            // �ߺ��� �г����� �ִ��� Ȯ��
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


    //Ŭ���̾�Ʈ���� �Լ��� ȣ���ϸ� �Լ� ������ �������� ����ǰ��ϴ� ��Ʈ����Ʈ
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
        //Ŭ���̾�Ʈ�鿡�� �� ���� ������Ʈ�� ��ȯ�Ǿ����� �˸� 
        //conn ������ �÷��̾��� ���� 
        NetworkServer.Spawn(playerCharacter.gameObject, connectionToClient);
        playerCharacter.ownerNetld = netId;
        playerCharacter.playercolor = color;

        /*
         
            Vector3 spawnPos = FindObjectOfType<SpawnPositions>().GetSpawnPosition();

        var player = Instantiate(spawnPrefabs[0],spawnPos,Quaternion.identity);

        //Ŭ���̾�Ʈ�鿡�� �� ���� ������Ʈ�� ��ȯ�Ǿ����� �˸� 
        //conn ������ �÷��̾��� ���� 
        NetworkServer.Spawn(player, conn);
         
         */

    }
  
}
