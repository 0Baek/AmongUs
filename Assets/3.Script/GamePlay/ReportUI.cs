using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReportUI : MonoBehaviour
{
    [SerializeField]
    private Image deadbodyImg;

    [SerializeField]
    private Material material;

    public void Open(EPlayerColor deadbodyColor)
    {
        AmongUsRoomplayer.MyRoomPlayer.myCharacter.IsMoveable = false;

        Material inst = Instantiate(material);
        deadbodyImg.material = inst;

        gameObject.SetActive(true);
        AudioManager.instance.PlaySFX("Report");
        deadbodyImg.material.SetColor("_PlayerColor", PlayerColor.GetColor(deadbodyColor));
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
