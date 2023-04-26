// This is for stopping the freelook when the mouse button isn't being clicked

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class VirtualCamHoldEnable : MonoBehaviour
{
	PlayerInput input;
	[SerializeField] Cinemachine.CinemachineVirtualCameraBase cinemachineCam;
	[SerializeField] Cinemachine.CinemachineInputProvider inputProvider;
	InputActionReference inputReference;


	private void OnEnable()
	{
		input.Enable();
	}

	void Awake()
    {
		if (inputProvider == null)
			inputProvider = GetComponent<Cinemachine.CinemachineInputProvider>();

		input = new PlayerInput();
		input.Player.Interact.started += context => EnableCam(true);
		input.Player.Interact.canceled += context => EnableCam(false);

		// store the input provider XY Axis for later, and set reference to null
		inputReference = inputProvider.XYAxis;
		inputProvider.XYAxis = null;
	}

	void EnableCam(bool value)
	{
		inputProvider.XYAxis = value ? inputReference : null;
	}

	private void OnDisable()
	{
		input.Disable();
	}
}
