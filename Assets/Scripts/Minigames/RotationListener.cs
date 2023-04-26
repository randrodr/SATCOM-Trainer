using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationListener : MonoBehaviour
{
	public FloatVariable rotationValue;
	public Transform rotateTarget;

	private void OnEnable()
	{
		Rotate();
	}

	public void Rotate()
	{
		rotateTarget.localRotation = Quaternion.Euler(
			rotateTarget.localRotation.eulerAngles.x,
			rotationValue.RuntimeValue,
			rotateTarget.localRotation.eulerAngles.z);
	}
}
