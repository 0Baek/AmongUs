using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoomSettingUI : SettingUI
{
    public void Open()
    {
        
        AmongUsRoomplayer.MyRoomPlayer.myCharacter.IsMoveable = false;
        gameObject.SetActive(true);
    }
    public override void Close()
    {
        base.Close();
        AmongUsRoomplayer.MyRoomPlayer.myCharacter.IsMoveable = true;

    }
    public void ExitGameRoom()
    {
        var manager = RoomManager.singleton;
        if (manager.mode == Mirror.NetworkManagerMode.Host)
        {
            manager.StopHost();

        }
        else
        {
            manager.StopClient();
        }
    }
}
