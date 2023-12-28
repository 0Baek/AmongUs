using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class CharacterMover : NetworkBehaviour
{
    protected Animator animator; // 상속 스크립트 사용가능

    private bool isMoveable;
    public bool IsMoveable
    {
        get
        {
            return isMoveable;
        }
        set
        {
            if (!value) //받아온 벨류값이 false일 때 애니메이션도 false 움직임 제어
            {
                animator.SetBool("isMove", false);
            }
            isMoveable = value;
        }
    }
    
    [SyncVar]
    public float speed = 2f;

    [SerializeField]
    private float characterSize = 0.5f;

    [SerializeField]
    private float cameraSize = 2.5f;

    protected SpriteRenderer spriteRenderer;

    [SyncVar(hook =nameof(SetPlayerColor_Hook))]
    public EPlayerColor playercolor;

    [SyncVar(hook =nameof(SetNickname_Hook))]
    public string nickname;
    [SerializeField]
    protected Text nicknameText;  // private -> protected // 상속받은 inGameCharacterMover에서 쓰기 위해

    public void SetNickname_Hook(string _, string value)
    {
        nicknameText.text = value;
    }

    public void SetPlayerColor_Hook(EPlayerColor oldColor, EPlayerColor newColor)
    {
        if (spriteRenderer==null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(newColor));
    }

    public virtual void Start() //InGameCharacterMover에서 재정의 할 수 있게 public virtual  12/26
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(playercolor));

        animator = GetComponent<Animator>();
        if (isOwned)
        {
            Camera cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 0f, -10f);
            cam.orthographicSize = cameraSize;
        }
    }
    private void FixedUpdate()
    {
        Move();
    }

    public void Move()
    {
        if (isOwned&& isMoveable)
        {
            bool isMove = false;
            if (PlayerSettings.countrolType == ECountrolType.KeyboardMouse)
            {
                Vector3 dir =  //대각선 이동 시에도 일정한 이동 속도를 유지대각선 이동 시에도 일정한 이동 속도를 유지
                    Vector3.ClampMagnitude(new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0f), 1f);
                if (dir.x<0f)
                {
                    transform.localScale = new Vector3(-characterSize, characterSize, 1f);
                }
                else if (dir.x>0f)
                {
                    transform.localScale = new Vector3(characterSize, characterSize, 1f);
                }
                transform.position += dir * speed * Time.deltaTime;
                isMove = dir.magnitude != 0f;

            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    Vector3 dir =//normalized를 사용함으로써 이동 방향이 속도와 분리되어 마우스 입력에 기반한 물체의 일관된 속도와 향상된 제어
                        (Input.mousePosition - new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f)).normalized;
                    if (dir.x<0f)
                    {
                        transform.localScale = new Vector3(-characterSize, characterSize, 1f);
                    }
                    else if (dir.x>0f)
                    {
                        transform.localScale = new Vector3(characterSize, characterSize, 1f);
                    }
                    transform.position += dir * speed * Time.deltaTime;
                    isMove = dir.magnitude != 0f;
                }
                
            }
            animator.SetBool("isMove", isMove);

        }
        if (transform.localScale.x<0)
        {
            nicknameText.transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        else if (transform.localScale.x>0)
        {
            nicknameText.transform.localScale = new Vector3(1f, 1f, 1f);
        }
    }


}
   
