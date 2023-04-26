using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageArray : MonoBehaviour
{
	public Image image;
	public Sprite[] sprites;

	public void SetImage(int index)
	{
		if (image)
		{
			image.sprite = sprites[index]; 
		}
		else
		{
			Debug.LogWarning("No image attached");
		}
	}

	public void SetImage(float index)
	{
		SetImage((int)index);
	}

	public void SetImage(FloatVariable index)
	{
		SetImage(index.RuntimeValue);
	}
}
