using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class EventOnEnable : MonoBehaviour
{
	public UnityEvent onEnable;
	public UnityEvent onDisable;

	private void OnEnable()
	{
		onEnable.Invoke();
	}

	private void OnDisable()
	{
		onDisable.Invoke();
	}
}
