using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class OplogNode : Node
{
	[DllImport("__Internal")] private static extern void SendNode(string nodeID);

	GameEvent onSoftwareStart;

	public OplogNode(Node baseNode) : base(baseNode)
	{
	}

	public override void Handle()
	{
		Debug.Log("Swapping over to software");
		onSoftwareStart.Raise();
		SendNode(_id);
	}

	public override void Setup(GameManager gameManager)
	{
		onSoftwareStart = gameManager.OnSoftwareStart;
	}
}
