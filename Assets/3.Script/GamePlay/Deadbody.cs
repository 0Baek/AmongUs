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
        //������ �÷��̾ �����ų� ���� ������,�ڱ� �ڽ��̰� ������ �ƴ� ���¶�� ����Ʈ ��ư Ȱ��ȭ 
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
        //������ �÷��̾ �����ų� ���� ������,�ڱ� �ڽ��̰� ������ �ƴ� ���¶�� ����Ʈ ��ư Ȱ��ȭ 
        if (player != null && player.isOwned && (player.playerType & EPlayerType.Ghost) != EPlayerType.Ghost)
        {
            InGameUIManager.Instance.ReportBtnUI.SetInteractable(false);
        }
    }
}
