using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameObject Set")]
public class GO_Set : RuntimeSet<GameObject>, ISerializationCallbackReceiver
{
	//public delegate void Del(GameObject item);
	//public Del OnItemAdded = delegate { };

	public override void Add(GameObject t)
	{
		base.Add(t);
		//OnItemAdded(t);
	}

	public void OnAfterDeserialize()
	{
		Items.Clear();
	}

	public void OnBeforeSerialize()
	{

	}
}
