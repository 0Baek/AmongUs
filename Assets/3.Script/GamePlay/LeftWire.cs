using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftWire : MonoBehaviour
{
    public EWireColor WireColor { get; private set; }

    public bool IsConnected { get; private set; }

    [SerializeField]
    private List<Image> mWireImages;
    [SerializeField]
    private RectTransform mWireBody;


    [SerializeField]
    private Image mLightImage;

    [SerializeField]
    private RightWire mConnectedWire;




    [SerializeField] private float offset = 15f;

    private Canvas mGameCanvas;

    // Start is called before the first frame update
    void Start()
    {
        mGameCanvas = FindObjectOfType<Canvas>();
    }

    // Update is called once per frame

    public void SetTarget(Vector3 targetPosition, float offset)
    {
        float angle = Vector2.SignedAngle(transform.position + Vector3.right - transform.position,
             targetPosition - transform.position);
        float distance = Vector2.Distance(mWireBody.transform.position, Input.mousePosition) - offset;
        mWireBody.localRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        mWireBody.sizeDelta = new Vector2(distance, mWireBody.sizeDelta.y);
       
    }
    public void ResetTarget()
    {
        mWireBody.localRotation = Quaternion.Euler(Vector3.zero);
        mWireBody.sizeDelta = new Vector2(0f, mWireBody.sizeDelta.y);
    }
    public void SetWireColor(EWireColor wireColor)
    {
        WireColor = wireColor;
        Color color = Color.black;
        switch (wireColor)
        {
          
            case EWireColor.Red:
                color = Color.red;
                break;
            case EWireColor.Blue:
                color = Color.blue;
                break;
            case EWireColor.Yellow:
                color = Color.yellow;
                break;
            case EWireColor.Magenta:
                color = Color.magenta;
                break;     
        }
        foreach (var image in mWireImages)
        {
            image.color = color;
        }
    }
    public void ConnectWire(RightWire rightwire)
    {
        if (mConnectedWire != null &&mConnectedWire !=rightwire)
        {
            mConnectedWire.DisconnectWire(this);
            mConnectedWire = null;
        }
        mConnectedWire = rightwire;
        if (mConnectedWire.WireColor==WireColor)
        {
            mLightImage.color = new Color(0f,255f,185f,255f);
            IsConnected = true;
        }
    }
    public void DisconnectWire()
    {
        if (mConnectedWire != null)
        {
            mConnectedWire.DisconnectWire(this);
            mConnectedWire = null;
        }
        mLightImage.color = Color.gray;
        IsConnected = false;
    }
}
