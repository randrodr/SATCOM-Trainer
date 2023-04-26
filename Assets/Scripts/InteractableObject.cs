using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Runtime.InteropServices;
using UnityEngine.Events;
using UnityEngine.UI;

public class InteractableObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler

{
	[DllImport("__Internal")] private static extern void SendHover(string text);

	[Header("Do not edit this little group of fields")]
	[SerializeField] protected GameEvent onClick;
	[SerializeField] protected GameEventHolder eventField;
	[SerializeField] private GO_Set interactableObjectsInScene;

	[Header("Editable fields")]
	[SerializeField] private GameEvent thisTrigger;
	[SerializeField] string displayName = "placeholder";
	[SerializeField] List<Renderer> extraRenderers;

	private Color hoverColor = Color.cyan;
	private Color hintColor = new Color(.9f, .9f, .4f, 1);
	private float hintFlashFrequency = 1f;
	private List<Renderer> renderers = new List<Renderer>();

	private Color[] originalEmission;
	private bool originalEmit;
	IEnumerator hintRoutine;
	private bool hinting;
	private Color hotspotHintColor;
	private Color tempHintColor;
	private bool isDragging;

	//test stuff
	//public StringVariable testString;

	public GameEvent ThisTrigger
	{
		get
		{
			return thisTrigger;
		}
	}

	public void Awake()
	{
		if (GetComponent<Renderer>())
		{
			renderers.Add(GetComponent<Renderer>());
		}
		// Modify renderers list
		foreach (Renderer renderer in extraRenderers)
		{
			if (!renderers.Contains(renderer))
				if(renderer)
					renderers.Add(renderer);
		}
		if (renderers.Count > 0)
		{
			originalEmission = new Color[renderers.Count];
			//originalEmit = renderers[0].material.IsKeywordEnabled("_EMISSION");
			for (int i = 0; i < renderers.Count; i++)
			{
				if (renderers[i].material.HasProperty("_EmissionColor"))
				{
					originalEmission[i] = renderers[i].material.GetColor("_EmissionColor"); 
				}
				else
				{
					originalEmission[i] = Color.black;
				}
			}
		}
		if (interactableObjectsInScene)
		{
			interactableObjectsInScene.Add(gameObject);
		}

		hintRoutine = AnimateHighlight(hintFlashFrequency);
		hotspotHintColor = new Color(hintColor.r, hintColor.g, hintColor.b);
	}

	public virtual void OnPointerClick(PointerEventData eventData)
	{
		if (!isDragging)
		{
			Debug.Log($"Raising {onClick.name} from {name} for {thisTrigger}");
			eventField.CurrentEvent = thisTrigger;
			onClick.Raise(); 
		}
	}

	public virtual void OnPointerEnter(PointerEventData eventData)
	{
		CursorManager.HoverCursor();
		if (thisTrigger)
		{
			ChangeEmission(hoverColor);
		}

		// Check what else should highlight or not


		if (eventData.pointerCurrentRaycast.gameObject.GetComponent<InteractableObject>())
		{
			InteractableObject[] ioInParents = eventData.pointerCurrentRaycast.gameObject.GetComponentsInParent<InteractableObject>(true);
			foreach (InteractableObject io in ioInParents)
			{
				if (io.gameObject != eventData.pointerCurrentRaycast.gameObject)
				{
					Debug.LogWarning($"blocking highlight on {io.name}");
					io.ChangeEmission(originalEmission, false);
				}
			}
			if (eventData.pointerCurrentRaycast.gameObject != gameObject)
			{
				Debug.LogWarning($"blocking highlight on this {gameObject.name}");
				ChangeEmission(originalEmission, false);
			}
		}

#if !UNITY_EDITOR
		SendHover(displayName); 
#endif
	}

	public virtual void OnPointerExit(PointerEventData eventData)
	{
		ChangeEmission(originalEmission, false);

		// Check what cursor should be
		if (eventData.pointerCurrentRaycast.gameObject == null || eventData.pointerCurrentRaycast.gameObject.GetComponent<InteractableObject>() == null)
		{
			CursorManager.NormalCursor();
#if !UNITY_EDITOR
			SendHover(string.Empty);
#endif
		}
		else
		{
#if !UNITY_EDITOR
			SendHover(eventData.pointerCurrentRaycast.gameObject.GetComponent<InteractableObject>().displayName);
#endif
			eventData.pointerCurrentRaycast.gameObject.GetComponent<InteractableObject>().OnPointerEnter(eventData);
		}
	}

	[ContextMenu("test hint flash")]
	public void HighlightHint()
	{
		hinting = true;
		StartCoroutine(hintRoutine);
	}

	public void StopHint()
	{
		StopCoroutine(hintRoutine);
		Color[] newColorArray = new Color[renderers.Count];
		for (int i = 0; i < newColorArray.Length; i++)
		{
			newColorArray[i] = originalEmission[i];
		}

		ChangeEmission(newColorArray, false);
		hinting = false;
	}

	IEnumerator AnimateHighlight(float frequency)
	{
		float intensity;
		//tempHintColor = hintColor;
		//Debug.Log($"{displayName} should be pulsing {hintColor}");

		while (true)
		{
			intensity = (Mathf.Sin(Time.time * frequency * 3.14f) + 2f) * .3f;
			//intensity = Mathf.Lerp(intensity, 0f, frequency * Time.deltaTime);

			ChangeEmission(hintColor * intensity);
			yield return null;
		}
	}


	void ChangeEmission(Color[] newColor, bool emit = true)
	{
		//Renderer renderer = GetComponent<Renderer>();
		foreach (Renderer renderer in renderers)
		{
			foreach (Material mat in renderer.materials)
			{
				mat.EnableKeyword("_EMISSION");
			}
		}

		if (renderers != null)
		{
			if (renderers.Count > 0)
			{
				//Debug.Log($"{gameObject} highlight {emit} and renderer count = {renderers.Count}");
				for (int i = 0; i < renderers.Count; i++)
				{
					if (hinting && renderers[i] is SpriteRenderer)
					{
						hotspotHintColor = Color.Lerp(Color.white, hintColor, newColor[i].a);
						renderers[i].material.color = emit ? hotspotHintColor : Color.white;
					}

					foreach (Material mat in renderers[i].materials)
					{
						if (renderers[i] is MeshRenderer || renderers[i] is SkinnedMeshRenderer)
						{
							mat.SetColor("_EmissionColor", newColor[i]);
						}
					}
				}
			}
		}
	}

	void ChangeEmission(Color color, bool emit = true)
	{
		Color[] newColors = new Color[renderers.Count];

		for (int i = 0; i < newColors.Length; i++)
		{
			newColors[i] = color;
		}

		ChangeEmission(newColors, emit);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		//Debug.Log("dragging...");
		isDragging = true;
		OnPointerExit(eventData);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		isDragging = false;
	}

	public void OnDrag(PointerEventData eventData)
	{
		// just here because all three drag handlers need to be present for any drag functionality to work...
	}
}
