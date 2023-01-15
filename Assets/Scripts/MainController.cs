using System;
using System.Collections;
using System.Collections.Generic;
using TextureSendReceive;
using UnityEngine;
using UnityEngine.UI;
using Wave.Essence;
using Wave.Essence.Samples.PassThrough;
using Wave.Native;

public class MainController : MonoBehaviour
{
    public TextureReceiver receiver;
    public RawImage image;
    public Billboard bill;
    private Texture2D texture;
    public PassThroughOverlayTest_Lisa lisa;
    private bool isCapturing = false;
    void Start()
    {
        Interop.WVR_ShowPassthroughOverlay(false);
        Interop.WVR_ShowProjectedPassthrough(false);
        var result = Interop.WVR_ShowPassthroughUnderlay(true);
        texture = new Texture2D(1, 1);
        bill.gameObject.SetActive(false);
        //setup 
        image.texture = texture;
        // image.material.mainTexture = texture;

        receiver.SetTargetTexture(texture);
        TextureReceiver.FrameReceived += TextureReceiverOnFrameReceived;
        // receiver
    }

    private void TextureReceiverOnFrameReceived()
    {
        print("frame captured");
        isCapturing = false;
        bill.gameObject.SetActive(true);
        bill.LockPos();
        if (lisa.planeInstance)
        {
            lisa.planeInstance.GetComponent<Renderer>().enabled = true;
            lisa.planeInstance.GetComponent<Renderer>().material.mainTexture = texture;
        }
    }

    public void ForceRetry()
    {
        isCapturing = false;
        StartCoroutine(RefreshImage());
    }

    private void Update()
    {
        if (WXRDevice.ButtonPress(WVR_DeviceType.WVR_DeviceType_Controller_Right, WVR_InputId.WVR_InputId_Alias1_A))
        {
            ForceRetry();
        }
    }

    // after placing plane
    public IEnumerator RefreshImage()
    {
        while (isCapturing)
        {
            yield return null;
        }
        // receiver sends capture command
        receiver.sendCaptureCommand();
        isCapturing = true;
        bill.gameObject.SetActive(false);
    }
}