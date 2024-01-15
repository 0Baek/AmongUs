using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

// 0x :   0 = 크루원 1= 임포스터 
// x0 :   0 = 살아있음 1 = 죽음 
// 00 = 살아있는 크루원 01 = 살아있는 임포스터 
// 10 - 죽은 크루원    11 = 죽은 임포스터 

public enum EPlayerType
{
    Crew = 0,
    Imposter = 1,
    Ghost = 2,
    Crew_Alive = 0,
    Imposter_Alive = 1,
    Crew_Ghost = 2,
    Imposter_Ghost = 3,
}

public class InGameCharacterMover : CharacterMover
{
    [SyncVar(hook = nameof(SetPlayerType_Hook))]
    public EPlayerType playerType;

    private void SetPlayerType_Hook(EPlayerType _, EPlayerType type)
    {
        if (isOwned&&type ==EPlayerType.Imposter)
        {
            InGameUIManager.Instance.Kill_BtnUI.Show(this);
            playerFinder.SetkillRange(GameSystem.instance.killRange + 1f);
        }
        else if(isOwned&&type ==EPlayerType.Imposter_Ghost)
        {
            InGameUIManager.Instance.Kill_BtnUI.Close(this);
        }
      
       
    }
    [SerializeField]
    private PlayerFinder playerFinder;
    [SyncVar]
    private float killCooldown;
    public float KillCooldown
    {
        get
        {
            return killCooldown;
        }
    }

    public bool isKillable
    {
        get
        {
            return killCooldown < 0f&&playerFinder.targets.Count!=0;
        }
    }
    public EPlayerColor foundDeadbodyColor; // 시체 색 발견 변수

    [SyncVar]
    public bool isReporter = false;
    [SyncVar]
    public bool isVote;
    [SyncVar]
    public int vote;
    [ClientRpc]
    public void RpcTeleport(Vector3 position)
    {
        transform.position = position;
    }
    public void SetNicknameColor(EPlayerType type)
    {
        if (playerType ==EPlayerType.Imposter && type == EPlayerType.Imposter)
        {
            nicknameText.color = Color.red;
        }
    }
    public void SetKillCooldown()
    {
        if (isServer)
        {
            killCooldown = GameSystem.instance.killCooldown;
        }
    }
 
    public override void Start()  //CharacterMover 클래스의 Start 함수를 덮어쓰도록 하기 위함
    {
        base.Start();
        if (isOwned)
        {
            IsMoveable = true;

            var myRoomPlayer = AmongUsRoomplayer.MyRoomPlayer;
            myRoomPlayer.myCharacter = this;
            CmdSetPlayerCharacter(myRoomPlayer.nickname, myRoomPlayer.playerColor);
        }
        GameSystem.instance.AddPlayer(this);
    }
    private void Update()
    {
        if (isServer && playerType ==EPlayerType.Imposter)
        {
            killCooldown -= Time.deltaTime;
        }
    }
    [Command]
    private void CmdSetPlayerCharacter(string nickname ,EPlayerColor color)
    {
        this.nickname = nickname;
        playercolor = color;
    }
    public void Kill()
    {
        CmdKill(playerFinder.GetFirstTarget().netId);
    }

    [SerializeField]
    private GameObject ImposterVictory;
    [Command]
    private void CmdKill(uint targetNetId)
    {

        InGameCharacterMover target = null;
        foreach (var player in GameSystem.instance.GetPlayerList())
        {
            if (player.netId == targetNetId)
            {
                target = player;
            }
        }
        if (target !=null)
        {
            RpcTeleport(target.transform.position);
            target.Dead(false,playercolor);
            killCooldown = GameSystem.instance.killCooldown;
        }
      

    }
 
    public void Dead(bool isEject,EPlayerColor imposterColor=EPlayerColor.Black)
    {
        playerType |= EPlayerType.Ghost;
        RpcDead(isEject, imposterColor, playercolor);
       
        if (!isEject) // 추방으로 죽은게 아닐때만
        {
            var manager = NetworkRoomManager.singleton as RoomManager;
            var deadbody = Instantiate(manager.spawnPrefabs[1], transform.position, transform.rotation).GetComponent<Deadbody>();
            NetworkServer.Spawn(deadbody.gameObject);
            deadbody.RpcSetColor(playercolor);
            AudioManager.instance.PlaySFX("Kill");
           
        }
       

    }

