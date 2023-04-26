using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorCommander : MonoBehaviour
{
	[SerializeField] GameManager gameManager;

	Animator currentAnimator;
	string currentParameter;
	
	public void SetAnimator(string animatorName)
	{
		if (gameManager.animatorsByID.ContainsKey(animatorName))
		{
			currentAnimator = gameManager.animatorsByID[animatorName];
			Debug.Log($"Debug animator set to {animatorName}"); 
		}
		else
		{
			Debug.LogWarning($"No animator found with name: {animatorName}");
		}
	}

	public void SetTrigger(string triggerName)
	{
		Debug.Log($"Debug Animator triggering {triggerName} on {currentAnimator}");
		currentAnimator.SetTrigger(triggerName);
	}
	
	public void SetBoolTrue(string boolName)
	{
		Debug.Log($"Debug Animator setting {boolName} to true on {currentAnimator}");
		currentAnimator.SetBool(boolName, true);
	}

	public void SetBoolFalse(string boolName)
	{
		Debug.Log($"Debug Animator setting {boolName} to false on {currentAnimator}");
		currentAnimator.SetBool(boolName, false);
	}

	public void SetParameter(string parameterName)
	{
		currentParameter = parameterName;
	}
}
