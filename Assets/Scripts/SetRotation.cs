using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRotation : MonoBehaviour
{
	public enum Axis
	{
		x, y, z
	}
	[SerializeField] SnapAxis oldAxis;
	[SerializeField] Axis axis;
	[SerializeField] float angle;
	[SerializeField] TransformVariable lookAtTransform;
	Vector3 turnAround = new Vector3(0, 180, 0);

	public void RotateTo(Axis thisAxis, float value)
	{
		//Debug.Log($"rotating from {transform.rotation}");
		switch(thisAxis)
		{
			case Axis.x:
				transform.rotation = Quaternion.Euler(
					value, 
					transform.rotation.eulerAngles.y + transform.localRotation.eulerAngles.y, 
					transform.rotation.eulerAngles.z + transform.localRotation.eulerAngles.z);
				//Debug.Log($" to {transform.rotation} via {value} on the {thisAxis} axis");
				break;
			case Axis.y:
				transform.rotation = Quaternion.Euler(
					transform.rotation.eulerAngles.x + transform.localRotation.x, 
					value, 
					transform.rotation.z + transform.localRotation.z);
				//Debug.Log($" to {transform.rotation} via {value} on the {thisAxis} axis");
				break;
			case Axis.z:
				transform.rotation = Quaternion.Euler(
					transform.rotation.x + transform.localRotation.x, 
					transform.rotation.y + transform.localRotation.y, 
					value);
				//Debug.Log($" to {transform.rotation} via {value} on the {thisAxis} axis");
				break;
			default:
				break;
		}
		//Debug.Log($" to {transform.rotation.eulerAngles} via {value} on the {thisAxis} axis");
	}

	public void DoTheRotation()
	{
		RotateTo(axis, angle);
	}

	void LookAtTransform(TransformVariable transformVariable)
	{
		if (transformVariable.RuntimeValue)
		{
			transform.rotation = transformVariable.RuntimeValue.rotation * Quaternion.Euler(turnAround); 
		}
	}

	private void Update()
	{
		if (lookAtTransform)
		{
			LookAtTransform(lookAtTransform); 
		}
	}
}
