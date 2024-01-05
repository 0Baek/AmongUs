using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;
public class CreateGameRoomData
{
    public int imposterCount;
    public int maxPlayerCount;
}

public class CreateRoomUI : MonoBehaviour
{
    [SerializeField]
    private List<Image> crewImags;

    [SerializeField]
    private List<Button> imposterCountBtn;

    [SerializeField]
    private List<Button> maxPlayerCountBtn;

    private CreateGameRoomData roomData;

    private void Start()
    {
        for (int i = 0; i < crewImags.Count; i++)
        {
            Material materialInstance = Instantiate(crewImags[i].material);
            crewImags[i].material = materialInstance;
        }
        roomData = new CreateGameRoomData() { imposterCount = 1, maxPlayerCount = 10 };
        UpdateCrewImages();
    }
    public void updateImposter(int count)
    {
        roomData.imposterCount = count;
        for (int i = 0; i < imposterCountBtn.Count; i++)
        {
            if (i==count -1)
            {
                imposterCountBtn[i].image.color = new Color(1f, 1f, 1f, 1f);
            }
            else
            {
                imposterCountBtn[i].image.color = new Color(1f, 1f, 1f, 0f);
            }
        }
        //count가 1이면, limitMaxPlayer는 4가 되고, 그렇지 않으면 다음 조건을 확인합니다.
        //만약 count가 2이면, limitMaxPlayer는 7이 되고, 그렇지 않으면 기본적으로 limitMaxPlayer는 9가 됩니다.
        int limitMaxPlayer = count == 1 ? 4 : count == 2 ? 7 : 9;
        if (roomData.maxPlayerCount<limitMaxPlayer)
        {
            updateMaxPlayerCount(limitMaxPlayer);
        }
        else
        {
            updateMaxPlayerCount(roomData.maxPlayerCount);
        }
        for (int i = 0; i < maxPlayerCountBtn.Count; i++)
        {
            var text = maxPlayerCountBtn[i].GetComponentInChildren<Text>();
         


            if (i<limitMaxPlayer-4)
            {
               
                Debug.Log(limitMaxPlayer);
                maxPlayerCountBtn[i].interactable = false;
                Debug.Log(i);
                text.color = Color.gray;
            }
            else
            {
              
                maxPlayerCountBtn[i].interactable= true;
                text.color = Color.white;

            }
           
        }
    }
    public void updateMaxPlayerCount(int count)
    {
        AudioManager.instance.PlaySFX("Next");
        roomData.maxPlayerCount = count;
        Debug.Log(count);
        for (int i = 0; i < maxPlayerCountBtn.Count; i++)
        {
            if (i==count-4)
            {
                maxPlayerCountBtn[i].image.color = new Color(1f, 1f, 1f, 1f); 
            }
            else
            {
                maxPlayerCountBtn[i].image.color = new Color(1f, 1f, 1f, 0f);
            }
        }
        UpdateCrewImages();
    }
    private void UpdateCrewImages()
    {
        for (int i = 0; i < crewImags.Count; i++)
        {
            crewImags[i].material.SetColor("_PlayerColor", Color.white);
        }
        int imposterCount = roomData.imposterCount;
        int idx = 0;
        while (imposterCount !=0)
        {
            if (idx >= roomData.maxPlayerCount)
            {
                idx = 0;
            }
            if (crewImags[idx].material.GetColor("_PlayerColor")!=Color.red &&Random.Range(0,5)==0)
            {
                crewImags[idx].material.SetColor("_PlayerColor", Color.red);
                imposterCount--;
            }
            idx++;
        }
        for (int i = 0; i < crewImags.Count; i++)
        {
            if (i<roomData.maxPlayerCount )
            {
                crewImags[i].gameObject.SetActive(true);
            }
            else
            {
                crewImags[i].gameObject.SetActive(false);
            }
        }
    }
    public void CreateRoom()
    {
        // 1번 var manager = RoomManager.singleton;
        //캐스팅
        var manager = NetworkManager.singleton as RoomManager;
        AudioManager.instance.PlaySFX("Next");
        //방설정 
        manager.minPlayerCount = roomData.imposterCount == 1 ? 4 : roomData.imposterCount == 2 ? 7 : 9;
        manager.imposterCount = roomData.imposterCount;
        manager.maxConnections = roomData.maxPlayerCount;
        manager.StartHost();
    }

    
}
