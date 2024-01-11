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
            // 호스트가 연결을 끊으면
            if (conn.identity.isServer)
            {
                // 새로운 호스트를 찾기
                NetworkRoomPlayer newHost = FindNewHost();

                if (newHost != null)
                {
                    // 새로운 호스트로 지정
                    NetworkServer.SetClientReady(newHost.connectionToClient);

                    // 연결된 모든 클라이언트에게 새로운 호스트 정보 전송
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
        // 클라이언트에서 호스트 변경 시 호출됩니다.
        if (NetworkServer.active && targetConnection.connectionId == NetworkServer.localConnection.connectionId)
        {
            // 새 호스트로 변경된 정보를 클라이언트에 전달
            // 여기서 gameRuleData와 같은 게임 상태 데이터를 전송할 수 있습니다.
            // targetConnection.Send(new GameRuleDataMessage(gameRuleData));
        }
    }*/
}

