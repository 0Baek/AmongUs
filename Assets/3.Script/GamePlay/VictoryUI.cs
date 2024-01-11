using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VictoryUI : MonoBehaviour
{
  

  /*  [SerializeField]
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



  /*  public IEnumerator ShowIntroSequence()
    {

     
        AudioManager.instance.PlaySFX("Start");
        yield return new WaitForSeconds(3f);
      

        ShowPlayerType();
        crewmateObj.SetActive(true);

    }*/

    public void ShowPlayerType()
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
        myCharacter.SetIntroCharacter(myPlayer.nickname, myPlayer.playercolor);

        if (myPlayer.playerType == EPlayerType.Imposter)
        {
            playerType.text = "ÆÐ¹è";
            playerType.color = gradintImg.color = imposterColor;

            int i = 0;
            foreach (var player in players)
            {
                if (!player.isOwned && player.playerType == EPlayerType.Imposter)
                {
                    otherCharacters[i].SetIntroCharacter(player.nickname, player.playercolor);
                    otherCharacters[i].gameObject.SetActive(true);
                    i++;
                }
            }
        }
        else
        {
            playerType.text = "½Â¸®";
            playerType.color = gradintImg.color = crewColor;

            int i = 0;
            foreach (var player in players)
            {
                if (!player.isOwned)
                {
                    otherCharacters[i].SetIntroCharacter(player.nickname, player.playercolor);
                    otherCharacters[i].gameObject.SetActive(true);
                    i++;
                }
            }
        }
    }
 
  
}
