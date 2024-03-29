using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightWire : MonoBehaviour
{
    public EWireColor WireColor { get; private set; }
    public bool IsConnected { get; private set; }

    [SerializeField]
    private List<Image> mWireImages;

    [SerializeField]
    private Image mLightImage;

    [SerializeField]
    private List<LeftWire> mConnectedWires = new List<LeftWire>();

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
    public void ConnectWire(LeftWire leftWire)
    {
        if (mConnectedWires.Contains(leftWire))
        {
            return;
        }
        mConnectedWires.Add(leftWire);
        if (mConnectedWires.Count ==1&&leftWire.WireColor ==WireColor)
        {
            mLightImage.color = new Color(0f, 255f, 185f, 255f);
            IsConnected = true;
        }
        else
        {
            mLightImage.color = Color.gray;
            IsConnected = false;
        }
    }
    public void DisconnectWire(LeftWire leftWire)
    {
        mConnectedWires.Remove(leftWire);
        if (mConnectedWires.Count==1&&mConnectedWires[0].WireColor==WireColor)
        {
            mLightImage.color = new Color(0f, 255f, 185f, 255f);
            IsConnected = true;
        }
        else
        {
            mLightImage.color = Color.gray;
            IsConnected = false;
        }
    }

}
