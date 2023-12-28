using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance;

    [SerializeField]
    private InGameIntroUI ingameintroUI;
    public InGameIntroUI IngameintroUI
    {
        get
        {
            return ingameintroUI;
        }
    }

    [SerializeField]
    private Kill_BtnUI kill_BtnUI;
    
    public Kill_BtnUI Kill_BtnUI
    {
        get
        {
            return kill_BtnUI;
        }
    }
    [SerializeField]
    private KillUI killUI;
    public KillUI KillUI
    {
        get
        {
            return killUI;
        }
    }


    private void Awake()
    {
        Instance = this;
    }
}
