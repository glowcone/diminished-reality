// "Wave SDK 
// © 2020 HTC Corporation. All Rights Reserved.
//
// Unless otherwise required by copyright law and practice,
// upon the execution of HTC SDK license agreement,
// HTC grants you access to and use of the Wave SDK(s).
// You shall fully comply with all of HTC’s SDK license agreement terms and
// conditions signed by you and all SDK and API requirements,
// specifications, and documentation provided by HTC to You."

using UnityEngine;
using Wave.Native;

namespace Wave.Essence.Samples.PassThrough
{
	public class PassThroughOverlayTest_Lisa : MonoBehaviour
	{
		private static string LOG_TAG = "Wave.Essence.Samples.PassThrough.PassThroughOverlayTest";

		private bool passThroughOverlayFlag = false;
		private bool showPassThroughOverlay = false;
		bool delaySubmit = false;
		bool showIndicator = false;
		float alpha = 1.0f;
		float alpha2 = 1.0f;
		int steps = 0;

		public Transform leftHandTransform;
		public GameObject planePrefab;
		public GameObject planeInScene;
		public GameObject planeInstance;
		public bool planeSpawned;
		public ParticleSystem smokeShow;

		// Start is called before the first frame update
		void Start()
		{
			Log.i(LOG_TAG, "PassThroughOverlay start: " + passThroughOverlayFlag);
			Interop.WVR_ShowPassthroughUnderlay(true);
			//Interop.WVR_ShowProjectedPassthrough(false);
			Log.i(LOG_TAG, "ShowPassThroughOverlay start: " + showPassThroughOverlay);

		}

		// Update is called once per frame
		void Update()
		{
			if (WXRDevice.ButtonPress(WVR_DeviceType.WVR_DeviceType_Controller_Right, WVR_InputId.WVR_InputId_Alias1_A))
			{
				//START EXPERIENCE
				//smokeShow = planeInstance.GetComponentInChildren(typeof (ParticleSystem));
				smokeShow.Play();
			}
			else if (WXRDevice.ButtonPress(WVR_DeviceType.WVR_DeviceType_Controller_Left, WVR_InputId.WVR_InputId_Alias1_X))
			{
                if (planeInstance == null)
                {
					planeInstance = Instantiate(planePrefab, leftHandTransform.position, Quaternion.identity);
					//planeInScene.transform.position = leftHandTransform.position;
				} else 
				{
					planeInstance.transform.position = leftHandTransform.position;
                }

			}
			else if (WXRDevice.ButtonPress(WVR_DeviceType.WVR_DeviceType_Controller_Left, WVR_InputId.WVR_InputId_Alias1_Y))
			{

			}
		}

	private void OnApplicationPause()
		{
		}

		private void OnApplicationQuit()
		{
		}
	}
}
