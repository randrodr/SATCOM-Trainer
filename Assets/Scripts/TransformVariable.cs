using UnityEngine;
 
[CreateAssetMenu(fileName = "TransformVariable", menuName = "ScriptableObjects/Transform Variable")]
public class TransformVariable : ScriptableObject, ISerializationCallbackReceiver
{
	public Transform InitialValue;

	[System.NonSerialized]
	public Transform RuntimeValue;

	public void OnAfterDeserialize()
	{
		if (InitialValue)
		{
			RuntimeValue = InitialValue; 
		}
	}

	public void OnBeforeSerialize() { }
}