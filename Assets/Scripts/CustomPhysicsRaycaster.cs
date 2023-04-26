using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class CustomPhysicsRaycaster : PhysicsRaycaster
{
	[Range(0.0f, 0.5f)]
	public float rayRadius;

	public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList)
	{
		if (Cursor.lockState != CursorLockMode.Locked)
		{
			base.Raycast(eventData, resultAppendList);
			return;
		}

		Ray ray = eventCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
		Debug.DrawRay(ray.origin, ray.direction, Color.red);

		var hits = Physics.RaycastAll(ray, eventCamera.farClipPlane, eventMask.value);
		for (int i = 0; i < hits.Length; i++)
		{
			var hit = hits[i];
			var result = new RaycastResult();
			result.distance = hit.distance;
			result.gameObject = hit.collider.gameObject;
			result.index = i;
			resultAppendList.Add(result);
		}
	}
}