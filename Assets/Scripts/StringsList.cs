using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "StringsList", menuName = "ScriptableObjects/StringsList")]
public class StringsList : ScriptableObject
{
	public List<string> strings;
}