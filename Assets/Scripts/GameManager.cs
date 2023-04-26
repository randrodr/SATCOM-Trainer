using System;
using System.Collections;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
	[DllImport("__Internal")] private static extern void SendReady();
	[DllImport("__Internal")] private static extern void SendFeedbackToPage(string text);
	[DllImport("__Internal")] private static extern void ShowFeedback();
	[DllImport("__Internal")] private static extern void SendTotalProgress(int total);
	[DllImport("__Internal")] private static extern void SendCorrect(string triggerID);

	private static bool[] correctStepTriggerBools;
	private static GameEvent[] correctStepTriggers;
	
	[SerializeField] private EquinoxFlowchartParser parser;
	[SerializeField] private GameEventHolder eventHolder;
	[SerializeField] private AnimatorSet animatorsInScene;
	[SerializeField] private GO_Set interactableObjectsInScene;
	[SerializeField] private StringsList feedbackPool;
	[SerializeField] private StringVariable feedback;
	[SerializeField] private GameEvent onSoftwareStart;
	[SerializeField] private GameEvent onSoftwareEnd;
	[SerializeField] private MinigameDictionary minigameSet;

	[Header("Topic Specific")]
	//[SerializeField] private SO_Set allTriggers;
	public SO_SetVariable allTriggersHolder;
	
	//public MinigameSetVariable minigameSetHolder;

	private Dictionary<string, Node> nodesByID = new Dictionary<string, Node>();
	private Dictionary<GameEvent, List<InteractableObject>> interactableObjectsByTrigger = new Dictionary<GameEvent, List<InteractableObject>>();
	private DifficultyLevel difficultyLevel;
	private Node nextNode;
	private string correctID;

	public Dictionary<string, GameEvent> triggersByID = new Dictionary<string, GameEvent>();
	public Dictionary<string, Animator> animatorsByID = new Dictionary<string, Animator>();
	public StringsList FeedbackPool { get => feedbackPool; }
	public DifficultyLevel DifficultyLevel { get => difficultyLevel; }
	public Node NextNode { get => nextNode; set => nextNode = value; }
	public MinigameDictionary MinigameSet { get => minigameSet; }
	public GameEvent OnSoftwareStart { get => onSoftwareStart; }
	public GameEvent OnSoftwareEnd { get => onSoftwareEnd; }
	public string CorrectID { set => correctID = value; }

	// test fields
	[Header("Test Fields")]
#if UNITY_EDITOR
	public TextAsset testJson; 
