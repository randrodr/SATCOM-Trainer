using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	[DllImport("__Internal")] private static extern void SendReady();

	[SerializeField] int activeScene;
	[SerializeField] int[] sceneToLoadIndex;

	bool[] sceneIndexLoaded;

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void Start()
	{
		sceneIndexLoaded = new bool[sceneToLoadIndex.Length];

#if !UNITY_EDITOR
		for (int i = 0; i < sceneToLoadIndex.Length; i++)
		{
			StartCoroutine(LoadScene(sceneToLoadIndex[i]));
		} 
#endif
	}
	
	public void SetActiveScene(int index)
	{
		SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(index));
		Debug.Log($"Active scene: {SceneManager.GetSceneByBuildIndex(index).name}");
	}

	IEnumerator LoadScene(int index)
	{
		AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(index, LoadSceneMode.Additive);

		while (!asyncLoad.isDone)
		{
			yield return null;
		}

		//sceneIndexLoaded[index] = asyncLoad.isDone;

		if (index == activeScene)
		{
			SetActiveScene(index);
		}
	}

	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		bool allLoaded = true;

		for (int i = 0; i < sceneToLoadIndex.Length; i++)
		{
			if (!SceneManager.GetSceneByBuildIndex(sceneToLoadIndex[i]).isLoaded)
				allLoaded = false;
		}

		if (allLoaded)
		{
#if !UNITY_EDITOR
		SendReady(); 
#endif
		}
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
}
