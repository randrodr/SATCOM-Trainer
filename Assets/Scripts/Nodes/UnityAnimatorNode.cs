using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class UnityAnimatorNode : Node
{
	GameManager gameManager;
	Animator[] stateAnimators;
		
	public UnityAnimatorNode(Node baseNode) : base(baseNode)
	{}

	public override void Handle()
	{
		PreHandleSetup();

		Debug.Log($"new node of type {nodeType}: {_id}");
		for (int i = 0; i < stateAnimators.Length; i++)
		{
			SetParameter(stateAnimators[i], content.items[i].update.name, content.items[i].update.value, content.items[i].update.type);
		}

		if (nextNodes.Length > 0)
		{
			nextNodes[0].Handle(); 
		}
	}

	// static so we can call it from the gamemanager
	public static void SetParameter(Animator animator, string parameterName, string valueAsString, string typeAsString)
	{
		if (Enum.TryParse(typeAsString, true, out ParameterType paramType))
		{
			Debug.Log($"Changing state of {animator}'s {parameterName} to {valueAsString}");

			switch (paramType)
			{
				case ParameterType.Float:
					if (float.TryParse(valueAsString, out float newFloat))
						animator.SetFloat(parameterName, newFloat);
					break;
				case ParameterType.Int:
					if (int.TryParse(valueAsString, out int newInt))
						animator.SetInteger(parameterName, newInt);
					break;
				case ParameterType.Bool:
					if (bool.TryParse(valueAsString, out bool newBool))
						animator.SetBool(parameterName, newBool);
					break;
				case ParameterType.Trigger:
					animator.SetTrigger(parameterName);
					break;
				default:
					break;
			}
		}
		else
		{
			Debug.LogWarning($"Could not parse {typeAsString} type, skipping");
		}
	}

	void PreHandleSetup()
	{}

	public override void Setup(GameManager newGameManager)
	{
		gameManager = newGameManager;

		stateAnimators = new Animator[content.items.Length];

		for (int i = 0; i < stateAnimators.Length; i++)
		{
			if (gameManager.animatorsByID.ContainsKey(content.items[i].name))
			{
				///Debug.Log($"Animator {content.items[i].name} found");
				stateAnimators[i] = gameManager.animatorsByID[content.items[i].name];
			}
			else
			{
				Debug.LogWarning($"Animator for {_id} not found (name: {content.items[i].name})");
			}
		}
	}
}
