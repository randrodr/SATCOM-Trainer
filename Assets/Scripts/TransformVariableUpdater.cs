using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformVariableUpdater : MonoBehaviour
{
	[SerializeField] TransformVariable transformVariable;
	[SerializeField] bool continuous = true;

    void Update()
    {
		if (continuous)
		{
			UpdateVariable(); 
		}
    }

	public void UpdateVariable()
	{
		if (transformVariable)
		{
			transformVariable.RuntimeValue = transform;
		}
	}
}
