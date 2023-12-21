using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRoomItem : MonoBehaviour
{
   public GameObject inactiveObject;

    private void Start()
    {
        if (!AmongUsRoomplayer.MyRoomPlayer.isServer)
        {
            inactiveObject.SetActive(false);
        }
    }
}
