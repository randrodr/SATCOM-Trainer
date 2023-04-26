using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class CameraZoom : MonoBehaviour
{
	[SerializeField] PlayerInput input;
	[SerializeField] GameplaySettings gameplaySettings;
	[SerializeField] Vector2 FOV_Extents;

	private CinemachineFreeLook flCam;
	private CinemachineVirtualCamera vCam;

	private void Awake()
	{
		if (flCam == null)
			flCam = GetComponent<CinemachineFreeLook>();
		if (vCam == null)
			vCam = GetComponent<CinemachineVirtualCamera>();

		input = new PlayerInput();
		//input.Player.Zoom.performed += context => scrollInput = context.ReadValue<Vector2>();
		input.Player.Zoom.performed += context => { ChangeFOV(context.ReadValue<Vector2>().y); };
		
	}

	void ChangeFOV(float scrollInput)
	{
		if (flCam)
		{
			if (flCam.ParentCamera.IsLiveChild(flCam))
			{
				flCam.m_Lens.FieldOfView += gameplaySettings.zoomSensitivity * scrollInput / Mathf.Abs(scrollInput) * -1;
				flCam.m_Lens.FieldOfView = Mathf.Clamp(flCam.m_Lens.FieldOfView, FOV_Extents.x, FOV_Extents.y); 
			}
		}
		if (vCam)
		{
			if (vCam.ParentCamera.IsLiveChild(vCam))
			{
				vCam.m_Lens.FieldOfView += gameplaySettings.zoomSensitivity * scrollInput / Mathf.Abs(scrollInput) * -1;
				vCam.m_Lens.FieldOfView = Mathf.Clamp(vCam.m_Lens.FieldOfView, FOV_Extents.x, FOV_Extents.y); 
			}
		}
	}

	private void OnEnable()
	{
		input.Enable();
	}

	private void OnDisable()
	{
		input.Disable();
	}
}
