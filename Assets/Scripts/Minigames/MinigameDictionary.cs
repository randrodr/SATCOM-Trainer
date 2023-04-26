using UnityEngine;
 
[CreateAssetMenu(fileName = "Minigame Dictionary", menuName = "ScriptableObjects/MinigameDictionary")]
public class MinigameDictionary : RuntimeSet<EventObjectPair>, ISerializationCallbackReceiver
{
	public void Add(GameEvent triggerToAdd, GameObject minigame)
	{
		// check to see if contains
		if(!ContainsKey(triggerToAdd))
		{
			Items.Add(new EventObjectPair(triggerToAdd, minigame));
			//OnItemAdded(t);
		}
	}

	public GameObject MinigameByTrigger(GameEvent trigger)
	{
		foreach(EventObjectPair item in Items)
		{
			if(item.gameEvent == trigger)
			{
				return item.gameObject;
			}
		}

		Debug.LogWarning($"Did not find minigame for {trigger.name}");
		return null;
	}

	public bool ContainsKey(GameEvent triggerToCheck)
	{
		foreach (EventObjectPair item in Items)
		{
			if (item.gameEvent == triggerToCheck)
			{
				return true;
			}
		}
		return false;
	}

	public void OnAfterDeserialize()
	{
		Items.Clear();
	}

	public void OnBeforeSerialize()
	{
		
	}
}