using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEditor;
using UnityEditor.SceneManagement;
 
[CreateAssetMenu(fileName = "BuildSwitcher", menuName = "BuildSwitcher")]
public class BuildSwitcher : ScriptableObject
{
	[System.Serializable]
	public class BuildSetup
	{
		public string topicName;
		[SerializeField] SO_Set topicTriggers;
		[SerializeField] SceneAsset[] scenes;

		public SceneAsset[] Scenes { get => scenes; }
		public SO_Set TopicTriggers { get => topicTriggers; }
	}

	[SerializeField] SO_SetVariable topicTriggers;
	[SerializeField] BuildSetup[] buildSetups;
	
	public BuildSetup[] BuildSetups { get => buildSetups; }
	
	// Change the current triggers, and scenes to those from a given BuildSetup
	public void ChangeSetup(BuildSetup newSetup)
	{
		if (topicTriggers)
		{
			if (newSetup.TopicTriggers)
			{
				topicTriggers.InitialValue = newSetup.TopicTriggers;  
			}
		}

		EditorBuildSettings.scenes = BuildScenes(newSetup.Scenes);
	}

	public void LoadScenes(BuildSetup newSetup)
	{
		Scene previousActive = SceneManager.GetActiveScene();

		// Close open scenes
		for (int i = 0; i < EditorSceneManager.sceneCount; i++)
		{
			EditorSceneManager.CloseScene(EditorSceneManager.GetSceneAt(i), true);
		}

		// Open relevant scenes
		bool previousStillRelevant = false;
		for (int i = 0; i < newSetup.Scenes.Length; i++)
		{
			string scenePath = AssetDatabase.GetAssetPath(newSetup.Scenes[i]);
			EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);

			if (EditorSceneManager.GetSceneByPath(scenePath) == previousActive)
				previousStillRelevant = true;
		}
		if(!previousStillRelevant)
			EditorSceneManager.CloseScene(previousActive, true);

		EditorSceneManager.SetActiveScene(EditorSceneManager.GetSceneAt(1));
	}

	EditorBuildSettingsScene[] BuildScenes(SceneAsset[] sceneAssets)
	{
		List<EditorBuildSettingsScene> editorBuildSettingsScenes = new List<EditorBuildSettingsScene>();

		foreach (SceneAsset sceneAsset in sceneAssets)
		{
			string scenePath = AssetDatabase.GetAssetPath(sceneAsset);
			if (!string.IsNullOrEmpty(scenePath))
				editorBuildSettingsScenes.Add(new EditorBuildSettingsScene(scenePath, true));
		}

		// Set the Build Settings window Scene list
		return editorBuildSettingsScenes.ToArray();
	}
}