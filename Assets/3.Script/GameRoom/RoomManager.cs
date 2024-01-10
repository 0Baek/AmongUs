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
 
}