#endif
	public DifficultyLevel testLevel;
	public StringVariable Feedback { get => feedback; }
	public Node Checkpoint { get; set; }

	private void Awake()
	{
		//allTriggers = allTriggersHolder.RuntimeValue;
	}

	private void OnEnable()
	{
		allTriggersHolder.RuntimeValue.OnItemAdded += AddToTriggerDictionary;
		animatorsInScene.OnItemAdded += AddToAnimatorDictionary;
		interactableObjectsInScene.OnItemAdded += AddToIoDictionary;
	}

	private void OnDisable()
	{
		allTriggersHolder.RuntimeValue.OnItemAdded -= AddToTriggerDictionary;
		animatorsInScene.OnItemAdded -= AddToAnimatorDictionary;
		interactableObjectsInScene.OnItemAdded -= AddToIoDictionary;
	}

	private void Start()
	{
#if UNITY_EDITOR
		
		SetDifficulty((int)testLevel);
		StartGame(testJson.text);
#endif
#if !UNITY_EDITOR && UNITY_WEBGL
		WebGLInput.captureAllKeyboardInput = false;
#endif
	}

	public void SetDifficulty(int difficulty)
	{
		difficultyLevel = (DifficultyLevel)difficulty;
		Debug.Log($"Difficulty set to {(DifficultyLevel)difficulty}");
	}

	public void StartGame(string jsonData)
	{		
		// These dictionaries are filled here instead of Awake, because
		//	animators need to add themselves to the runtime set first
		Debug.Log(" - Filling Dictionaries");
		triggersByID = SetToDictionary(allTriggersHolder.RuntimeValue);
		animatorsByID = SetToDictionary(animatorsInScene);
		interactableObjectsByTrigger = SetToDictionary(interactableObjectsInScene);

		Debug.Log($"String recieved: {jsonData}");
		if (parser)
		{
			Debug.Log(" - Parsing data");
			parser.ReadJson(jsonData);
			Debug.Log(" - Setting nodes dictionary");
			nodesByID = parser.nodesByID;
		}
		else
		{
			Debug.Log("GameManager has no parser");
		}
		Debug.Log("setting nodes references");
		SetNodeReferences(nodesByID); // This is where we "initialize" the nodes

		Debug.Log($"triggers: {CountInteractions(new List<Node>(nodesByID.Values))}");

#if !UNITY_EDITOR
		SendTotalProgress(CountInteractions(new List<Node>(nodesByID.Values))); 
#endif

		Debug.Log(" - Starting Steps");
		StartSteps();
	}

	private Dictionary<string, GameEvent> SetToDictionary(SO_Set set)
	{
		Dictionary<string, GameEvent> newDict = new Dictionary<string, GameEvent>();

		for (int i = 0; i < set.Items.Count; i++)
		{
			if (!newDict.ContainsKey(set.Items[i].name))
			{
				newDict.Add(set.Items[i].name, (GameEvent)set.Items[i]);
			}
		}

		return newDict;
	}

	public void AddToTriggerDictionary(ScriptableObject newTrigger)
	{
		Debug.Log($"Adding trigger {newTrigger.name} to dictionary");
		triggersByID = SetToDictionary(allTriggersHolder.RuntimeValue);
	}

	public static Dictionary<string, Animator> SetToDictionary(AnimatorSet set)
	{
		Dictionary<string, Animator> newDict = new Dictionary<string, Animator>();
		Animator[] animators = set.Items.ToArray();

		for (int i = 0; i < set.Items.Count; i++) // loop through animators in set (list)
		{
			if (set.Items[i] == null)
			{
				continue;
			}
			if (!newDict.ContainsKey(set.Items[i].runtimeAnimatorController.name)) 
			{
				for (int j = 0; j < animators.Length; j++) // loop through animators in set (array)
				{
					if (animators[j].runtimeAnimatorController == set.Items[i].runtimeAnimatorController)
					{
						newDict.Add(set.Items[i].runtimeAnimatorController.name, animators[j]);
					}
				}
			}
		}

		return newDict;
	}

	public void AddToAnimatorDictionary(Animator newAnimator)
	{
		//Debug.Log($"Adding animator {newAnimator.name} to dictionary");
		animatorsByID = SetToDictionary(animatorsInScene);
	}

	public Dictionary<GameEvent, List<InteractableObject>> SetToDictionary(GO_Set set)
	{
		Dictionary<GameEvent, List<InteractableObject>> newDict = new Dictionary<GameEvent, List<InteractableObject>>();
		GameEvent tempTrigger;

		for (int i = 0; i < set.Items.Count; i++)
		{
			if (set.Items[i] == null)
			{
				continue;
			}
			
			// look at trigger, if trigger exists in dict, add the io, if not add the key and io
			tempTrigger = set.Items[i].GetComponent<InteractableObject>().ThisTrigger;
			//Debug.Log($"temptrigger for {set.Items[i].name} = {tempTrigger}");
			if (tempTrigger)
			{
				if (!newDict.ContainsKey(tempTrigger))
				{
					newDict.Add(tempTrigger, new List<InteractableObject>());
				}
				newDict[tempTrigger].Add(set.Items[i].GetComponent<InteractableObject>()); 
			}
		}

		return newDict;
	}

	public void AddToIoDictionary(GameObject newIO)
	{
		//Debug.Log($"Adding IO {newIO.name} to dictionary");
		interactableObjectsByTrigger = SetToDictionary(interactableObjectsInScene);
	}

	void SetNodeReferences(Dictionary<string, Node> nodesDictionary)
	{
		foreach (KeyValuePair<string, Node> entry in nodesDictionary)
		{
			entry.Value.Setup(this);
		}
	}

	private void StartSteps()
	{
		foreach (Node node in nodesByID.Values)
		{
			if (node.nodeType == NodeType.Start)
			{
				Debug.Log("Start node found");
				if (node.parent.Equals("$root", System.StringComparison.OrdinalIgnoreCase))
				{
					Debug.Log("THE start node found");
					node.Handle();

					//HandleNode(node);
				}
			}
		}
	}
	
	// For JS to call from UI
	[ContextMenu("Next Node")]
	public void HandleNextNode()
	{
		//Debug.Log("Hiding hints");
		HideHints();
		nextNode.Handle();
	}

	// For JS to call from UI
	public void HandleNode(string nodeID)
	{
		Debug.Log($"Handling node of ID: {nodeID}");

		onSoftwareEnd.Raise();
		if (nodesByID.ContainsKey(nodeID))
		{
			nodesByID[nodeID].Handle(); 
		}
		else
		{
			Debug.Log($"No such node with id: {nodeID}");
		}
	}

	public void SendFeedback()
	{
		SendFeedbackToPage(Feedback.RuntimeValue);
	}

	public void SetListener(GameEvent[] triggers, Node newNextNode)
	{
		GetComponent<GameEventListener>().Response.RemoveAllListeners();
		correctStepTriggerBools = new bool[triggers.Length];
		correctStepTriggers = triggers;
		GetComponent<GameEventListener>().Response.AddListener(CheckTrigger);
		nextNode = newNextNode;
	}

	public void CheckTrigger()
	{
		bool correctTrigger = false;

		// See if the clicked trigger matches any of the correct answers
		for (int i = 0; i < correctStepTriggers.Length; i++)
		{
			if (eventHolder.CurrentEvent == correctStepTriggers[i])
			{
				correctTrigger = true;
				break;
			}
		}

		if (correctTrigger == false)
		{
			Debug.Log($"WRONG CLICK \n Eventfield holds {eventHolder.CurrentEvent} but {correctStepTriggers[0]} is correct");
			//Debug.Log($"Feedback: {feedback.RuntimeValue}");
#if !UNITY_EDITOR
			ShowFeedback(); 
#endif
			return;
		}

		int currentTriggerIndex = Array.IndexOf(correctStepTriggers, eventHolder.CurrentEvent);
		correctStepTriggerBools[currentTriggerIndex] = true;

		for (int i = 0; i < correctStepTriggerBools.Length; i++)
		{
			if (correctStepTriggerBools[i] == false)
			{
				Debug.Log($"Incomplete steps; invoke {correctStepTriggers[i].name}");
				return;
			}
		}

		Debug.Log("CORRECT!");
#if !UNITY_EDITOR
		SendCorrect(correctID);
		correctID = "";
#endif
		//Debug.Log("Hiding hints");
		HideHints();
		//Debug.Log("Hiding minigames");
		HideMinigames();
		correctStepTriggers = null; // hotfix for oplog hints highlighting previous trigger hints
		GetComponent<GameEventListener>().Response.RemoveListener(CheckTrigger);
		HandleNextNode();
	}

	public void SetState(Animator animator, string parameter, bool value)
	{
		animator.SetBool(parameter, value);
	}

	// JS Debug functions

	public SO_Set GetTriggers()
	{
		Debug.Log($"triggers: {allTriggersHolder.RuntimeValue.Items}");
		return allTriggersHolder.RuntimeValue;
	}

	public AnimatorSet GetAnimators()
	{
		Debug.Log($"animators: {animatorsInScene.Items}");
		return animatorsInScene;
	}

	[ContextMenu("show hints")]
	public void ShowHints()
	{
		foreach (GameEvent correctTrigger in correctStepTriggers)
		{
			if (interactableObjectsByTrigger.ContainsKey(correctTrigger))
			{
				foreach (InteractableObject io in interactableObjectsByTrigger[correctTrigger])
				{
					if (io.gameObject.activeInHierarchy)
					{
						io.HighlightHint();
					}
				}
			}
		}
	}

	void HideHints()
	{
		if (correctStepTriggers != null) // basically if this step has a trigger
		{
			foreach (GameEvent correctTrigger in correctStepTriggers) // iterate through triggers
			{
				if (interactableObjectsByTrigger.ContainsKey(correctTrigger)) // check if the trigger corresponds to an interactableObject
				{
					foreach (InteractableObject io in interactableObjectsByTrigger[correctTrigger])
					{
						//Debug.Log("$Hiding hint for ")
						io.StopHint();
					} 
				}
			} 
		}
	}

	void HideMinigames()
	{
		foreach (EventObjectPair minigame in MinigameSet.Items)
		{
			//Debug.Log($"Hiding minigame {minigame.gameObject.transform.parent}");
			minigame.gameObject.SetActive(false);
		}
	}

	// this is used for sending progress bar data
	int CountInteractions(List<Node> nodes)
	{
		int total = 0;

		foreach(Node node in nodes)
		{
			if(node is UnityTriggerNode)
			{
				if (node.content.items[0].value != string.Empty)
					total++;
				else
					Debug.LogWarning($"Empty Trigger Node found: {node._id}, not added to count");
			}
			//UnityTriggerNode newNode = node as UnityTriggerNode;
			//if(newNode != null)
			//{
			//	if(newNode.content.items[0].value != string.Empty)
			//		total++;
			//}
		}

		return total;
	}
	
	// debug
	[ContextMenu("Prev Checkpoint")]
	public void ReturnToCheckpoint()
	{
		Checkpoint.prevNode.Handle();
	}
	
	public void ForceAnimate(string animationData)
	{
		char breakChar = ' ';

		string animatorName = animationData.Substring(0, animationData.IndexOf(breakChar));
		animationData = animationData.Remove(0, animationData.IndexOf(breakChar) + 1);
		Debug.Log($"{animatorName} taken and left {animationData}");

		string parameterName = animationData.Substring(0, animationData.IndexOf(breakChar));
		animationData = animationData.Remove(0, animationData.IndexOf(breakChar) + 1);
		Debug.Log($"{parameterName} taken and left {animationData}");

		string parameterValue = animationData.Substring(0, animationData.IndexOf(breakChar));
		animationData = animationData.Remove(0, animationData.IndexOf(breakChar) + 1);
		Debug.Log($"{parameterValue} taken and left {animationData}");

		string parameterType = animationData;
		Debug.Log($"{parameterType} is left");
		//animationData.Remove(0, animationData.IndexOf(breakChar) + 1);

		Animator animator = animatorsByID[animatorName];


		if(animator)
		{
			UnityAnimatorNode.SetParameter(animator, parameterName, parameterValue, parameterType);
		}
	}
}
