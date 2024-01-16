using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using Mirror;

public class GameSystem : NetworkBehaviour
{
    public static GameSystem instance;

  

    private List<InGameCharacterMover> players = new List<InGameCharacterMover>();

    private Queue<string> chatMessages = new Queue<string>();
  
    [SyncVar]
    public int maxMessageCount = 13;

    [SerializeField]
    private Transform spawnTransform;

    [SerializeField]
    private float spawnDistance; //���� Ʈ������ ������ �Ÿ�

    [SyncVar]
    public float killCooldown;
    [SyncVar]
    public int killRange;
    [SyncVar]
    public int skipVotePlayerCount;
    [SyncVar]
    public float remainTime;



    [SerializeField]
    private Light2D shadowLight;
    [SerializeField]
    private Light2D lightMapLight;
    [SerializeField]
    private Light2D globalLight;

    public void AddPlayer(InGameCharacterMover player)
    {
        if (!players.Contains(player))
        {
            players.Add(player);
        }
    }
    private IEnumerator GameReady()
    {
        var manager = NetworkManager.singleton as RoomManager;

        killCooldown = manager.gameRuleData.KillCooldown;
        killRange = (int)manager.gameRuleData.killRange;

        while (manager.roomSlots.Count != players.Count)
        {
            yield return null;
        }
        for (int i = 0; i < manager.imposterCount; i++)
        {
            var player = players[Random.Range(0, players.Count)];
            if (player.playerType != EPlayerType.Imposter)
            {
                player.playerType = EPlayerType.Imposter;
            }
            else
            {
                i--;
            }
        }
        PlayerTable(players.ToArray());

        yield return new WaitForSeconds(1f);
        //yield return StartCoroutine(InGameUIManager.Instance.IngameintroUI.ShowIntroSequence());

        RpcStartGame(); //ȣ��Ʈ�� Ŭ���̾�Ʈ ���ʿ��� ���� �� �Լ� 

        foreach (var player in players)
        {
            player.SetKillCooldown();
        }
    }

