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
    private float spawnDistance; //스폰 트랜스폼 사이의 거리
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

        RpcStartGame(); //호스트와 클라이언트 양쪽에서 실행 될 함수 
    }
    [ClientRpc]
    private void RpcStartGame()
    {
        StartCoroutine(StartGameCoroutine());
    }
    private IEnumerator StartGameCoroutine()
    {
        yield return StartCoroutine(InGameUIManager.Instance.IngameintroUI.ShowIntroSequence());

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
        if (isServer) //호스트에서만 동작하게
        {
            StartCoroutine(GameReady());
        }
        
    }
}
