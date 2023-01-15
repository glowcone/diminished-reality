using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR;

public class ProjectionMatrix : MonoBehaviour
{
    // Start is called before the first frame update
    private XRDisplaySubsystem targetDisplay;
    [SerializeField] private Text txt;
    void Start()
    {
	    GetTargetDisplaySubsystem();
        GetWorldCameraMatrixFromDisplayProvider();
        txt.text += "hi";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private bool GetTargetDisplaySubsystem()
	{
		List<XRDisplaySubsystem> displays = new List<XRDisplaySubsystem>();
		SubsystemManager.GetInstances(displays);

		if (displays.Count == 0)
			throw new Exception("No XR display provider.");

		foreach (var d in displays)
		{
			if (d.running)
			{
				targetDisplay = d;
				return true;
			}
		}
		return false;
	}

	private bool GetWorldCameraMatrixFromDisplayProvider()
	{
		XRDisplaySubsystem.XRRenderPass renderPass;
		XRDisplaySubsystem.XRRenderParameter renderParameter;

		Matrix4x4 worldToCameraMatrixL, worldToCameraMatrixR;
		Matrix4x4 projectionMatrixL, projectionMatrixR;

		var display = targetDisplay;

		var passCount = display.GetRenderPassCount();
		if (passCount == 1)
		{
			// SinglePass
			display.GetRenderPass(0, out renderPass);
			int parameterCount = renderPass.GetRenderParameterCount();
			if (parameterCount != 2)
				Debug.LogError("weird");

			renderPass.GetRenderParameter(Camera.main, 0, out renderParameter);
			worldToCameraMatrixL = renderParameter.view;  // This is in Unity's convension
			projectionMatrixL = renderParameter.projection;

			renderPass.GetRenderParameter(Camera.main, 1, out renderParameter);
			worldToCameraMatrixR = renderParameter.view;  // This is in Unity's convension
			projectionMatrixR = renderParameter.projection;
		}
		else if (passCount == 2)
		{
			// MultiPass
			int parameterCount = 0;

			display.GetRenderPass(0, out renderPass);
			parameterCount = renderPass.GetRenderParameterCount();
			if (parameterCount != 1)
				Debug.LogError("weird");

			renderPass.GetRenderParameter(Camera.main, 0, out renderParameter);
			worldToCameraMatrixL = renderParameter.view;  // This is in Unity's convension
			projectionMatrixL = renderParameter.projection;

			display.GetRenderPass(1, out renderPass);
			parameterCount = renderPass.GetRenderParameterCount();
			if (parameterCount != 1)
				Debug.LogError("weird");

			renderPass.GetRenderParameter(Camera.main, 0, out renderParameter);
			worldToCameraMatrixR = renderParameter.view;  // This is in Unity's convension
			projectionMatrixR = renderParameter.projection;
		}
		else
		{
			// Stop all mirror camera
			passCount = 0;

			// If editor, need wait for several frame to have it.
			Debug.LogWarning("weird.   No RenderPass.");
			projectionMatrixL = projectionMatrixR = Camera.main.projectionMatrix;
			return false;
		}

        var debugMode = true;
		DebugMatrix("w2cL", worldToCameraMatrixL, debugMode);
		DebugMatrix("w2cR", worldToCameraMatrixR, debugMode);
		DebugMatrix("projL", projectionMatrixL, debugMode);
		DebugMatrix("projR", projectionMatrixR, debugMode);
		return true;
	}

	private void DebugMatrix(string name, Matrix4x4 m, bool show = true)
	{
		if (!show) return;
		StringBuilder sb = new StringBuilder(160);
		sb.AppendFormat("Matrix {0,-16}", name).AppendLine();
		sb.AppendFormat("/ {0:F6} {1:F6} {2:F6} {3:F6} \\", m.m00, m.m01, m.m02, m.m03).AppendLine();
		sb.AppendFormat("| {0:F6} {1:F6} {2:F6} {3:F6} |", m.m10, m.m11, m.m12, m.m13).AppendLine();
		sb.AppendFormat("| {0:F6} {1:F6} {2:F6} {3:F6} |", m.m20, m.m21, m.m22, m.m23).AppendLine();
		sb.AppendFormat("\\ {0:F6} {1:F6} {2:F6} {3:F6} /", m.m30, m.m31, m.m32, m.m33);
		txt.text += sb.ToString();
	}

}
