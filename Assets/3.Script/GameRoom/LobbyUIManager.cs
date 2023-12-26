using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using Mirror;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager instance;
 

    [SerializeField] private CustumizeUI custumizeUI;
    public CustumizeUI CustumizeUI { get { return custumizeUI; } }
     

    [SerializeField]
    private Button useButton;
    [SerializeField]
    private Sprite originUseButtonSprite;

    [SerializeField]
    private Button startButton;

  

    [SerializeField]
    private GameRoomPlayerCount gameRoomPlayerCount;
    public  GameRoomPlayerCount GameRoomPlayerCount
    {
        get
        {
            return gameRoomPlayerCount;
        }
    }

 
    private void Awake()
    {
        instance = this;
    }
    public void SetUseButton(Sprite sprite, UnityAction action)
    {
        useButton.image.sprite = sprite;           // 버튼 이미지를 주어진 스프라이트로 설정
        useButton.onClick.AddListener(action);     // 버튼 클릭 이벤트에 주어진 액션(함수)를 추가
        useButton.interactable = true;             // 버튼을 상호 작용 가능하도록 설정
    }
    public void UnsetUseButton()
    {
        useButton.image.sprite = originUseButtonSprite;   // 버튼 이미지를 초기 스프라이트로 설정
        useButton.onClick.RemoveAllListeners();          // 모든 클릭 이벤트 리스너를 제거
        useButton.interactable = false;                  // 버튼을 비활성화
    }
    public void ActiveStartButton()
    {
        startButton.gameObject.SetActive(true);
    }
    public void SetInteractableStartButton(bool isInteractable)
    {
        startButton.interactable = isInteractable;
    }
     public void OnClickStartButton()
    {
        var players = FindObjectsOfType<AmongUsRoomplayer>(); 
        
        for (int i = 0; i < players.Length; i++)
        {
            // players[i].CmdChangeReadyState(true);     12.26 문제해결  주석처리코드로 빌드할시 클라이언트들이 게임씬으로 가면 다 튕겨버림 
            players[i].CmdSetConnection(true);
        }
        var manager = NetworkManager.singleton as RoomManager;
        manager.gameRuleData = FindObjectOfType<GameRuleStore>().GetGameRuleData();
       
        manager.ServerChangeScene(manager.GameplayScene);
    }


}
