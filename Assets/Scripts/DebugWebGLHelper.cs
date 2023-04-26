using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public static class DebugWebGLHelper
{
	[DllImport("__Internal")] private extern static void DebugAlert(string alertMessage);

	public static void Alert(string e)
	{
		Debug.LogError(e);
		DebugAlert(e);
	}
}
