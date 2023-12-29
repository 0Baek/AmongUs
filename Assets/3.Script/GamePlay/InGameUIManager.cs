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
    [SerializeField]
    private ReportBtnUI reportBtnUI;
    public ReportBtnUI ReportBtnUI
    {
        get
        {
            return reportBtnUI;
        }
    }
    [SerializeField]
    private ReportUI reportUI;
    public ReportUI ReportUI
    {
        get
        {
            return reportUI;
        }
    }
    [SerializeField]
    private MeetingUI meetingUI;
    public MeetingUI MeetingUI
    {
        get
        {
            return meetingUI;
        }
    }


    private void Awake()
    {
        Instance = this;
    }
}
