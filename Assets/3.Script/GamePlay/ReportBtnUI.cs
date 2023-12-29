using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportBtnUI : MonoBehaviour
{
    [SerializeField]
    private Button reportBtn;

    public void SetInteractable(bool isInteractable)
    {
        reportBtn.interactable = isInteractable;
    }
    public void OnClickBtn()
    {
        var character = AmongUsRoomplayer.MyRoomPlayer.myCharacter as InGameCharacterMover;
        character.Report();
    }
}
