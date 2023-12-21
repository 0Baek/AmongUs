using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public enum EKillRange
{
    Short,Normal,Long
}
public enum ETaskBarUpdates
{
    Always,Meetings,Never
}
public struct GameRuleData
{
    public bool confirmEjects;
    public int emergencyMeetings;
    public int emergencyMeetingsCooldown;
    public int meetingsTime;
    public bool anonymousVotes;
    public float moveSpeed;
    public float crewSight;
    public float imposterSight;
    public float KillCooldown;
    public EKillRange killRange;
    public bool visualTaske;
    public ETaskBarUpdates taskBarUpdates;
    public int commonTask;
    public int complexTask;
    public int simpleTask;
}

public class GameRuleStore : NetworkBehaviour
{
    [SyncVar(hook=nameof(SetIsRule_Hook))]
    private bool isRule;
    [SerializeField]
    private Toggle isRuleToggle;
    public void SetIsRule_Hook(bool _,bool value)
    {
        UpdateGameRuleOverview();
    }
    public void OnRuleToggle(bool value)
    {
        isRule = value;
        if (isRule)
        {
            SetRecommendGameRule();
        }
    }

    [SyncVar(hook =nameof(SetConfirmEjects_Hook))]
    private bool confirmEjects;
    [SerializeField]
    private Toggle confirmEjectsToggle;
    public void SetConfirmEjects_Hook(bool _,bool value)
    {
        UpdateGameRuleOverview();
    }
    public void OnConfirmEjectsToggle(bool value)
    {
        isRule = false;
        isRuleToggle.isOn = false;
        confirmEjects = value;
    }

    [SyncVar(hook =nameof(SetEmergencyMeetings_Hook))]
    private int emergencyMeetings;
    [SerializeField]
    private Text emergencyMeetingsText;
    public void SetEmergencyMeetings_Hook(int _,int value)
    {
        emergencyMeetingsText.text = value.ToString();
        UpdateGameRuleOverview();
    }
    public void OnChangeEmergencyMeetings(bool isPlus)
    {
        emergencyMeetings = Mathf.Clamp(emergencyMeetings + (isPlus ? 1 : -1), 0, 9);
        isRule = false;
        isRuleToggle.isOn = false;
    }

    [SyncVar(hook =nameof(SetEmergencyMeetingCooldown_Hook))]
    private int emergencyMeetingsCooldown;
    [SerializeField]
    private Text emergencyMeetingsCooldownText;
    public void SetEmergencyMeetingCooldown_Hook(int _, int value)
    {
        emergencyMeetingsCooldownText.text = string.Format("{0}s", value);
        UpdateGameRuleOverview();
    }
    public void OnChangeEmergencyMeetingsCooldown(bool isPlus)
    {
        emergencyMeetingsCooldown = Mathf.Clamp(emergencyMeetingsCooldown + (isPlus ? 5 : -5), 0, 60);
        isRule = false;
        isRuleToggle.isOn = false;
    }

    [SyncVar(hook =nameof(SetMeetingsTime_Hook))]
    private int meetingsTime;
    [SerializeField]
    private Text meetingsTimeText;
    public void SetMeetingsTime_Hook(int _, int value)
    {
        meetingsTimeText.text = string.Format("{0}s", value);
        UpdateGameRuleOverview();
    }
    public void OnChangeMeetingsTime(bool isPlus)
    {
        meetingsTime = Mathf.Clamp(meetingsTime + (isPlus ? 5 : -5), 0, 120);
        isRule = false;
        isRuleToggle.isOn = false;
    }

    [SyncVar(hook =nameof(SetVoteTime_Hook))]
    private int voteTime;
    [SerializeField]
    private Text voteTimeText;
    public void SetVoteTime_Hook(int _,int value)
    {
        voteTimeText.text = string.Format("{0}s", value);
        UpdateGameRuleOverview();

    }
    public void OnChangeVoteTime(bool isPlus)
    {
        voteTime = Mathf.Clamp(voteTime + (isPlus ? 5 : -5), 0, 300);
        isRule = false;
        isRuleToggle.isOn = false;
    }

    [SyncVar(hook =nameof(SetAnonymousVotes_Hook))]
    private bool anonymousVotes;
    [SerializeField]
    private Toggle anonymousVotesToggle;
    public void SetAnonymousVotes_Hook(bool _,bool value)
    {
        UpdateGameRuleOverview();
    }
    public void OnAnonymouseVotesToggle(bool value)
    {
        isRule = false;
        isRuleToggle.isOn = false;
        anonymousVotes = value;
    }

