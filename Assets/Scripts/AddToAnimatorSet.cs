using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToAnimatorSet : MonoBehaviour
{
	public AnimatorSet set;

    void Awake()
    {
		//Debug.Log($"adding animator on {gameObject.name}");
		set.Add(GetComponent<Animator>()); 
    }

	private void OnEnable()
	{
		//set.Add(GetComponent<Animator>());
	}
}
