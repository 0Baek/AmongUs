using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingCrew : MonoBehaviour
{
    public EPlayerColor playerColor;


    private SpriteRenderer spriteRenderer;
    private Vector3 direction;
    private float floatingSpeed;
    private float rotateSpeed;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    public void SetFloatingCrew(Sprite sprite,EPlayerColor playercolor,Vector3 drirection,float floatingSpeed,float rotateSpeed,float size)
    {
        this.playerColor = playercolor;
        this.direction = drirection;
        this.floatingSpeed = floatingSpeed;
        this.rotateSpeed = rotateSpeed;

        spriteRenderer.sprite = sprite;
        spriteRenderer.material.SetColor("_PlayerColor", PlayerColor.GetColor(playercolor));

        transform.localScale = new Vector3(size*2, size*2, size*2);
        spriteRenderer.sortingOrder = (int)Mathf.Lerp(1, 32767, size);

    }
    void Update()
    {
        transform.position += direction * floatingSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0f, 0f, rotateSpeed));
    }



}
