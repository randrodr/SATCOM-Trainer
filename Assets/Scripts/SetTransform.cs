using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTransform : MonoBehaviour
{
	[SerializeField] TransformVariable target;
	[SerializeField] bool affectPosition;
	[SerializeField] bool affectRotation;

	private void FixedUpdate()
	{
		if (target.RuntimeValue)
		{
			if (affectPosition)
			{
				transform.position = target.RuntimeValue.position;
			}
			if (affectRotation)
			{
				transform.rotation = target.RuntimeValue.rotation;
			} 
		}
	}
}
