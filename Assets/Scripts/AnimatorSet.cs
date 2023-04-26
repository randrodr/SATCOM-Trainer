using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Animator Set")]
public class AnimatorSet : RuntimeSet<Animator>, ISerializationCallbackReceiver
{
	//public delegate void Del(Animator item);
	//public Del OnItemAdded = delegate { };

	public override void Add(Animator t)
	{
		//Debug.Log($"adding {t.name}");
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
