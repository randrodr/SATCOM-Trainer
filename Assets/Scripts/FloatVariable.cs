using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Float Variable")]
public class FloatVariable : ScriptableObject, ISerializationCallbackReceiver
{
	public float InitialValue;

	[System.NonSerialized]
	public float RuntimeValue;

	public void OnAfterDeserialize()
	{
		RuntimeValue = InitialValue;
	}

	void OnEnable()
	{
		hideFlags = HideFlags.DontUnloadUnusedAsset; // <- keeps state during runtime in case it stops being referenced
	}

	public void OnBeforeSerialize() { }
}
