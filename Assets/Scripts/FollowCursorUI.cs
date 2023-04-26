using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class FollowCursorUI : MonoBehaviour
{
	RectTransform rectTransform;
	Vector2 mousePosition;

	private void Awake()
	{
		if(rectTransform == null)
		{
			rectTransform = GetComponent<RectTransform>();
		}
	}

	public void FollowCursor()
	{
		mousePosition = Mouse.current.position.ReadValue();

		rectTransform.anchoredPosition = mousePosition;
	}
}
