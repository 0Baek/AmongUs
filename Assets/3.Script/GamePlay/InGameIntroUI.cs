using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InGameIntroUI : MonoBehaviour
{
    [SerializeField]
    private GameObject shhhhhhObj;

    [SerializeField]
    private GameObject crewmateObj;

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

    [SerializeField]
    private CanvasGroup canvasGroup;

    public IEnumerator ShowIntroSequence()
    {
       
        shhhhhhObj.SetActive(true);
        AudioManager.instance.PlaySFX("Start");
        yield return new WaitForSeconds(3f);
        shhhhhhObj.SetActive(false);

        ShowPlayerType();
        crewmateObj.SetActive(true);

    }

    public void ShowPlayerType()
    {
        var players = GameSystem.instance.GetPlayerList();

        InGameCharacterMover myPlayer = null;
        foreach(var player in players)
        {
            if (player.isOwned)
            {
                myPlayer = player;
                break;
            }
        }
        myCharacter.SetIntroCharacter(myPlayer.nickname,myPlayer.playercolor,myPlayer.playerType);

        if (myPlayer.playerType == EPlayerType.Imposter)
        {
            playerType.text = "��������";
            playerType.color = gradintImg.color = imposterColor;

            int i = 0;
            foreach (var player in players)
            {
                if (!player.isOwned&&player.playerType==EPlayerType.Imposter)
                {
                    otherCharacters[i].SetIntroCharacter(player.nickname, player.playercolor,myPlayer.playerType);
                    otherCharacters[i].gameObject.SetActive(true);
                    i++;
                }
            }
        }
        else
        {
            playerType.text = "ũ���";
            playerType.color = gradintImg.color = crewColor;

            int i = 0;
            foreach (var player in players)
            {
                if (!player.isOwned )
                {
                    otherCharacters[i].SetIntroCharacter(player.nickname, player.playercolor,myPlayer.playerType);
                    otherCharacters[i].gameObject.SetActive(true);
                    i++;
                }
            }
        }
    }
    public void Close()
    {
        StartCoroutine(FadeOut());
    }
    private IEnumerator FadeOut()
    {
        float timer = 0f;
        while (timer<=1f)
        {
            yield return null;
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, timer);
        }
        gameObject.SetActive(false);
    }
}
