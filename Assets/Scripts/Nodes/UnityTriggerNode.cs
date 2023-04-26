using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class UnityTriggerNode : Node
{
	[DllImport("__Internal")] private static extern void SendHint(string text); //js functions
	[DllImport("__Internal")] private static extern void SendFeedbackToPage(string text);
	[DllImport("__Internal")] private static extern void AddToMaxScore(string nodeID);

	GameManager gameManager;
	GameEvent[] triggers;
	StringsList feedbackPool;
	MinigameDictionary minigameSet;

	StringVariable hint;
	StringVariable feedback;

	public GameEvent[] Triggers { get => triggers; }

	public UnityTriggerNode(Node baseNode) : base(baseNode)
	{	
	}

	public override void Handle()
	{
		Debug.Log($"new node of type {nodeType}: {triggers[0]} \n    node id: {_id}");

		gameManager.SetListener(triggers, nextNodes[0]);
		gameManager.CorrectID = _id;
#if !UNITY_EDITOR
		AddToMaxScore(_id); 
#endif

		// vvv For debug checkpointing vvv
		if (prevNode == null)
		{
			if (gameManager.Checkpoint != null)
			{
				prevNode = gameManager.Checkpoint; // <-- setting prevnode to previous TRIGGER, not actual previous
			} 
		}
		gameManager.Checkpoint = this;
		// ^^^ end of debug checkpointing ^^^

		if (minigameSet.ContainsKey(triggers[0]))
		{
			Debug.Log($"Minigame starting for {triggers[0]}");
			minigameSet.MinigameByTrigger(triggers[0]).SetActive(true);
		}
		if (gameManager.DifficultyLevel == DifficultyLevel.Crawl)
		{
			gameManager.ShowHints();
		}
#if !UNITY_EDITOR
		SendHint(content.hint);
		if(gameManager.DifficultyLevel == DifficultyLevel.Run)
		{
			SendFeedbackToPage(PickAString(feedbackPool.strings));
		}
		else
		{
			SendFeedbackToPage(content.feedback);
		}
#else
		if (gameManager.DifficultyLevel == DifficultyLevel.Run)
			feedback.RuntimeValue = PickAString(feedbackPool.strings);
		else
			feedback.RuntimeValue = content.feedback;
#endif
	}

	public override void Setup(GameManager newGameManager)
	{
		//Debug.Log($"Setting up trigger: {_id}");
		gameManager = newGameManager;

		feedback = gameManager.Feedback;
		feedbackPool = gameManager.FeedbackPool;
		minigameSet = gameManager.MinigameSet;

		// turn all those trigger name strings into actual trigger references
		triggers = new GameEvent[content.items.Length];

		for (int i = 0; i < triggers.Length; i++)
		{
			if (gameManager.triggersByID.ContainsKey(content.items[i].value))
			{
				triggers[i] = gameManager.triggersByID[content.items[i].value];
			}
			else
			{
				Debug.LogWarning($"Trigger asset for {_id} not found. Check the scene triggers SO_Set for {content.items[i].value}");
			}
		}
	}

	string PickAString(List<string> strings)
	{
		return strings[Random.Range(0, strings.Count)];
	}
}
