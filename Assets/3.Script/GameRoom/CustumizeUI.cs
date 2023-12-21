using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CustumizeUI : MonoBehaviour
{
    [SerializeField] private Button colorBtn;
    [SerializeField] private GameObject colorPanel;
    [SerializeField] private Button gameRuleBtn;
    [SerializeField] private GameObject gameRulePanel;


    [SerializeField] private Image characterPreview;

    [SerializeField] private List<ColorSelect> colorSelects;


 
    void Start()
    {

        var inst = Instantiate(characterPreview.material);
        characterPreview.material = inst;
    }
    public void ActiveColorPanel()
    {
        colorBtn.image.color = new Color(0f, 0f, 0f, 0.75f);
        gameRuleBtn.image.color= new Color(0f, 0f, 0f, 0.25f);

        colorPanel.SetActive(true);
        gameRulePanel.SetActive(false);
    }
    public void ActiveGameRulePanel()
    {
        colorBtn.image.color = new Color(0f, 0f, 0f, 0.25f);
        gameRuleBtn.image.color = new Color(0f, 0f, 0f, 0.75f);

        colorPanel.SetActive(false);
        gameRulePanel.SetActive(true);
    }
    private void OnEnable()
    {

        UpdateColorBtn();
        ActiveColorPanel();

        var roomSlots = (NetworkManager.singleton as RoomManager).roomSlots;

        foreach (var player in roomSlots)
        {
            var aplayer = player as AmongUsRoomplayer;
            if (aplayer.isLocalPlayer)
            {
                UpdatePreviewColor(aplayer.playerColor);
                break;
            }
        }
    }


    public void UpdateColorBtn()
    {
        var roomSlots = (NetworkManager.singleton as RoomManager).roomSlots;
        for (int i = 0; i < colorSelects.Count; i++)
        {
            colorSelects[i].SetInteractable(true);
        }
        foreach (var player in roomSlots)
        {
            var aplayer = player as AmongUsRoomplayer;
            colorSelects[(int)aplayer.playerColor].SetInteractable(false);
        }
    }
    public void UpdateSelectColorBtn(EPlayerColor color)
    {
        colorSelects[(int)color].SetInteractable(false);
    }
    public void UpdateUnSelectColorBtn(EPlayerColor color)
    {
        colorSelects[(int)color].SetInteractable(true);
    }
    public void UpdatePreviewColor(EPlayerColor color)
    {
        characterPreview.material.SetColor("_PlayerColor", PlayerColor.GetColor(color));
    }
    public void OnClickColorBtn(int index)
    {
        if (colorSelects[index].isInteractable)
        {
            AmongUsRoomplayer.MyRoomPlayer.CmdSetPlayerColor ((EPlayerColor)index);
            UpdatePreviewColor((EPlayerColor)index);
        }
    }
    public void Open()
    {
      // lobbyCharacter = FindObjectOfType<LobbyCharacterMover>();
        AmongUsRoomplayer.MyRoomPlayer.lobbyPlayerCharacter.IsMoveable = false;
       // lobbyCharacter.isMoveable = false;
        gameObject.SetActive(true);
    }
    public void Close()
    {
        //lobbyCharacter.isMoveable = true;
        AmongUsRoomplayer.MyRoomPlayer.lobbyPlayerCharacter.IsMoveable = true;
        gameObject.SetActive(false);
    }
}
