using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateDrivenTransform : MonoBehaviour
{
	public List<SO_Pair> statePairs;
	public int entryState;

	public int testInt;

	private void Awake()
	{
		UpdateState(entryState);
	}

	[ContextMenu("test states")]
	public void UpdateStateTest()
	{
		UpdateState(testInt);
	}

	public void UpdateState(int index)
	{
		if (index < statePairs.Count)
		{
			TransformVariable tempTransform = (TransformVariable)statePairs[index].value;

			if (tempTransform.RuntimeValue)
			{
				transform.position = tempTransform.RuntimeValue.position;
				Debug.Log($"changing position to {tempTransform.RuntimeValue.parent.name}");
			}
		}
		else
		{
			Debug.LogWarning($"No state-pair at index {index}");
		}
	}
}
