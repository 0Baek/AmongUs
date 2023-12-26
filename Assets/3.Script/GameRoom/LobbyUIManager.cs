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
        useButton.image.sprite = sprite;           // ��ư �̹����� �־��� ��������Ʈ�� ����
        useButton.onClick.AddListener(action);     // ��ư Ŭ�� �̺�Ʈ�� �־��� �׼�(�Լ�)�� �߰�
        useButton.interactable = true;             // ��ư�� ��ȣ �ۿ� �����ϵ��� ����
    }
    public void UnsetUseButton()
    {
        useButton.image.sprite = originUseButtonSprite;   // ��ư �̹����� �ʱ� ��������Ʈ�� ����
        useButton.onClick.RemoveAllListeners();          // ��� Ŭ�� �̺�Ʈ �����ʸ� ����
        useButton.interactable = false;                  // ��ư�� ��Ȱ��ȭ
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
            // players[i].CmdChangeReadyState(true);     12.26 �����ذ�  �ּ�ó���ڵ�� �����ҽ� Ŭ���̾�Ʈ���� ���Ӿ����� ���� �� ƨ�ܹ��� 
            players[i].CmdSetConnection(true);
        }
        var manager = NetworkManager.singleton as RoomManager;
        manager.gameRuleData = FindObjectOfType<GameRuleStore>().GetGameRuleData();
       
        manager.ServerChangeScene(manager.GameplayScene);
    }


}
