using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUIManager : MonoBehaviour
{
    public static InGameUIManager Instance;

    [SerializeField]
    private InGameIntroUI ingameintroUI;
    public InGameIntroUI IngameintroUI
    {
        get
        {
            return ingameintroUI;
        }
    }


    private void Awake()
    {
        Instance = this;
    }
}
