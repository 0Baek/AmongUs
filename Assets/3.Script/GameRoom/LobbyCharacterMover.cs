using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LobbyCharacterMover : CharacterMover
{
    [SyncVar(hook =nameof(SetOwnerNetId_Hook))]
    public uint ownerNetld; // ¾ÆÀÌµð 

    public void SetOwnerNetId_Hook(uint _, uint newOwnerId)
    {
        var players = FindObjectsOfType<AmongUsRoomplayer>();

        foreach (var player in players)
        {
            if (newOwnerId == player.netId)
            {
                player.myCharacter = this;
                break;
            }
        }

    }
    public void CompleteSpawn()
    {
        
        if (isOwned)
        {
            IsMoveable = true;
        }
    }

}
