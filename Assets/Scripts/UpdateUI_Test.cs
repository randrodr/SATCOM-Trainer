using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateUI_Test : MonoBehaviour
{
	public StringVariable testString;
	public Text displayText;

    // Update is called once per frame
    void Update()
    {
		displayText.text = testString.RuntimeValue;
    }
}
