using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroCharacter : MonoBehaviour
{
    [SerializeField]
    private Image character;

    [SerializeField]
    private Text nickname;

    public void SetIntroCharacter(string nick,EPlayerColor playerColor,EPlayerType type)
    {

        var matInst = Instantiate(character.material);
        character.material = matInst;
        nickname.text = nick;
        if (type==EPlayerType.Imposter_Ghost)
        {
            nickname.color = Color.red;
        }
        else
        {
            nickname.color = Color.white;
        }
       
        character.material.SetColor("_PlayerColor", PlayerColor.GetColor(playerColor));
    }

}
