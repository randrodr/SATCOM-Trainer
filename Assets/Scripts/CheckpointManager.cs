using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
	[SerializeField] AnimatorSet animatorsInGame;
	[SerializeField] string[] checkpointParameters;

	public void GoToCheckpoint(int index)
	{
		if (index > -1)
		{
			Debug.Log($"Setting up Checkpoint: {checkpointParameters[index]}");
			foreach (Animator animator in animatorsInGame.Items)
			{
				Debug.Log($"{checkpointParameters[index]} on {animator}");
				animator.SetTrigger(checkpointParameters[index]);
			} 
		}
	}

	//[System.Serializable]
	//public class AnimatorCommand
	//{
	//	[SerializeField] Animator animator;
	//	//[SerializeField] AnimatorControllerParameter.
	//	[SerializeField] StateChange[] parameters;

	//	public void SetAllParameters()
	//	{
	//		for (int i = 0; i < parameters.Length; i++)
	//		{
	//			if (System.Enum.TryParse(parameters[i].type, true, out ParameterType paramType))
	//			{
	//				Debug.Log($"Changing state of {animator.name}'s {parameters[i].name} to {parameters[i].value}");

	//				switch (paramType)
	//				{
	//					case ParameterType.Float:
	//						if (float.TryParse(parameters[i].value, out float newFloat))
	//							animator.SetFloat(parameters[i].name, newFloat);
	//						break;
	//					case ParameterType.Int:
	//						if (int.TryParse(parameters[i].value, out int newInt))
	//							animator.SetInteger(parameters[i].name, newInt);
	//						break;
	//					case ParameterType.Bool:
	//						if (bool.TryParse(parameters[i].value, out bool newBool))
	//							animator.SetBool(parameters[i].name, newBool);
	//						break;
	//					case ParameterType.Trigger:
	//						animator.SetTrigger(parameters[i].name);
	//						break;
	//					default:
	//						break;
	//				}
	//			}
	//			else
	//			{
	//				Debug.LogWarning($"Could not parse {parameters[i].type} type, skipping");
	//			}
	//		}
	//	}
	//}

	//[System.Serializable]
	//public class Checkpoint
	//{
	//	[SerializeField] AnimatorCommand[] animatorCommands;

	//	public void SetAllAnimators()
	//	{
	//		for (int i = 0; i < animatorCommands.Length; i++)
	//		{
	//			animatorCommands[i].SetAllParameters();
	//		}
	//	}
	//}

	//[SerializeField] Checkpoint[] checkpoints;

	//[ContextMenu("Test checkpoint")]
	//public void GoToCheckpoint(int index)
	//{
	//	checkpoints[index].SetAllAnimators();
	//}
}
