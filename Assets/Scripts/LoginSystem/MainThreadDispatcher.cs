using System;
using System.Collections.Generic;
using UnityEngine;

public class MainThreadDispatcher : MonoBehaviour
{
	private static readonly Queue<Action> _executionQueue = new Queue<Action>();

	public static MainThreadDispatcher Instance { get; private set; }

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
			Debug.Log("MainThreadDispatcher initialized.");
		}
		else
		{
			Debug.LogWarning("Attempt to initialize duplicate MainThreadDispatcher.");
			Destroy(gameObject);
		}
	}

	private void Update()
	{
		lock (_executionQueue)
		{
			while (_executionQueue.Count > 0)
			{
				_executionQueue.Dequeue().Invoke();
			}
		}
	}

	public void Enqueue(Action action)
	{
		if (action == null) return;
		lock (_executionQueue)
		{
			_executionQueue.Enqueue(action);
		}
	}
}
