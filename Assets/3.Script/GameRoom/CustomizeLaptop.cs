using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomizeLaptop : MonoBehaviour
{
    [SerializeField] private Sprite useBtnSprite;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        var inst = Instantiate(spriteRenderer.material);
        spriteRenderer.material = inst;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<CharacterMover>();
        if (character!=null&&character.isOwned)
        {
            spriteRenderer.material.SetFloat("_Highlighted", 1f);
            LobbyUIManager.instance.SetUseButton(useBtnSprite, OnClickUse);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent<CharacterMover>();
        if (character != null && character.isOwned)
        {
            spriteRenderer.material.SetFloat("_Highlighted", 0f);
            LobbyUIManager.instance.UnsetUseButton();
        }
    }
    public void OnClickUse()
    {
        Debug.Log("들어왔나");
        LobbyUIManager.instance.CustumizeUI.Open();
    }
}
