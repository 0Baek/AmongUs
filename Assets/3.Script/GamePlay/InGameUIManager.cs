using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

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
    [SerializeField]
    private EjectionUI ejectionUI;
    public EjectionUI EjectionUI
    {
        get
        {
            return ejectionUI;
        }
    }
    [SerializeField]
    private FIxWiringTask _FixWiringTaskUI;
    public FIxWiringTask FIxWiringTask
    {
        get
        {
            return _FixWiringTaskUI;
        }
    }

    [SerializeField]
    private VictoryUI victoryUI;
    public VictoryUI VictoryUi
    {
        get
        {
            return victoryUI;
        }
    }
  

    [SerializeField]
    private Button _UseButton;
    [SerializeField]
    private Sprite _OriginUseButtonSprite;


    private void Awake()
    {
        Instance = this;
    }
    public void SetUseButton(Sprite sprite,UnityAction action)
    {
        _UseButton.image.sprite = sprite;
        _UseButton.onClick.AddListener(action);
        _UseButton.interactable = true;
    }
    public void UnSetUseButton()
    {
        _UseButton.image.sprite = _OriginUseButtonSprite;
        _UseButton.onClick.RemoveAllListeners();
        _UseButton.interactable = false;
    }
}
