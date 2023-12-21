using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField]
    private Button MouseControlBtn;
    [SerializeField]
    private Button KeyboardMouseControlBtn;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();    
    }
    private void OnEnable()
    {
        switch (PlayerSettings.countrolType)
        {
            case ECountrolType.Mouse:
                MouseControlBtn.image.color = Color.green;
                KeyboardMouseControlBtn.image.color = Color.white;
                break;
            case ECountrolType.KeyboardMouse:
                MouseControlBtn.image.color = Color.white;
                KeyboardMouseControlBtn.image.color = Color.green;
                break;
          
        }
    }
    public  void SetControlMode(int controlType)
    {
        PlayerSettings.countrolType = (ECountrolType)controlType;

        switch (PlayerSettings.countrolType)
        {
            case ECountrolType.Mouse:
                MouseControlBtn.image.color = Color.green;
                KeyboardMouseControlBtn.image.color = Color.white;
                break;
            case ECountrolType.KeyboardMouse:
                MouseControlBtn.image.color = Color.white;
                KeyboardMouseControlBtn.image.color = Color.green;
                break;

        }
    }
    public virtual void Close()
    {
        StartCoroutine(CloseAfterDelay());
    }
    private IEnumerator CloseAfterDelay()
    {
        animator.SetTrigger("close");
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
