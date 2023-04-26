using UnityEngine;
 
[CreateAssetMenu(fileName = "CursorManager", menuName = "ScriptableObjects/CursorManager")]
public class CursorManager : ScriptableObject
{
	[SerializeField] CursorData normalCursor;
	[SerializeField] CursorData hoverCursor;
	[SerializeField] CursorData holdCursor;

	static CursorData normalCursorStatic;
	static CursorData hoverCursorStatic;
	static CursorData holdCursorStatic;

	private void OnEnable()
	{
		//Debug.Log("Hello im SO");
		normalCursorStatic = normalCursor;
		hoverCursorStatic = hoverCursor;
		holdCursorStatic = holdCursor;

	}

	static void ChangeCursor(CursorData newCursorData)
	{
		Cursor.SetCursor(newCursorData.CursorTexture, newCursorData.Hotspot, newCursorData.CursorMode);
	}

	// public static

	public static void HoverCursor()
	{
		ChangeCursor(hoverCursorStatic);
	}

	public static void NormalCursor()
	{
		ChangeCursor(normalCursorStatic);
	}

	public static void ClickHoldCursor()
	{
		ChangeCursor(holdCursorStatic);
	}

	// instance methods

	public void HoverCursorInstance()
	{
		ChangeCursor(hoverCursorStatic);
	}

	public void NormalCursorInstance()
	{
		ChangeCursor(normalCursorStatic);
	}

	public void ClickHoldCursorInstance()
	{
		ChangeCursor(holdCursorStatic);
	}
}
