using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConditionNode : Node
{
	DifficultyLevel difficulty;

	public ConditionNode(Node baseNode) : base(baseNode) {}

	public override void Handle()
	{
		Debug.Log($"new node of type {nodeType}: {_id}");
		if ((int)difficulty < nextNodes.Length)
		{
			nextNodes[(int)difficulty].Handle(); 
		}
		else
		{
			nextNodes[0].Handle();
		}
	}

	public override void Setup(GameManager gameManager)
	{
		difficulty = gameManager.DifficultyLevel;
	}
}
