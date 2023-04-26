using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AnimatorSendMessage : MonoBehaviour
{
	[DllImport("__Internal")] private static extern void ResetCameraButton();

	Animator animator;

	private void Awake()
	{
		if(animator == null)
		{
			animator = GetComponent<Animator>();
		}
	}

	public void SetTrigger(string triggerName)
	{
		animator.SetTrigger(triggerName);
	}

	public void EnableResetCameraButton()
	{
#if !UNITY_EDITOR
		ResetCameraButton(); 
#endif
	}
}
