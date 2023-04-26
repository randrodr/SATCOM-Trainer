using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchTheNumber : MonoBehaviour
{
	public FloatVariable correctAnswer;
	public FloatVariable answerAttempt;
	public float leniency = 5f;
	public float repeatAfter;

	public GameEventHolder gameEventHolder;
	public GameEvent incorrect;
	public GameEvent correct;

	public GameEvent numberChanged;

	public void StartAddNumberCoroutine(float number)
	{
		StartCoroutine(AddNumberCoroutine(number));
	}

	public void AddNumber(float number)
	{
		answerAttempt.RuntimeValue = Mathf.Repeat(answerAttempt.RuntimeValue + number, repeatAfter);
		numberChanged.Raise();
		CheckNumber();
	}

	public IEnumerator AddNumberCoroutine(float number)
	{
		while(true)
		{
			// Mathf.Repeat wraps the number
			AddNumber(number);
			yield return null;
		}
	}

	public void CheckNumber()
	{
		//leniency added here for imperfect answers
		if(answerAttempt.RuntimeValue >= correctAnswer.RuntimeValue - leniency &&
			answerAttempt.RuntimeValue <= correctAnswer.RuntimeValue + leniency)
		{
			Debug.Log($"{answerAttempt.RuntimeValue} is correct");
			gameEventHolder.CurrentEvent = correct;
		}
		else
		{
			//Debug.Log($"{answerAttempt.RuntimeValue} is not near {correctAnswer.RuntimeValue}");
			gameEventHolder.CurrentEvent = incorrect;
		}
	}
}
