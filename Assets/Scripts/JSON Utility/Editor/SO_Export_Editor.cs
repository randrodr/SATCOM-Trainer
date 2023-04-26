using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SO_Set))]
public class SO_Export_Editor : Editor
{
	SO_Set soArray;

	public override void OnInspectorGUI()
	{
		if(GUILayout.Button("Export to JSON"))
		{
			soArray = (SO_Set)target;
			string path = EditorUtility.SaveFilePanel("Save JSON", "", soArray.name + ".json", "json");
			Export(path);
			Debug.Log("whoo");
			GUIUtility.ExitGUI();
		}

		base.OnInspectorGUI();
	}

	void Export(string path)
	{
		soJsonHelper newArray = new soJsonHelper(soArray);

		Debug.Log($"new array is {newArray}");
		System.IO.File.WriteAllText(path, JsonUtility.ToJson(newArray, true));
	}
}

[System.Serializable]
public class soJsonHelper
{
	public string[] items;

	public soJsonHelper(SO_Set oldArray)
	{
		items = new string[oldArray.Items.Count];

		for (int i = 0; i < items.Length; i++)
		{
			items[i] = oldArray.Items[i].name;
		}
	}
}
