using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Scriptable Object Set")]
public class SO_Set : RuntimeSet<ScriptableObject>
{
	//public delegate void Del(ScriptableObject item);
	//public Del OnItemAdded = delegate { };

	public override void Add(ScriptableObject t)
	{
		base.Add(t);
		//OnItemAdded(t);
	}
}
