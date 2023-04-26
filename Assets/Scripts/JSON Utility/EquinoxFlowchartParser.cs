using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;

public class EquinoxFlowchartParser : MonoBehaviour
{
	// Test field
#if UNITY_EDITOR
	[Header("JSON file for in-editor testing only")]
	public TextAsset jsonFile;
#endif

	public List<Node> nodesList;
	public List<Edge> edgesList;

	public Dictionary<string, Node> nodesByID = new Dictionary<string, Node>();
	public Dictionary<string, bool> variablesByName = new Dictionary<string, bool>();

	void Start()
    {
		//ReadJson();
    }

	public void ReadJson(string data)
	{
		Debug.Log(" - Parser parsing");
		EquinoxJSON eqJson = JsonUtility.FromJson<EquinoxJSON>(data);
		Debug.Log(" - Making lists for nodes/edges");
		nodesList = eqJson.nodes.ToList();
		edgesList = eqJson.edges.ToList();


		// Create nodes dictionary first
		foreach(Node node in nodesList)
		{
			//SetType(node);
			if(!nodesByID.ContainsKey(node._id))
				nodesByID.Add(node._id, SetType(node));

			// Process Variables
			if (node.nodeType == NodeType.Variable)
			{
				string variableName = ExtractFromBrackets(node.content.value);
				if(!variablesByName.ContainsKey(variableName))
				{
					variablesByName.Add(variableName, false);
				}
			}
		}
		Debug.Log(" - nodes dict done, setting edges");
		
		// Then handle edges (node connections)
		SetConnections();
	}

	public void ReadJson()
	{
		Debug.LogWarning("Test function, this shouldn't be called in build");
#if UNITY_EDITOR
		ReadJson(jsonFile.text); 
#endif
	}

	public Node SetType(Node eqNode)
	{
		if (eqNode.type[0].Equals('$')) // remove '$' from node type
		{
			eqNode.type = eqNode.type.Remove(0, 1);
		}

		if (System.Enum.TryParse(eqNode.type, out NodeType nodeType))
		{
			eqNode.nodeType = nodeType;
		}
		else
		{
			eqNode.nodeType = NodeType.none;
		}

		switch (eqNode.nodeType)
		{
			case NodeType.Start:
				break;
			case NodeType.UnityTrigger:
				return new UnityTriggerNode(eqNode);
			case NodeType.UnityAnimator:
				return new UnityAnimatorNode(eqNode);
			case NodeType.Conversation:
				return new ConversationNode(eqNode);
			case NodeType.Condition:
				return new ConditionNode(eqNode);
			case NodeType.End:
				return new EndNode(eqNode);
			case NodeType.OPLOG:
				return new OplogNode(eqNode);
			case NodeType.Variable:
				return null;
			case NodeType.Transition:
				return new TransitionNode(eqNode);
			default:
				return new Node(eqNode);
		}
		return eqNode;
	}


	public void SetConnections()
	{
		foreach(Edge edge in edgesList)
		{
			if (edge._id != null) // If our edge has an id,
			{
				if (nodesByID.ContainsKey(edge.source)) // and if the edge's source node is in our dict,
				{
					Node sourceNode = nodesByID[edge.source];
					//Debug.Log($"setting edge for {sourceNode._id} (this edge id is {edge._id})");

					if (sourceNode.nodeType == NodeType.Start) // -(quick check on start node)-
					{
						if (sourceNode.nextNodes == null)
							Debug.Log($"next nodes null on {sourceNode._id}");
					}

					// we need to make sure the source node's nextNodes array isn't null so it's ready for assigning
					if (sourceNode.nextNodes != null) 
					{
						if (sourceNode.nextNodes.Length == 0)
						{
							sourceNode.nextNodes = new Node[sourceNode.outs]; // then init the array
						} 
					}
					else // since it's null,
					{
						sourceNode.nextNodes = new Node[sourceNode.outs]; // then init the array 
					}

					// Here we assign, based on the edge's index fitting in nextNodes array
					if (edge.o < sourceNode.nextNodes.Length)
					{
						if (nodesByID.ContainsKey(edge.target))
						{
							sourceNode.nextNodes[edge.o] = nodesByID[edge.target];   
						}
						else
						{
							Debug.LogError($"\n---- BIG WARNING ----\nNode {sourceNode._id} not connected, probably\n----BIG WARNING ----\n");
#if !UNITY_EDITOR
							DebugWebGLHelper.Alert($"Warning, {sourceNode._id} not connected, probably");
#endif
						}
					}
				}
			}
			else
			{
				Debug.LogWarning("Edge is null, removing from list");
				//edgesList.Remove(edge);
			}
		}
	}

	public static string ExtractFromBrackets(string rawString) // for handling handwritten variables
	{
		string newString = rawString.Remove(0, 1);
		int closingBracketIndex = newString.LastIndexOf(']');
		return newString.Remove(closingBracketIndex);
	}
}
