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

}
