using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;

public class CustomInputModule : InputSystemUIInputModule
{
	[SerializeField] InputActionAsset exposedActionsAsset;
	CursorLockMode currentLockState = CursorLockMode.None;

	protected override void Awake()
	{
		actionsAsset = exposedActionsAsset;

		base.Awake();
	}

	// Disable cursor lock and THEN process, and return cursor lock to original state. This is a wrapper
	public override void Process()
	{
		currentLockState = Cursor.lockState;
		Cursor.lockState = CursorLockMode.None;
		base.Process();
		Cursor.lockState = currentLockState;
	}
}
