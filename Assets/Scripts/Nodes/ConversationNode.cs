using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[System.Serializable]
public class ConversationNode : Node
{
	[DllImport("__Internal")] private static extern void SendDialogue(string nodeID, bool showButton, string fileName); //js functions
	[DllImport("__Internal")] private static extern void SendAudio(string filename);
	[DllImport("__Internal")] private static extern void SendTaskTitle(string taskTitle);

	GameManager gameManager;

	public ConversationNode(Node baseNode) :base(baseNode)
	{

	}

	public override void Handle()
	{
		Debug.Log($"New Conversation node: {content.conversation.text}");

		// send title if title
		if (title != "")
		{			
#if !UNITY_EDITOR
			SendTaskTitle(title);
#endif
		}

		// send audio
#if !UNITY_EDITOR
		SendAudio(content.audio);
#endif
		if (content.audio == "")
		{
			Debug.LogWarning($"blank audio field");
		}

		// check if there's more dialogue after this
		// if there's more dialogue, make a continue button show up and don't handle next right away
		bool showButton = false;
		
		/*

		if (nextNodes[0] is ConversationNode)
		{
			gameManager.NextNode = nextNodes[0];
			showButton = true;
		}
		else
		{
			if (nextNodes[0] is ConditionNode)
			{
				if(nextNodes[0].nextNodes[(int)gameManager.DifficultyLevel] is ConversationNode)
				{
					gameManager.NextNode = nextNodes[0];
					showButton = true;
				}
				else
				{
#if !UNITY_EDITOR
					SendDialogue(content.conversation.text, showButton, content.backgroundImage);
#endif
					nextNodes[0].Handle();
					return;
				}
			}
			else
			{
#if !UNITY_EDITOR
				SendDialogue(content.conversation.text, showButton, content.backgroundImage);
#endif
				nextNodes[0].Handle();
				return;
			}
		}
		*/

		if (!(nextNodes[0] is UnityTriggerNode))
		{
			gameManager.NextNode = nextNodes[0];
			showButton = true;
		}
		
#if !UNITY_EDITOR
			SendDialogue(_id, showButton, content.backgroundImage);
#endif

		if(showButton)
		{
			gameManager.NextNode = nextNodes[0];
		}
		else
		{
			nextNodes[0].Handle();
		}
	}

	public override void Setup(GameManager newGameManager)
	{
		gameManager = newGameManager;

		// Remove text in <> brackets
		content.conversation.text = RemoveBrackets(content.conversation.text);
		title = RemoveBrackets(title);
	}

	string RemoveBrackets(string originalString)
	{
        int bracketIndex = originalString.IndexOf('<');
        if (bracketIndex > -1)
        {
            int bracketsLength = originalString.IndexOf('>') - bracketIndex + 1;
            if (bracketsLength > 0)
            {
                return originalString.Remove(bracketIndex, bracketsLength);
            }
        }
		return originalString;

		// "<1234>"
		// "Do thing <2345>"
	}

	bool ShouldPause()
	{


		return false;
	}
}