    [ClientRpc]
    private void RpcDead(bool isEject,EPlayerColor imposterColor,EPlayerColor crewColor)
    {
        if (isOwned) // 죽은 크루원이 자신 일 때만 킬ui 띄우기
        {
            animator.SetBool("isGhost", true);
            if (!isEject) // 킬ui가 추방으로 죽은게 아닐 때만
            {
                InGameUIManager.Instance.KillUI.Open(imposterColor, crewColor);
            }
          
            var players = GameSystem.instance.GetPlayerList();
            foreach (var player in players)
            {
                if ((player.playerType & EPlayerType.Ghost)==EPlayerType.Ghost)
                {
                    player.SetVisibility(true);
                }
            }
            GameSystem.instance.ChangeLightMode(EPlayerType.Ghost);
            GameSystem.instance.ChangeLightMode(EPlayerType.Imposter_Ghost);
        }
        else
        {
            var myPlayer = AmongUsRoomplayer.MyRoomPlayer.myCharacter as InGameCharacterMover;
            if (((int)myPlayer.playerType & 0x02)!=(int)EPlayerType.Ghost)
            {
                SetVisibility(false);
            }
        }
        var collider = GetComponent<BoxCollider2D>();
        if (collider)
        {
            collider.enabled = false;
            audioSource.enabled = false;
        }
       
    }
    public void Report()
    {
        CmdReport(foundDeadbodyColor);
    }
    [Command]
    public void CmdReport(EPlayerColor deadbodyColor)
    {
        isReporter = true;
        GameSystem.instance.StartReportMeeting(deadbodyColor);
       
    }
    public void SetVisibility(bool isVisible)
    {
        if (isVisible)
        {
            
            var color = PlayerColor.GetColor(playercolor);
            color.a = 1f;
          
            spriteRenderer.material.SetColor("_PlayerColor", color); 
           
            nicknameText.text = nickname;
           
        }
        else
        {
            var color = PlayerColor.GetColor(playercolor);

           /* color.a = 0f;
            spriteRenderer.material.SetColor("_MainTex", color);*/
            color.a = 0f;
            spriteRenderer.material.SetColor("_PlayerColor", color);
          
            nicknameText.text = "";
           
        }
    }

    [Command]
    public void CmdVoteEjectPlayer(EPlayerColor ejectColor)
    {
        isVote = true;
        GameSystem.instance.RpcSignVoteEject(playercolor, ejectColor);

        var players = FindObjectsOfType<InGameCharacterMover>();
        InGameCharacterMover ejectedPlayer = null;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].playercolor== ejectColor)
            {
                ejectedPlayer = players[i];
            }
        }
        ejectedPlayer.vote += 1;
    }
    [Command]
    public void CmdSkipVote()
    {
        isVote = true;
        GameSystem.instance.skipVotePlayerCount += 1;
        GameSystem.instance.RpcSignSkipVote(playercolor);
    }
    [Command]
   public void CmdSendChatMessage(string message)
    {
        InGameUIManager.Instance.MeetingUI.ChatSign.SetActive(true);
        AudioManager.instance.PlaySFX("Chat");
        GameSystem.instance.RpcReceiveChatMessage(message);
    }

    /*    private void SetTextureAlpha(SpriteRenderer spriteRenderer, string texturePropertyName, float alpha)
        {
            Material material = spriteRenderer.material;

            // Clone the material to avoid modifying the shared material
            material = new Material(material);

            // Get the main texture from the material
            Texture2D mainTexture = (Texture2D)material.GetTexture(texturePropertyName);

            // Clone the texture to avoid modifying the shared texture
            mainTexture = Instantiate(mainTexture);

            // Get the color array from the texture
            Color[] colors = mainTexture.GetPixels();

            // Modify the alpha value for each pixel
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i].a = alpha;
            }

            // Apply the modified color array to the texture
            mainTexture.SetPixels(colors);

            // Apply changes and set the modified texture to the material
            mainTexture.Apply();
            material.SetTexture(texturePropertyName, mainTexture);

            // Set the material back to the sprite renderer
            spriteRenderer.material = material;
        }*/

}