    [SyncVar(hook =nameof(SetMoveSpeed_Hook))]
    private float moveSpeed;
    [SerializeField]
    private Text movespeedText;
    public void SetMoveSpeed_Hook(float _, float value)
    {
        movespeedText.text = string.Format("{0:0.00}x", value);
        UpdateGameRuleOverview();
    }
    public void OnChangeMoveSpeed(bool isPlus)
    {
        moveSpeed = Mathf.Clamp(moveSpeed + (isPlus ? 0.25f : -0.25f), 0.5f, 3f);
        isRule = false;
        isRuleToggle.isOn = false;
    }

    [SyncVar(hook =nameof(SetCrewSight_Hook))]
    private float crewSight;
    [SerializeField]
    private Text crewSightText;
    public void SetCrewSight_Hook(float _,float value)
    {
        crewSightText.text = string.Format("{0:0.00}x", value);
        UpdateGameRuleOverview();
    }
    public void OnChangeCrewSight(bool isPlus)
    {
        crewSight = Mathf.Clamp(crewSight + (isPlus ? 0.25f : -0.25f), 0.25f, 5f);
        isRule = false;
        isRuleToggle.isOn = false;
    }

    [SyncVar(hook =nameof(SetImposterSight_Hook))]
    private float imposterSight;
    [SerializeField]
    private Text imposterSightText;
    public void SetImposterSight_Hook(float _,float value)
    {
        imposterSightText.text = string.Format("{0:0.00}x", value);
        UpdateGameRuleOverview();
    }
    public void OnChangeImposterSight(bool isPlus)
    {
        imposterSight = Mathf.Clamp(imposterSight + (isPlus ? 0.25f : -0.25f), 0.25f, 5f);
        isRule = false;
        isRuleToggle.isOn = false;
    }

    [SyncVar(hook =nameof(SetKillCooldown_Hook))]
    private float KillCooldown;
    [SerializeField]
    private Text killCooldownText;
    public void SetKillCooldown_Hook(float _, float value)
    {
        killCooldownText.text = string.Format("{0:0.0}s", value);
        UpdateGameRuleOverview();
    }
    public void OnChangeKillCooldown(bool isPlus)
    {
        KillCooldown = Mathf.Clamp(KillCooldown + (isPlus ? 2.5f : -2.5f), 10f, 60f);
        isRule = false;
        isRuleToggle.isOn = false;
    }


    [SyncVar(hook = nameof(SetKillRange_Hook))]
    private EKillRange killRange;
    [SerializeField]
    private Text KillRangeText;
    public void SetKillRange_Hook(EKillRange _,EKillRange value)
    {
        KillRangeText.text = value.ToString();
        UpdateGameRuleOverview();
    }
    //열거형 캐스팅 방식
    public void OnChangeKillRange(bool isPlus)
    {
        killRange = (EKillRange)Mathf.Clamp((int)killRange + (isPlus ? 1 : -1), 0f, 2);
        isRule = false;
        isRuleToggle.isOn = false;
    }

    [SyncVar(hook = nameof(SetVisualTasks_Hook))]
    private bool visualTaske;
    [SerializeField]
    private Toggle visualTaskeToggle;
    public void SetVisualTasks_Hook(bool _,bool value)
    {
        UpdateGameRuleOverview();
    }
    public void OnVisualTasksToggle(bool value)
    {
        isRule = false;
        isRuleToggle.isOn = false;
        visualTaske = value;
    }


    [SyncVar(hook = nameof(SetTaskBarUpdates_Hook))]
    private ETaskBarUpdates taskBarUpdates;
    [SerializeField]
    private Text taskBarUpdatesText;
    public void SetTaskBarUpdates_Hook(ETaskBarUpdates _,ETaskBarUpdates value)
    {
        taskBarUpdatesText.text = value.ToString();
        UpdateGameRuleOverview();
    }
    public void OnChangeTaskBarUpdates(bool isPlus)
    {
        taskBarUpdates = (ETaskBarUpdates)Mathf.Clamp((int)taskBarUpdates + (isPlus ? 1 : -1), 0f, 2);
        isRule = false;
        isRuleToggle.isOn = false;
    }