    private void PlayerTable(InGameCharacterMover[] players)
    {
        for (int i = 0; i < players.Length; i++)
        {
            float radian = (2f * Mathf.PI) / players.Length;
            radian *= i;
            players[i].RpcTeleport(spawnTransform.position + (new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f) * spawnDistance));
        }
    }

    [ClientRpc]
    private void RpcStartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }
    private IEnumerator StartGameCoroutine()
    {
        AudioManager.instance.StopBGM();

        yield return StartCoroutine(InGameUIManager.Instance.IngameintroUI.ShowIntroSequence());

        InGameCharacterMover myCharacter = null;
        foreach (var player in players)
        {
            if (player.isOwned)
            {
                myCharacter = player;
                break;
            }
        }
        foreach (var player in players)
        {
            player.SetNicknameColor(myCharacter.playerType);
        }
        yield return new WaitForSeconds(3f);
        InGameUIManager.Instance.IngameintroUI.Close();
    }
    public List<InGameCharacterMover> GetPlayerList()
    {
        return players;
    }

    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        if (isServer) //ȣ��Ʈ������ �����ϰ�
        {
            StartCoroutine(GameReady());
        }
        
    }
  /*  public bool CheckCrewExists()
    {
        var players = FindObjectsOfType<InGameCharacterMover>();

        foreach (var player in players)
        {
            if (player.playerType == EPlayerType.Crew)
            {
                // ũ����� �����ϴ� ���
                return true;
            }
        }

        // ũ����� �������� �ʴ� ���
        return false;
    }*/
    public void ChangeLightMode(EPlayerType type)
    {
        if (type == EPlayerType.Ghost)
        {
            lightMapLight.lightType = Light2D.LightType.Global;
            shadowLight.intensity = 0f;
            globalLight.intensity = 1f;
        }
        else if (type == EPlayerType.Imposter_Ghost)
        {
            lightMapLight.lightType = Light2D.LightType.Global;
            shadowLight.intensity = 0f;
            globalLight.intensity = 1f;
        }
        else
        {
            lightMapLight.lightType = Light2D.LightType.Point;
            shadowLight.intensity = 0.5f;
            globalLight.intensity = 0.5f;
        }
    }
    public void StartReportMeeting(EPlayerColor deadbodyColor)
    {
        RpcSendReportSign(deadbodyColor);
        StartCoroutine(MeetingProcess_Coroutine());
    }
    private IEnumerator StartMeeting_Coroutine()
    {
        yield return new WaitForSeconds(3f);
        InGameUIManager.Instance.ReportUI.Close();
        InGameUIManager.Instance.MeetingUI.Open();
        InGameUIManager.Instance.MeetingUI.ChangeMeetingState(EMeetimgState.Meeting);
    }
    private IEnumerator MeetingProcess_Coroutine()
    {
        var players = FindObjectsOfType<InGameCharacterMover>();
        foreach (var player in players)
        {
            player.isVote = true;
        }
        yield return new WaitForSeconds(3f);

        var manager = NetworkManager.singleton as RoomManager;
        remainTime = manager.gameRuleData.meetingsTime;
        while (true)
        {
            remainTime -= Time.deltaTime;
            yield return null;
            if (remainTime <= 0f)
            {
                break;
            }
        }
        skipVotePlayerCount = 0;
            
        foreach (var player in players)
        {
            if ((player.playerType&EPlayerType.Ghost)!=EPlayerType.Ghost)
            {
                player.isVote = false;
            }
            player.vote = 0;
        }
        RpcStartVoteTime();
        remainTime = manager.gameRuleData.voteTime;
        while (true)
        {
            remainTime -= Time.deltaTime;
            yield return null;
            if (remainTime<=0f)
            {
                break;
            }
        }
        foreach(var player in players)
        {
            if (!player.isVote&&(player.playerType&EPlayerType.Ghost)!=EPlayerType.Ghost)
            {
                player.isVote = true;
                skipVotePlayerCount += 1;
                RpcSignSkipVote(player.playercolor);
            }
        }
        RpcEndVoteTime();

        yield return new WaitForSeconds(3f);

        StartCoroutine(CalculateVoteResult_Coroutine(players));
    }
    public class CharacterVoteComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            InGameCharacterMover xplayer = (InGameCharacterMover)x;
            InGameCharacterMover yplayer = (InGameCharacterMover)y;
            return xplayer.vote <= yplayer.vote ? 1 : -1;
        }
    }

        private IEnumerator CalculateVoteResult_Coroutine(InGameCharacterMover[] players)
    {
        System.Array.Sort(players, new CharacterVoteComparer());

        int remainImposter = 0;
        int remainCrew = 0;

        foreach(var player in players)
        {
            if ((player.playerType & EPlayerType.Imposter_Alive) == EPlayerType.Imposter_Alive)
            {
                remainImposter++;
            }
            else if (player.playerType == EPlayerType.Crew)
            {
                remainCrew++;
            }
        }
        //��ŵ ǥ ���� ���� ���� ���� ǥ ������  ���ų� ���ٸ� �÷��̾�� �߹� ������ �ʴ´�.

        if (skipVotePlayerCount >= players[0].vote)
        {
            RpcOpenEjectionUI(false, EPlayerColor.Black, false, remainImposter,remainCrew);
        }
        //���� ���� ǥ�� ���� �÷��̾�� 2���� �÷��̾ ������ ��쵵 �߹��� �̷������ �ʴ´�.

        else if(players[0].vote == players[1].vote)
        {
            RpcOpenEjectionUI(false, EPlayerColor.Black, false, remainImposter,remainCrew);
        }
        //��ŵ ǥ�� 2���� ǥ���� 1���� ǥ�� ���� ��쿡��  1���� �÷��̾ �߹� ��Ų��.
        else
        {
            bool isImposter = (players[0].playerType & EPlayerType.Imposter) == EPlayerType.Imposter;
            RpcOpenEjectionUI(true, players[0].playercolor, isImposter, isImposter ? remainImposter - 1:remainImposter,remainCrew) ;

            
            players[0].Dead(true);
        }
      
        var deadbodys = FindObjectsOfType<Deadbody>();
        for (int i = 0; i < deadbodys.Length; i++)
        {
            Destroy(deadbodys[i].gameObject);
        }
        PlayerTable(players);

        yield return new WaitForSeconds(8f);


        RpcCloseEjectionUI();
        
     



    }
    public IEnumerator ImposterVictory(InGameCharacterMover[] players)
    {
        System.Array.Sort(players, new CharacterVoteComparer());

       
        int remainCrew = 0;

        foreach (var player in players)
        {
            if (player.playerType == EPlayerType.Crew)
            {
                remainCrew++;
            }
        }
        foreach (var player in players)
        {
            if (player.playerType == EPlayerType.Crew)
            {
                remainCrew--;
                player.Kill(); // �Ǵ� Kill �޼��� ȣ��
            }
        }
        yield return new WaitForSeconds(2f);
        // remainCrew�� Rpc �޼���� ����
        RpcImposterVictory(remainCrew);
    }
    [ClientRpc]
    public void RpcImposterVictory(int remainCrew)
    {
        InGameUIManager.Instance.KillUI.Open2(remainCrew);
    }
    //�� �޼���� �������� ȣ��� �����̱� ������ Rpc ��Ʈ����Ʈ�� ���������Ѵ�.
    [ClientRpc]
    public void RpcOpenEjectionUI(bool isEjection,EPlayerColor ejectionPlayerColor, bool isImposter,int remainImposterCount,int remainCrewCount)
    {
        InGameUIManager.Instance.EjectionUI.Open(isEjection,ejectionPlayerColor,isImposter,remainImposterCount,remainCrewCount);
        InGameUIManager.Instance.MeetingUI.Close();
    }
    [ClientRpc]
    public void RpcCloseEjectionUI()
    {
        InGameUIManager.Instance.EjectionUI.Close();
        AmongUsRoomplayer.MyRoomPlayer.myCharacter.IsMoveable = true;
    }
    [ClientRpc]
     public void RpcStartVoteTime()
    {
        InGameUIManager.Instance.MeetingUI.ChangeMeetingState(EMeetimgState.Vote);
    }
    [ClientRpc]
    public void RpcEndVoteTime()
    {
        InGameUIManager.Instance.MeetingUI.CompleteVote();
    }
    [ClientRpc]
    private void RpcSendReportSign(EPlayerColor deadbodyColor)
    {
        InGameUIManager.Instance.ReportUI.Open(deadbodyColor);
        StartCoroutine(StartMeeting_Coroutine());
    }
    [ClientRpc]
    public void RpcSignVoteEject(EPlayerColor voterColor,EPlayerColor ejectColor)
    {
        InGameUIManager.Instance.MeetingUI.UpdateVote(voterColor, ejectColor);
    }
    [ClientRpc]
    public void RpcSignSkipVote(EPlayerColor skipVotePlayerColor)
    {
        InGameUIManager.Instance.MeetingUI.UpdateSkipVotePlayer(skipVotePlayerColor);
    }
    [ClientRpc]
    public void RpcReceiveChatMessage(string message)
    {
        chatMessages.Enqueue(message);

        while (chatMessages.Count > maxMessageCount)
        {

            chatMessages.Dequeue();
        }
        InGameUIManager.Instance.MeetingUI.chatText.text = string.Join("", chatMessages.ToArray());
        InGameUIManager.Instance.MeetingUI.ChatSign.SetActive(true);

    }

}
