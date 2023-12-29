using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeetingPlayerPanel : MonoBehaviour
{
    [SerializeField]
    private Image characterImg;

    [SerializeField]
    private Text ninknameText;

    [SerializeField]
    private GameObject deadPlayerBlock;

    [SerializeField]
    private GameObject reportSign;

    [SerializeField]
    private GameObject voteButtons;

    public InGameCharacterMover targetPlayer;

    [SerializeField]
    private GameObject voteSign;

    [SerializeField]
    private GameObject voterPrefab;

    [SerializeField]
    private Transform voterParentTransform;

    public void UpdatePanel(EPlayerColor voterColor)
    {
        var voter = Instantiate(voterPrefab, voterParentTransform).GetComponent<Image>();
        voter.material = Instantiate(voter.material);
        voter.material.SetColor("_PlayerColor", PlayerColor.GetColor(voterColor));
        voterParentTransform.gameObject.SetActive(true);
    }
    public void UpdateVoteSign(bool isVoted)
    {
        voteSign.SetActive(isVoted);
    }

    public void SetPlayer(InGameCharacterMover target)
    {
        Material inst = Instantiate(characterImg.material);
        characterImg.material = inst;

        targetPlayer = target;
        characterImg.material.SetColor("_PlayerColor", PlayerColor.GetColor(targetPlayer.playercolor));
        ninknameText.text = target.nickname;

        var myCharacter = AmongUsRoomplayer.MyRoomPlayer.myCharacter as InGameCharacterMover;
        if (((myCharacter.playerType & EPlayerType.Imposter)==EPlayerType.Imposter)
            &&(targetPlayer.playerType&EPlayerType.Imposter)==EPlayerType.Imposter)
        {
            ninknameText.color = Color.red;
        }

        bool isDead = (targetPlayer.playerType & EPlayerType.Ghost) == EPlayerType.Ghost;
        deadPlayerBlock.SetActive(isDead);
        GetComponent<Button>().interactable = !isDead;
        reportSign.SetActive(targetPlayer.isReporter);
    }
    public void OnClickPlayerPanel()
    {
        var myCharacter = AmongUsRoomplayer.MyRoomPlayer.myCharacter as InGameCharacterMover;
        if (myCharacter.isVote)
        {
            return;
        }
        if ((myCharacter.playerType& EPlayerType.Ghost)!=EPlayerType.Ghost)
        {
            InGameUIManager.Instance.MeetingUI.SelectPlayerPanel();
            voteButtons.SetActive(true);
        }
     
    }
    public void Select()
    {

        var myCharacter = AmongUsRoomplayer.MyRoomPlayer.myCharacter as InGameCharacterMover;
        myCharacter.CmdVoteEjectPlayer(targetPlayer.playercolor);
        Unselect();
    }
    public void Unselect()
    {
        voteButtons.SetActive(false);
    }
}
