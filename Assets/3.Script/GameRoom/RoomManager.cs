using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class RoomManager : NetworkRoomManager
{
    public GameRuleData gameRuleData;
    public int minPlayerCount;
    public int imposterCount;

    public override void OnRoomServerConnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerConnect(conn);
    }

  /*  public override void OnRoomServerDisconnect(NetworkConnectionToClient conn)
    {
        base.OnRoomServerDisconnect(conn);

        if (conn.identity != null)
        {
            // ȣ��Ʈ�� ������ ������
            if (conn.identity.isServer)
            {
                // ���ο� ȣ��Ʈ�� ã��
                NetworkRoomPlayer newHost = FindNewHost();

                if (newHost != null)
                {
                    // ���ο� ȣ��Ʈ�� ����
                    NetworkServer.SetClientReady(newHost.connectionToClient);

                    // ����� ��� Ŭ���̾�Ʈ���� ���ο� ȣ��Ʈ ���� ����
                   // RpcChangeHost(newHost.connectionToClient);
                }
            }
        }
    }

    NetworkRoomPlayer FindNewHost()
    {
        List<NetworkRoomPlayer> players = new List<NetworkRoomPlayer>(roomSlots);
        players.RemoveAll(player => player == null);

        if (players.Count > 0)
        {
            int newHostIndex = Random.Range(0, players.Count);
            return players[newHostIndex];
        }

        return null;
    }
*/
 /*   [ClientRpc]
    void RpcChangeHost(NetworkConnection targetConnection)
    {
        // Ŭ���̾�Ʈ���� ȣ��Ʈ ���� �� ȣ��˴ϴ�.
        if (NetworkServer.active && targetConnection.connectionId == NetworkServer.localConnection.connectionId)
        {
            // �� ȣ��Ʈ�� ����� ������ Ŭ���̾�Ʈ�� ����
            // ���⼭ gameRuleData�� ���� ���� ���� �����͸� ������ �� �ֽ��ϴ�.
            // targetConnection.Send(new GameRuleDataMessage(gameRuleData));
        }
    }*/
}

