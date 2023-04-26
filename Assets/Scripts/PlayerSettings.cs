using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/Player Settings")]
public class PlayerSettings : ScriptableObject
{
	public float moveSpeed;
	public float mouseSensitivity;
	public bool invertY;
	public float zoomSensitivity;
}
