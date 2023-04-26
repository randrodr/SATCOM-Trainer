using UnityEngine;
 
[CreateAssetMenu(fileName = "CursorData", menuName = "ScriptableObjects/CursorData")]
public class CursorData : ScriptableObject
{
	[SerializeField] Texture2D cursorTexture;
	[SerializeField] Vector2 hotspot;
	[SerializeField] CursorMode cursorMode;

	public Texture2D CursorTexture { get => cursorTexture; }
	public Vector2 Hotspot { get => hotspot; }
	public CursorMode CursorMode { get => cursorMode; }
}