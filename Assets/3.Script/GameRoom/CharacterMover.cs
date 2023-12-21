using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterMover : NetworkBehaviour
{
    private Animator animator;

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

    private SpriteRenderer spriteRenderer;

    [SyncVar(hook =nameof(SetPlayerColor_Hook))]
    public EPlayerColor playercolor;

    public void SetPlayerColor_Hook(EPlayerColor oldColor, EPlayerColor newColor)
    {
        if (spriteRenderer==null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(newColor));
    }

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(playercolor));

        animator = GetComponent<Animator>();
        if (isOwned)
        {
            Camera cam = Camera.main;
            cam.transform.SetParent(transform);
            cam.transform.localPosition = new Vector3(0f, 0f, -10f);
            cam.orthographicSize = 2.5f;
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
                    transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
                }
                else if (dir.x>0f)
                {
                    transform.localScale = new Vector3(0.5f, 0.5f, 1f);
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
                        transform.localScale = new Vector3(-0.5f, 0.5f, 1f);
                    }
                    else if (dir.x>0f)
                    {
                        transform.localScale = new Vector3(0.5f, 0.5f, 1f);
                    }
                    transform.position += dir * speed * Time.deltaTime;
                    isMove = dir.magnitude != 0f;
                }
                
            }
            animator.SetBool("isMove", isMove);

        }
    }


}
   
