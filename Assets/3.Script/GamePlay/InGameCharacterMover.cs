using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
public enum EPlayerType
{
    Crew,
    Imposter
}

public class InGameCharacterMover : CharacterMover
{
    [SyncVar]
    public EPlayerType playerType;

    [ClientRpc]
    public void RpcTeleport(Vector3 position)
    {
        transform.position = position;
    }
    public void SetNicknameColor(EPlayerType type)
    {
        if (playerType ==EPlayerType.Imposter && type == EPlayerType.Imposter)
        {
            nicknameText.color = Color.red;
        }
    }
 
    public override void Start()  //CharacterMover 클래스의 Start 함수를 덮어쓰도록 하기 위함
    {
        base.Start();
        if (isOwned)
        {
            IsMoveable = true;

            var myRoomPlayer = AmongUsRoomplayer.MyRoomPlayer;
            myRoomPlayer.myCharacter = this;
            CmdSetPlayerCharacter(myRoomPlayer.nickname, myRoomPlayer.playerColor);
        }
        GameSystem.instance.AddPlayer(this);
    }
    [Command]
    private void CmdSetPlayerCharacter(string nickname ,EPlayerColor color)
    {
        this.nickname = nickname;
        playercolor = color;
    }
  
}
