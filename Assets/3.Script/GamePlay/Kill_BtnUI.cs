using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Kill_BtnUI : MonoBehaviour
{
    [SerializeField]
    private Button killBtn;

    [SerializeField]
    private Text cooldownText;

    private InGameCharacterMover targetplayer;
   public void Show(InGameCharacterMover player)
    {
       
        gameObject.SetActive(true);
        targetplayer = player;
    }
    private void Update()
    {
        if (targetplayer !=null)
        {
            if (!targetplayer.isKillable)
            {
                cooldownText.text = targetplayer.KillCooldown > 0 ? ((int)targetplayer.KillCooldown).ToString() : "";
                killBtn.interactable = false;
            }
            else
            {
                cooldownText.text = "";
                killBtn.interactable = true;
            }
        }
    }
    public void OnClickKillBtn()
    {
       
        targetplayer.Kill();
    }



}
