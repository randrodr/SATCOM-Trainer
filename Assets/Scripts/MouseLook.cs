using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{
	[SerializeField] Transform pitchTarget;
	[SerializeField] Transform yawTarget;
	[SerializeField] GameplaySettings playerSettings;
	[SerializeField] bool lockCursor;
	[SerializeField] InputActionReference actionReference;

	PlayerInput input;
	Vector2 lookInput = new Vector2();
	int invertFactor;
	float pitch = 0f;
	float yaw = 0f;	

	private void Awake()
	{
		if (!pitchTarget)
			pitchTarget = transform;
		if (!yawTarget)
			yawTarget = transform;
		if (!playerSettings)
			playerSettings = new GameplaySettings();

		input = new PlayerInput();
		input.Player.Look.performed += context => lookInput = context.ReadValue<Vector2>();
		input.Player.Look.canceled += context => lookInput = Vector2.zero;

		CursorLock(lockCursor);
	}

	void LateUpdate()
    {
		invertFactor = (playerSettings.invertY) ? -1 : 1; 

		pitch = lookInput.y * invertFactor * playerSettings.mouseSensitivity * Time.deltaTime;
		yaw = -lookInput.x * playerSettings.mouseSensitivity * Time.deltaTime;

		pitchTarget.Rotate(transform.right, pitch, Space.World);
		yawTarget.Rotate(transform.up, yaw, Space.World);
	}

	public void CursorLock(bool value)
	{
		Cursor.lockState = value == true ? CursorLockMode.Locked : CursorLockMode.None;
		Cursor.visible = !value;
	}

	private void OnEnable()
	{
		input.Enable();
		//CursorLock(true);
	}

	private void OnDisable()
	{
		input.Disable();
		//CursorLock(false);
	}
}
