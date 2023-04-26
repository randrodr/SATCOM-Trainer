using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class EndNode : Node
{
	[DllImport("__Internal")] private static extern void EndReached();

	public EndNode(Node baseNode) : base(baseNode)
	{}

	public override void Handle()
	{		
		Debug.Log("End node reached");
		EndReached();
	}
}
