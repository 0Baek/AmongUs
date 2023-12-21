using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
   public void OnClickOnineBtn()
    {
        Debug.Log("Click Online");
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
