using UnityEngine;
using UnityEditor;

public class SceneManagerWindow : EditorWindow
{
	public BuildSwitcher buildSwitcher;

	int buildSetupIndex = -1;
	[SerializeField] [HideInInspector] int selectionIndex;
	string[] topicNames;

	[MenuItem("Window/Build-Topic Swapper")]
    public static void ShowWindow()
	{
		GetWindow<SceneManagerWindow>("Build-Topic Swapper");
	}

	private void OnEnable()
	{
		topicNames = new string[buildSwitcher.BuildSetups.Length];
		for (int i = 0; i < topicNames.Length; i++)
		{
			topicNames[i] = buildSwitcher.BuildSetups[i].topicName;
		}
	}

	private void OnGUI()
	{
		GUILayout.Space(8f);
		GUILayout.Label("Topic Data");
		
		buildSwitcher = (BuildSwitcher)EditorGUILayout.ObjectField(buildSwitcher, (typeof(BuildSwitcher)));
		GUILayout.Space(8f);
		EditorGUILayout.LabelField($"Topics in {buildSwitcher.name}:");

		selectionIndex = GUILayout.SelectionGrid(selectionIndex, topicNames, 1);
		if (buildSetupIndex != selectionIndex)
		{
			buildSetupIndex = selectionIndex;
			SwapBuildTopic();
		}

		EditorGUILayout.Space(8f);
		if(GUILayout.Button("Load Selected Topic"))
		{
			LoadScenes();
		}
	}

	void SwapBuildTopic()
	{
		//Debug.Log($"Hello, scene index changed to {buildSetupIndex}");
		buildSwitcher.ChangeSetup(buildSwitcher.BuildSetups[buildSetupIndex]);
	}

	void LoadScenes()
	{
		buildSwitcher.LoadScenes(buildSwitcher.BuildSetups[buildSetupIndex]);
	}
}
