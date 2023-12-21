using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineUI : MonoBehaviour
{
    [SerializeField]
    private InputField nicknameInputField;
    [SerializeField]
    private GameObject creatRoomUI;

    public void onClickCreateRoomButton()
    {
        if (nicknameInputField.text !="")
        {
            PlayerSettings.ninkname = nicknameInputField.text;
            creatRoomUI.SetActive(true);
            gameObject.SetActive(false);
        }
        else
        {
            nicknameInputField.GetComponent<Animator>().SetTrigger("on");
        }
    }
    public void OnClickEnterGameRoomBtn()
    {
        if (nicknameInputField.text!="")
        {
            PlayerSettings.ninkname = nicknameInputField.text;
            var manager = RoomManager.singleton;
            manager.StartClient();

        }
        else
        {
            nicknameInputField.GetComponent<Animator>().SetTrigger("on");
        }
      
    }

}
