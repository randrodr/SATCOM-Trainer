using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EquinoxFlowchartParser))]
public class ParserButton : Editor
{
	public override void OnInspectorGUI()
	{
		EquinoxFlowchartParser parser = (EquinoxFlowchartParser)target;

		if(GUILayout.Button("Read JSON"))
		{
			parser.ReadJson();
		}

		DrawDefaultInspector();
	}
}
