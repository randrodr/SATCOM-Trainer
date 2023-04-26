using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="ScriptableObjects/Gameplay Settings")]
[System.Serializable]
public class GameplaySettings : ScriptableObject
{
	[SerializeField] public float moveSpeed;
	public float mouseSensitivity;
	public bool invertY;
	public float zoomSensitivity;
}
