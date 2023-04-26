using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EventObjectPair 
{
	public GameEvent gameEvent;
	public GameObject gameObject;

	public EventObjectPair(GameEvent gameEvent, GameObject gameObject)
	{
		this.gameEvent = gameEvent;
		this.gameObject = gameObject;
	}
}
