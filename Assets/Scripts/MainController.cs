using System.Collections;
using System.Collections.Generic;
using TextureSendReceive;
using UnityEngine;
using UnityEngine.UI;

public class MainController : MonoBehaviour
{
	public TextureReceiver receiver;
	public RawImage image;
	
	private bool isCapturing = false;
    void Start()
    {
        //setup 
		Texture2D texture;
		receiver = GetComponent<TextureReceiver>();
		texture = new Texture2D(1, 1);
		image.texture = texture;

		receiver.SetTargetTexture(texture);
		TextureReceiver.FrameReceived += TextureReceiverOnFrameReceived;
	    // receiver
    }

    private void TextureReceiverOnFrameReceived()
    {
	    isCapturing = false;
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
    }
}
