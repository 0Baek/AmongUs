using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Deadbody : NetworkBehaviour
{
    private SpriteRenderer spriteRenderer;
    private EPlayerColor deadbodyColor;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    [ClientRpc]
    public void RpcSetColor(EPlayerColor color)
    {
        deadbodyColor = color;
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(color));
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var player = collision.GetComponent<InGameCharacterMover>();
        //감지된 플레이어가 들어오거나 나갈 때마다,자기 자신이고 유령이 아닌 상태라면 리포트 버튼 활성화 
        if (player != null && player.isOwned && (player.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
        {
            InGameUIManager.Instance.ReportBtnUI.SetInteractable(true);
            var myCharacter = AmongUsRoomplayer.MyRoomPlayer.myCharacter as InGameCharacterMover;
            myCharacter.foundDeadbodyColor = deadbodyColor;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var player = collision.GetComponent<InGameCharacterMover>();
        //감지된 플레이어가 들어오거나 나갈 때마다,자기 자신이고 유령이 아닌 상태라면 리포트 버튼 활성화 
        if (player != null && player.isOwned && (player.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
        {
            InGameUIManager.Instance.ReportBtnUI.SetInteractable(false);
        }
    }
}
