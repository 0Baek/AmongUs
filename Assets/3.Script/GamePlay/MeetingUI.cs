using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeetingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPanelPrefab;

    [SerializeField]
    private Transform playerPanelsParent;

    private List<MeetingPlayerPanel> meetingPlayerPanels = new List<MeetingPlayerPanel>();

    public void Open()
    {
        var myCharacter = AmongUsRoomplayer.MyRoomPlayer.myCharacter as InGameCharacterMover;
        var myPanel = Instantiate(playerPanelPrefab, playerPanelsParent).GetComponent<MeetingPlayerPanel>();
        myPanel.SetPlayer(myCharacter);
        meetingPlayerPanels.Add(myPanel);

        gameObject.SetActive(true);

        var players = FindObjectsOfType<InGameCharacterMover>();
        foreach (var player in players)
        {
            if (player != myCharacter)
            {
                var panel = Instantiate(playerPanelPrefab, playerPanelsParent).GetComponent<MeetingPlayerPanel>();
                panel.SetPlayer(player);
                meetingPlayerPanels.Add(panel);
            }
        }
    }
    public void SelectPlayerPanel()
    {
        foreach (var panel in meetingPlayerPanels)
        {
            panel.Unselect();
            
        }
    }
    public void UpdateVote(EPlayerColor voterColor,EPlayerColor ejectColor)
    {
        foreach (var panel in meetingPlayerPanels)
        {
            if (panel.targetPlayer.playercolor==ejectColor)
            {
                panel.UpdatePanel(voterColor);
            }
            if (panel.targetPlayer.playercolor==voterColor)
            {
                panel.UpdateVoteSign(true);
            }
        }
    }
}
