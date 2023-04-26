using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Content
{
	// Conversation Type
	public Conversation conversation;
	public string backgroundImage;
	public string audio;

	// Trigger and Animator Type
	public NodeItems[] items;

	// Trigger Type
	public string feedback;
	public string hint;

	// Animator	Type
	//public string animatorParameter;
	//public string parameterType;
	//public float stateFloat;
	//public int stateInt;
	//public bool stateBool;

	// DecisionPoint Type
	//public Branch[] branches;
	//public bool noAssessed;

	// Transition Type
	public string text;

	// Variable Type
	public string value;

	// Condition Type
	public string[] branches;
}

[System.Serializable]
public class NodeItems
{
	// Trigger type
	public string value;
	public GameEvent trigger;

	// Animator type
	public string name;
	public Animator animator;
	public StateChange update;
}

[System.Serializable]
public class StateChange
{
	public string name;
	public string value;
	public string type;
}

[System.Serializable]
public class Speaker
{
	public string bio;
	public string name;
	public string _id;
}

[System.Serializable]
public class Conversation
{
	public Speaker speaker;
	public string text;
}

[System.Serializable]
public class Branch
{
	public string option;
	public float grade;
	public string feedback;
}

[System.Serializable]
public class Edge
{
	public string _id;
	public string source;
	public string target;
	// i and o are the index of in/out edges to/from nodes
	public int i;
	public int o; // index within source node
}

[System.Serializable]
public class EquinoxJSON
{
	public Node[] nodes;
	public Edge[] edges;
}


// Enums 

[System.Serializable]
public enum NodeType
{
	none,
	Conversation,
	UnityTrigger,
	UnityAnimator,
	Variable,
	DecisionPoint,
	Condition,
	Composite,
	Start,
	End,
	OPLOG,
	Transition
}

[System.Serializable]
public enum ParameterType
{
	none,
	Float,
	Int,
	Bool,
	Trigger
}

public enum DifficultyLevel
{
	Crawl,
	Walk,
	Run
}

