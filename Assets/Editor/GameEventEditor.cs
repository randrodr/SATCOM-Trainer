using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameEvent))]
public class GameEventEditor : Editor
{
	GameEvent myTarget;

	public void OnEnable()
	{
		myTarget = (GameEvent)target;
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
		GUILayout.Space(10);
		if (myTarget)
		{
			if (GUILayout.Button("Raise Event"))
			{
				Debug.Log($"Raising {myTarget} from editor");
				myTarget.Raise();
			} 
		}
	}
}
