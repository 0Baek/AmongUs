using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EjectionUI : MonoBehaviour
{
    [SerializeField]
    private Text ejectionResultText;

    [SerializeField]
    private Image ejectionPlayer;

    [SerializeField]
    private RectTransform left;

    [SerializeField]
    private RectTransform right;

    [SerializeField]
    private GameObject Victory;

    private void Start()
    {
        ejectionPlayer.material = Instantiate(ejectionPlayer.material);
    }
    public void Open(bool isEjection, EPlayerColor ejectionPlayerColor,bool isImposter,int remainImposterCount)
    {
        string text = "";
        InGameCharacterMover ejectPlayer = null;
        if (isEjection)
        {
            InGameCharacterMover[] players = FindObjectsOfType<InGameCharacterMover>();
            foreach (var player in players)
            {
                if (player.playercolor==ejectionPlayerColor)
                {
                    ejectPlayer = player;
                    break;
                }
            }
            text = string.Format("{0}은 임포스터{1}\n임포스터가 {2}명 남았습니다.",
                ejectPlayer.nickname, isImposter ? "입니다." : "가 아니었습니다.", remainImposterCount);
        }
        else
        {
            text = string.Format("아무도 퇴출되지 않았습니다.\n임포스터가 {0}명 남았습니다.", remainImposterCount);
        }
     
        StartCoroutine(SHowEjectionResult_Coroutine(ejectPlayer, text));

        if (remainImposterCount==0)
        {

            gameObject.SetActive(true);
            StartCoroutine(SHowEjectionResult_Coroutine2(ejectPlayer, text));
        }
      
    }
    private IEnumerator SHowEjectionResult_Coroutine(InGameCharacterMover ejectionPlayerMover,string text)
    {
        ejectionResultText.text = "";

        string forwardText = "";
        string backText = "";

        if (ejectionPlayerMover != null)
        {
            ejectionPlayer.material.SetColor("_PlayerColor", PlayerColor.GetColor(ejectionPlayerMover.playercolor));

            float timer = 0f;
            while (timer<=1f)
            {
                yield return null;
                timer += Time.deltaTime + 0.5f;

                AudioManager.instance.PlaySFX("Eject");

                ejectionPlayer.rectTransform.anchoredPosition = Vector2.Lerp(left.anchoredPosition, right.anchoredPosition, timer);
                ejectionPlayer.rectTransform.rotation = Quaternion.Euler(ejectionPlayer.rectTransform.rotation.eulerAngles +
                    new Vector3(0f, 0f, -360f * Time.deltaTime));
            }
        }
        backText = text;
        while (backText.Length !=0)
        {
            forwardText += backText[0];
            backText = backText.Remove(0, 1);
            ejectionResultText.text = string.Format("<color=#FFFFFF>{0}</color><color=#000000>{1}</color>", forwardText, backText);

            AudioManager.instance.PlaySFX("Eject");

            yield return new WaitForSeconds(0.05f);
        }
       
    }
    private IEnumerator SHowEjectionResult_Coroutine2(InGameCharacterMover ejectionPlayerMover, string text)
    {
        ejectionResultText.text = "";

        string forwardText = "";
        string backText = "";

        if (ejectionPlayerMover != null)
        {
            ejectionPlayer.material.SetColor("_PlayerColor", PlayerColor.GetColor(ejectionPlayerMover.playercolor));

            float timer = 0f;
            while (timer <= 1f)
            {
                yield return null;
                timer += Time.deltaTime + 0.5f;

                AudioManager.instance.PlaySFX("Eject");

                ejectionPlayer.rectTransform.anchoredPosition = Vector2.Lerp(left.anchoredPosition, right.anchoredPosition, timer);
                ejectionPlayer.rectTransform.rotation = Quaternion.Euler(ejectionPlayer.rectTransform.rotation.eulerAngles +
                    new Vector3(0f, 0f, -360f * Time.deltaTime));
            }
        }
        backText = text;
        while (backText.Length != 0)
        {
            forwardText += backText[0];
            backText = backText.Remove(0, 1);
            ejectionResultText.text = string.Format("<color=#FFFFFF>{0}</color><color=#000000>{1}</color>", forwardText, backText);

            AudioManager.instance.PlaySFX("Eject");

            yield return new WaitForSeconds(0.05f);

           
        }
        yield return new WaitForSeconds(2f);
        Victory.SetActive(true);
        InGameUIManager.Instance.VictoryUi.ShowPlayerType();
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }
}
