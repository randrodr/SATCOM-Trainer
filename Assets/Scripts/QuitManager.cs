using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitManager : MonoBehaviour
{
    public void QuitUnity()
	{
		Debug.Log("Quitting application");
		Application.Quit();
	}
}
