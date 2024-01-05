using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    private void Start()
    {
        AudioManager.instance.PlayBGM("MainBGM");
    }
    public void OnClickOnineBtn()
    {
        AudioManager.instance.PlaySFX("Next");
        gameObject.SetActive(false);
        
    }
    public void OnClickQuitBtn()
    {
#if  UNITY_EDITOR 
        UnityEditor.EditorApplication.isPlaying = false;
#else
       Application.Quit();
#endif 
    }
    
}
