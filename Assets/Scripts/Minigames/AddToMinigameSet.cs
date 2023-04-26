using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToMinigameSet : MonoBehaviour
{
	public GameEvent trigger;
	public MinigameDictionary set;

	private void Awake()
	{
		if (trigger)
		{
			Debug.Log($"Adding minigame trigger: {trigger.name}");
			set.Add(trigger, transform.GetChild(0).gameObject); 
		}
	}
}
