using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wave.Native;

public class PassthroughSwitch : MonoBehaviour
{
	private static string LOG_TAG = "Wave.Essence.Samples.PassThrough.PassThroughOverlayTest";

	private bool passThroughOverlayFlag = false;
	private bool showPassThroughOverlay = false;
	bool delaySubmit = false;
	bool showIndicator = false;
	float alpha = 1.0f;
	float alpha2 = 1.0f;
	int steps = 0;
	// Start is called before the first frame update

	void Start()
	{
		Log.i(LOG_TAG, "PassThroughOverlay start: " + passThroughOverlayFlag);
		showPassThroughOverlay = Interop.WVR_ShowPassthroughOverlay(passThroughOverlayFlag);
		Interop.WVR_ShowProjectedPassthrough(false);
		Log.i(LOG_TAG, "ShowPassThroughOverlay start: " + showPassThroughOverlay);
	}

	public void PassthruON()
	{
		{
			bool visible = !Interop.WVR_IsPassthroughOverlayVisible();
			if (visible)
			{
				if (steps == 0)
				{
					delaySubmit = false;
					showIndicator = false;
				}
				else if (steps == 1)
				{
					delaySubmit = true;
					showIndicator = false;
				}
				else if (steps == 2)
				{
					delaySubmit = false;
					showIndicator = true;
				}
				else if (steps == 3)
				{
					delaySubmit = true;
					showIndicator = true;
				}
				Interop.WVR_ShowPassthroughOverlay(visible, delaySubmit, showIndicator);
				Log.i(LOG_TAG, "WVR_ShowPassthroughOverlay: visible:" + visible + " ,delaySubmit: " + delaySubmit + " ,showIndicator: " + showIndicator);
				alpha = 1.0f;
				Interop.WVR_SetPassthroughOverlayAlpha(alpha);
			}
		}
	}
}
