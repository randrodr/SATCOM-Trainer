using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/String Variable")]
public class StringVariable : ScriptableObject, ISerializationCallbackReceiver
{
	public string InitialValue;

	[System.NonSerialized]
	public string RuntimeValue;

	public void OnAfterDeserialize()
	{
		RuntimeValue = InitialValue;
	}

	public void OnBeforeSerialize() { }
}
