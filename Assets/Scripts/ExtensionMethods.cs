using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public static class ExtensionMethods
{
	[DllImport("__Internal")] public static extern void Alert(string alert);

	public static void Alert(this Debug debug, string alert)
	{
		Alert(alert);
	}
}
