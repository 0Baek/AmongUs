using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FixWiringTaskObject : MonoBehaviour
{
    [SerializeField]
    protected Sprite _UseButtonSprite;

    protected SpriteRenderer _spriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _spriteRenderer.material = Instantiate(_spriteRenderer.material);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        var character = collision.GetComponent<InGameCharacterMover>();
        if (character != null&&character.isOwned)
        {
            _spriteRenderer.material.SetFloat("_Highlighted", 1f);
            InGameUIManager.Instance.SetUseButton(_UseButtonSprite, OnClickUse);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        var character = collision.GetComponent<InGameCharacterMover>();
        if (character != null && character.isOwned)
        {
            _spriteRenderer.material.SetFloat("_Highlighted", 0f);
            InGameUIManager.Instance.UnSetUseButton();
        }
    }
    public void OnClickUse()
    {
        InGameUIManager.Instance.FIxWiringTask.Open();
    }

}
