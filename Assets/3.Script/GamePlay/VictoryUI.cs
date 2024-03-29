using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
  
/*
    [SerializeField]
    private GameObject crewmateObj;*/

    [SerializeField]
    private Text playerType;

    [SerializeField]
    private Image gradintImg;

    [SerializeField]
    private IntroCharacter myCharacter;

    [SerializeField]
    private List<IntroCharacter> otherCharacters = new List<IntroCharacter>();

    [SerializeField]
    private Color crewColor;

    [SerializeField]
    private Color imposterColor;






    private void Start()
    {
        ShowCrewVIctory();
        
    }

    public void ShowCrewVIctory()
    {
        var players = GameSystem.instance.GetPlayerList();

        InGameCharacterMover myPlayer = null;
        
        foreach (var player in players)
        {
            if (player.isOwned)
            {
                myPlayer = player;
                break;
            }
        }
        myCharacter.SetIntroCharacter(myPlayer.nickname, myPlayer.playercolor,myPlayer.playerType);

        if (myPlayer.playerType == EPlayerType.Imposter_Ghost)
        {
            playerType.text = "�й�";
            playerType.color = gradintImg.color = imposterColor;

            int i = 0;
            foreach (var player in players)
            {
                if (!player.isOwned && player.playerType == EPlayerType.Imposter_Ghost)
                {
                    otherCharacters[i].SetIntroCharacter(player.nickname, player.playercolor,player.playerType);
                    otherCharacters[i].gameObject.SetActive(true);
                    i++;
                }
            }
        }
        else
        {
            playerType.text = "�¸�";
            playerType.color = gradintImg.color = crewColor;

            int i = 0;
            foreach (var player in players)
            {
                if (!player.isOwned)
                {
                    otherCharacters[i].SetIntroCharacter(player.nickname, player.playercolor,player.playerType);
                    otherCharacters[i].gameObject.SetActive(true);
                    i++;
                }
            }
        }
        AudioManager.instance.PlaySFX("CrewVictory");
        myPlayer.IsMoveable = false;
    }



}