    [SyncVar(hook =nameof(SetCommonTask_Hook))]
    private int commonTask;
    [SerializeField]
    private Text commonTaskText;
    public void SetCommonTask_Hook(int _, int value)
    {
        commonTaskText.text = value.ToString();
        UpdateGameRuleOverview();
    }
    public void OnChangeCommonTask(bool isPlus)
    {
        commonTask = Mathf.Clamp(commonTask + (isPlus ? 1 : -1), 0, 2);
        isRule = false;
        isRuleToggle.isOn = false;
    }


    [SyncVar(hook =nameof(SetComplexTask_Hook))]
    private int complexTask;
    [SerializeField]
    private Text complexTaskText;
    public void SetComplexTask_Hook(int _, int value)
    {
        complexTaskText.text = value.ToString();
        UpdateGameRuleOverview();
    }
    public void OnChangeComplexTask(bool isPlus)
    {
        complexTask = Mathf.Clamp(complexTask + (isPlus ? 1 : -1), 0, 3);
        isRule = false;
        isRuleToggle.isOn = false;
    }

    [SyncVar(hook =nameof(SetSimpleTask_Hook))]
    private int simpleTask;
    [SerializeField]
    private Text simpleTaskText;
    public void SetSimpleTask_Hook(int _, int value)
    {
        simpleTaskText.text = value.ToString();
        UpdateGameRuleOverview();
    }
    public void OnChangeSimpleTask(bool isPlus)
    {
        simpleTask = Mathf.Clamp(simpleTask + (isPlus ? 1 : -1), 0, 5);
        isRule = false;
        isRuleToggle.isOn = false;
    }

    [SyncVar(hook =nameof(SetImposterCount_Hook))]
    private int imposterCount;
    private void SetImposterCount_Hook(int _, int value)
    {
        UpdateGameRuleOverview();
    }

    [SerializeField]
    private Text gameRuleOverview;

    private void UpdateGameRuleOverview()
    {
        var manager = NetworkManager.singleton as RoomManager; //캐스팅 
        StringBuilder sb = new StringBuilder(isRule ? "추천 설정\n" : "커스텀 설정\n");
        sb.Append("맵:The Skeld\n");
        sb.Append($"#임포스터:{imposterCount}\n");
        sb.Append(string.Format("Confirm Ejects:{0}\n", confirmEjects ? "켜짐" : "꺼짐"));
        sb.Append($"긴급 회의:{emergencyMeetings}\n");
        sb.Append(string.Format("Anonymous Votes:{0}\n", anonymousVotes ? "켜짐" : "꺼짐"));
        sb.Append($"긴급 회의 쿨타임:{emergencyMeetingsCooldown}\n");
        sb.Append($"회의 제한 시간:{meetingsTime}\n");
        sb.Append($"투표 제한 시간:{voteTime}\n");
        sb.Append($"이동 속도:{moveSpeed}\n");
        sb.Append($"크루원 시야:{crewSight}\n");
        sb.Append($"임포스터 시야:{imposterSight}\n");
        sb.Append($"킬 쿨타임{KillCooldown}\n");
        sb.Append($"킬 범위:{killRange}\n");
        sb.Append($"Task Bar Updates:{taskBarUpdates}\n");
        sb.Append(string.Format("Visual Tasks:{0}\n", visualTaske ? "켜짐" : "꺼짐"));
        sb.Append($"공통 임무:{commonTask}\n");
        sb.Append($"복잡한 임무:{complexTask}\n");
        sb.Append($"간단한 임무{simpleTask}\n");
        gameRuleOverview.text = sb.ToString();
    }
    private void SetRecommendGameRule()
    {
        isRule = true;
        confirmEjects = true;
        emergencyMeetings = 1;
        emergencyMeetingsCooldown = 15;
        meetingsTime = 15;
        voteTime = 120;
        moveSpeed = 1f;
        crewSight = 1f;
        imposterSight = 1f;
        KillCooldown = 45f;
        killRange = EKillRange.Normal;
        visualTaske = true;
        commonTask = 1;
        complexTask = 1;
        simpleTask = 2;
    }
    private void Start()
    {
        if (isServer)
        {
            var manager = NetworkManager.singleton as RoomManager;
            imposterCount = manager.imposterCount;
            anonymousVotes = false;
            taskBarUpdates = ETaskBarUpdates.Always;
            SetRecommendGameRule();
        }
    }
}
