using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameSystem : NetworkBehaviour
{
    public static GameSystem instance;

    private List<InGameCharacterMover> players = new List<InGameCharacterMover>();

    [SerializeField]
    private Transform spawnTransform;

    [SerializeField]
    private float spawnDistance; //���� Ʈ������ ������ �Ÿ�

    [SyncVar]
    public float killCooldown;
    [SyncVar]
    public int killRange;
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

        while (manager.roomSlots.Count!=players.Count)
        {
            yield return null;
        }
        for (int i = 0; i < manager.imposterCount; i++)
        {
            var player = players[Random.Range(0, players.Count)];
            if (player.playerType !=EPlayerType.Imposter)
            {
                player.playerType = EPlayerType.Imposter;
            }
            else
            {
                i--;
            }
        }
        for (int i = 0; i < players.Count; i++)
        {
            float radian = (2f * Mathf.PI) / players.Count;
            radian *= i;
            players[i].RpcTeleport(spawnTransform.position + (new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0f) * spawnDistance));
        }

        yield return new WaitForSeconds(1f);
        //yield return StartCoroutine(InGameUIManager.Instance.IngameintroUI.ShowIntroSequence());

        RpcStartGame(); //ȣ��Ʈ�� Ŭ���̾�Ʈ ���ʿ��� ���� �� �Լ� 

        foreach (var player in players)
        {
            player.SetKillCooldown();
        }
    }
    [ClientRpc]
    private void RpcStartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }
    private IEnumerator StartGameCoroutine()
    {
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
}
