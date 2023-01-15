using System.Collections;
using System.Collections.Generic;
using TextureSendReceive;
using UnityEngine;
using UnityEngine.UI;
using Wave.Native;

public class MainController : MonoBehaviour
{
    public TextureReceiver receiver;
    public RawImage image;
    public Billboard bill;
	
    private bool isCapturing = false;
    void Start()
    {
        Interop.WVR_ShowPassthroughOverlay(false);
        Interop.WVR_ShowProjectedPassthrough(true);
        var result = Interop.WVR_ShowPassthroughUnderlay(true);
        bill.gameObject.SetActive(false);
        //setup 
        Texture2D texture;
        texture = new Texture2D(1, 1);
        image.texture = texture;

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
    }

    public void ForceRetry()
    {
        isCapturing = false;
        StartCoroutine(RefreshImage());
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