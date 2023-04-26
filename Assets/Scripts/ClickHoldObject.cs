using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickHoldObject : InteractableObject, IPointerDownHandler, IPointerUpHandler
{
	[SerializeField] float secondsToHold;

	float secondsHeld;
	bool counting;

	public float SecondsToHold { get => secondsToHold; }
	public float SecondsHeld { get => secondsHeld; }

	public override void OnPointerClick(PointerEventData eventData)
	{
		// empty override so base doesn't do anything
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		secondsHeld = 0f;
		counting = true;
		StartCoroutine(Timer());
	}
	
	public void OnPointerUp(PointerEventData eventData)
	{
		if (counting)
		{
			StopCoroutine(Timer());
			counting = false;
			Debug.Log(secondsHeld);

			if (secondsHeld > secondsToHold)
			{
				eventField.CurrentEvent = ThisTrigger;
				onClick.Raise();
			}
			else
			{
				Debug.Log($"Not held long enough ({secondsHeld} / {secondsToHold})");
			} 
		}
	}

	public override void OnPointerEnter(PointerEventData eventData)
	{
		base.OnPointerEnter(eventData);
		CursorManager.ClickHoldCursor();
	}

	public override void OnPointerExit(PointerEventData eventData)
	{
		OnPointerUp(eventData);

		base.OnPointerExit(eventData);
	}

	IEnumerator Timer()
	{
		while (counting)
		{
			secondsHeld += Time.deltaTime;
			yield return null; 
		}
	}
}
