using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class RuntimeSet<T> : ScriptableObject
{
	public List<T> Items = new List<T>();

	public delegate void Del(T item);
	public Del OnItemAdded = delegate { };

	public virtual void Add(T t)
	{
		if (!Items.Contains(t))
		{
			Items.Add(t);
			OnItemAdded(t);
		}
	}

	public void Remove(T t)
	{
		if (Items.Contains(t))
			Items.Remove(t);
	}
}
