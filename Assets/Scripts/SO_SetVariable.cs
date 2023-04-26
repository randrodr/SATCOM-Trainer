using UnityEngine;
 
[CreateAssetMenu(menuName = "ScriptableObjects/SO_SetVariable")]
public class SO_SetVariable : ScriptableObject, ISerializationCallbackReceiver
{
	public SO_Set InitialValue;

	[System.NonSerialized]
	public SO_Set RuntimeValue;

	public void OnAfterDeserialize()
	{
		RuntimeValue = InitialValue;
	}

	public void OnBeforeSerialize()
	{
		
	}
}