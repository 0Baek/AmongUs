using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class LobbyUIManager : MonoBehaviour
{
    public static LobbyUIManager instance;
 

    [SerializeField] private CustumizeUI custumizeUI;

    public CustumizeUI CustumizeUI { get { return custumizeUI; } }

    [SerializeField]
    private Button useButton;
    [SerializeField]
    private Sprite originUseButtonSprite;
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

}
