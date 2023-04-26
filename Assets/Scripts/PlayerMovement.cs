using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
	[SerializeField] GameplaySettings playerSettings;

	CharacterController characterController;
	PlayerInput input;
	Vector2 movementInput = new Vector2();	
	Vector3 moveDirection = new Vector3();

	private void Awake()
	{		
		// New Input System! We must instantiate in here, no inspector exposure
		input = new PlayerInput();
		characterController = GetComponent<CharacterController>();

		// Adding listeners to input events
		input.Player.Move.performed += context => movementInput = context.ReadValue<Vector2>();
		input.Player.Move.canceled += context => movementInput = Vector2.zero;
	}

	private void FixedUpdate()
	{
		moveDirection = (movementInput.x * transform.right) + (movementInput.y * transform.forward);
		characterController.SimpleMove(moveDirection * playerSettings.moveSpeed); // SimpleMove already multiplies by deltaTime
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
