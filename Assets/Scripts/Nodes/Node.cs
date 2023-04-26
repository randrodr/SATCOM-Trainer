using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Base node
[System.Serializable]
public class Node
{
	public string _id;
	public int outs;

	public string title;
	public Content content;

	[HideInInspector]
	public string type;
	public NodeType nodeType;
	public string parent;

	// Added so we don't have to constantly reference edges array
	public Node[] nextNodes = new Node[0];
	public Node prevNode;

	public Node(Node baseNode)
	{
		_id = baseNode._id;
		outs = baseNode.outs;
		title = baseNode.title;
		content = baseNode.content;
		type = baseNode.type;
		nodeType = baseNode.nodeType;
		parent = baseNode.parent;
	}

	public virtual void Handle()
	{
		Debug.Log($"Handling of nodeType {type} not implemented; skipping to next node");
		nextNodes[0].Handle();
	}

	public virtual void Setup(GameManager gameManager)
	{
		//Debug.Log($"Setting up base node: {_id}");
	}
}
