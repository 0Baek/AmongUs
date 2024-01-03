using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public enum EMeetimgState
{
    None,
    Meeting,
    Vote
}

public class MeetingUI : MonoBehaviour
{
    [SerializeField]
    private GameObject playerPanelPrefab;

    [SerializeField]
    private Transform playerPanelsParent;

    [SerializeField]
    private GameObject voterPrefab;

    [SerializeField]
    private GameObject skipvoteBtn;

    [SerializeField]
    private GameObject skipVoteplayers;

    [SerializeField]
    private Transform skipvoteParentTransform;

    [SerializeField]
    private GameObject ChatBtn;
    [SerializeField]
    public GameObject chatPanel; // 추가된 부분
    [SerializeField]
    public Text chatText; // 추가된 부분
    [SerializeField]
    private InputField chatInputField; // 추가된 부분

    [SerializeField]
    private Text meetingTimeText;

    private EMeetimgState meetimgState;

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
    public void ChangeMeetingState(EMeetimgState state)
    {
        meetimgState = state;
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
    public void UpdateSkipVotePlayer(EPlayerColor skipVoterPlayerColor)
    {
        foreach(var panel in meetingPlayerPanels)
        {
            if (panel.targetPlayer.playercolor ==skipVoterPlayerColor)
            {
                panel.UpdateVoteSign(true);
            }
        }
        var voter = Instantiate(voterPrefab, skipvoteParentTransform).GetComponent<Image>();
        voter.material = Instantiate(voter.material);
        voter.material.SetColor("_PlayerColor", PlayerColor.GetColor(skipVoterPlayerColor));
        skipvoteBtn.SetActive(false);

        
    }
    public void OnClickSkipVoteBtn()
    {
        var myCharacter = AmongUsRoomplayer.MyRoomPlayer.myCharacter as InGameCharacterMover;
        if (myCharacter.isVote)
        {
            return;
        }

        myCharacter.CmdSkipVote();
        SelectPlayerPanel();
    }
    public void OnClickChatBtn()
    {
        ChatBtn.SetActive(true);
    }
    public void CompleteVote()
    {
        foreach(var panel in meetingPlayerPanels)
        {
            panel.OpenResult();
        }
        skipVoteplayers.SetActive(true);
        skipvoteBtn.SetActive(false);
    }
    public void Close()
    {
        foreach (var panel in meetingPlayerPanels)
        {
            Destroy(panel.gameObject);
        }

        meetingPlayerPanels.Clear();

        gameObject.SetActive(false);
    }
    private void Update()
    {
        if (meetimgState ==EMeetimgState.Meeting)
        {
            meetingTimeText.text = string.Format("회의시간 : {0}s", (int)Mathf.Clamp(GameSystem.instance.remainTime, 0f, float.MaxValue));
        }
        else if (meetimgState == EMeetimgState.Vote)
        {
            meetingTimeText.text = string.Format("투표시간 : {0}s", (int)Mathf.Clamp(GameSystem.instance.remainTime, 0f, float.MaxValue));
        }
    }



}
