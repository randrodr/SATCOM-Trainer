using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFillListener : MonoBehaviour
{
	public ClickHoldObject clickHoldObject;

	float maxTime;
	Image image;

	private void Awake()
	{
		if(image == null)
		{
			image = GetComponent<Image>();
		}
		maxTime = clickHoldObject.SecondsToHold;

		Debug.Log($"max time is {maxTime}");
	}

	private void LateUpdate()
	{
		image.fillAmount = Mathf.InverseLerp(0f, maxTime, clickHoldObject.SecondsHeld);
	}
}
