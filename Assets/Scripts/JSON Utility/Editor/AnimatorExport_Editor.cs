using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

[CustomEditor(typeof(AnimatorControllerSet))]
public class AnimatorExport_Editor : Editor
{
	AnimatorControllerSet animArray;

	public override void OnInspectorGUI()
	{
		if (GUILayout.Button("Export to JSON"))
		{
			animArray = (AnimatorControllerSet)target;
			string path = EditorUtility.SaveFilePanel("Save JSON", "", animArray.name + ".json", "json");
			Export(path);
			Debug.Log("whoo");
			GUIUtility.ExitGUI();
		}

		base.OnInspectorGUI();
	}

	void Export(string path)
	{
		AnimJsonHelper newArray = new AnimJsonHelper(animArray);
		System.IO.File.WriteAllText(path, JsonUtility.ToJson(newArray, true));
	}
}

[System.Serializable]
public class AnimJsonHelper
{
	public AnimatorData[] animators;

	public AnimJsonHelper(AnimatorControllerSet oldArray)
	{
		animators = new AnimatorData[oldArray.Items.Count];

		for (int i = 0; i < animators.Length; i++)
		{
			animators[i] = new AnimatorData(oldArray.Items[i]);
			
		}
	}
}

[System.Serializable]
public class AnimatorData
{
	public string name;
	public ParameterData[] parameters;

	public AnimatorData(AnimatorController oldAnimator)
	{
		name = oldAnimator.name;
		parameters = new ParameterData[oldAnimator.parameters.Length];

		for (int i = 0; i < oldAnimator.parameters.Length; i++)
		{
			parameters[i] = new ParameterData(oldAnimator.parameters[i]);
		}
	}
}

[System.Serializable]
public class ParameterData
{
	public string name;
	public string type;

	public ParameterData(AnimatorControllerParameter parameter)
	{
		name = parameter.name;
		type = parameter.type.ToString();
	}
}

