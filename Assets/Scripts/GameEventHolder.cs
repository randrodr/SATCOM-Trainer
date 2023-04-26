using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/GameEvent Holder")]
public class GameEventHolder : ScriptableObject
{
	[SerializeField] private GameEvent currentEvent;

	public GameEvent CurrentEvent
	{
		get => currentEvent;
		set => currentEvent = value;
	}
}
