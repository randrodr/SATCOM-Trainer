using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLookBoundary : MonoBehaviour
{
	[SerializeField] Cinemachine.CinemachineFreeLook freelookCam;
	[SerializeField] Vector2 boundaries;

	public float value;
	public Vector3 camPosition;
    
    void Start()
    {
		Debug.Log($"free look: {freelookCam}");
		freelookCam.m_XAxis.m_MinValue = boundaries.x;
		freelookCam.m_XAxis.m_MaxValue = boundaries.y;
		camPosition = freelookCam.transform.position;
    }

	private void LateUpdate()
	{
		camPosition = freelookCam.transform.position;

		if (camPosition.x > boundaries.x)
			camPosition = new Vector3(boundaries.x, camPosition.y, camPosition.z);

		freelookCam.transform.position = Vector3.up;
	}
}
