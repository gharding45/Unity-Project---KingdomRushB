using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable] //allows creation of waves in the inspector
public class Wave 
{
	public List<GameObject> enemies;
}

public class WaveSpawner : MonoBehaviour
{
	public Transform spawnPoint;
	public float timeBetweenWaves;
	private float countdown = 5f; //5 second countdown as game starts, then uses timeBetweenWaves variable
	public Text waveCountdownText;
	public GameObject finalWaveText;
	public GameObject gameManager;
	public List<Wave> waves;
	private int waveIndex = 0;
	public static int EnemiesAlive = 0;
	private bool finalWaveTextActivate = false;
	private GameObject[] enemyList;

	private int front = 0;
	private int rear = 0;
	private int queueSize = 0;
	private int maxSize = 0;
	private List<GameObject> waveQueue = new List<GameObject>();

	void Start()
	{
		maxSize = findMaxSize();
		for (int i = 0; i < maxSize; i++)
		{
			waveQueue.Add(null);
		}
	}


	void Update()
	{
		enemyList = GameObject.FindGameObjectsWithTag("Enemy");
		if (EnemiesAlive > 0 || enemyList.Length > 0) //next wave starts when all enemies are dead
			return; 

		if (finalWaveTextActivate == true) 
		{
			finalWaveText.SetActive(true);
			finalWaveTextActivate = false;
		}

		if (countdown <= 0f)
		{
			if (waves.Count == waveIndex)
			{
				gameManager.GetComponent<GameManagement>().WinGame();
				this.enabled = false;
			}
			StartCoroutine(SpawnWave(waves[waveIndex]));
			countdown = timeBetweenWaves;
		}
		countdown -= Time.deltaTime; //remove 1 per second
		waveCountdownText.text = ("Next wave in " + Mathf.Round(countdown).ToString() + "s"); 
	}

	IEnumerator SpawnWave(Wave wave) //Coroutine allows pauses seperate from rest of the code, slowly release enemies rather than all at once
	{
		finalWaveText.SetActive(false);
		waveIndex++;
		EnqueueList(wave.enemies);
		int repeats = queueSize; //queueSize changes while iterating, use temporary variable instead.
		for(int i = 0; i < repeats; i++)
		{
			SpawnEnemy(Dequeue());

			yield return new WaitForSeconds(1.0f); //time to wait between each iteration
		}
		if ((waves.Count - 1) == waveIndex) //when 1 wave remains
		{
			finalWaveTextActivate = true; 
		}

	}

	void SpawnEnemy(GameObject enemy)
	{
		Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
		EnemiesAlive++;
	}

	private int findMaxSize() //Find the longest wave length 
	{
		int longestWave = 0;
		for (int i = 0; i < waves.Count; i++)
		{
			if (waves[i].enemies.Count > longestWave)
			{
				longestWave = waves[i].enemies.Count;
			}
		}
		return longestWave;
	}

	private void EnqueueList(List<GameObject> items)
	{
		for(int i = 0; i < items.Count; i++)
		{
			Enqueue(items[i]);
		}
	}

	private void Enqueue(GameObject item)
	{
		if (isFull() == true)
		{
			Debug.Log("Queue full. Nothing to enqueue.");
		}
		else
		{
			waveQueue[rear] = item;
			queueSize += 1;
			rear += 1;
			if (rear >= maxSize)
			{
				rear = rear % maxSize;
			}
		}
	}
	private GameObject Dequeue()
	{
		if (isEmpty() == true)
		{
			Debug.Log("Queue empty. Nothing to dequeue.");
			return null;
		}
		else
		{
			GameObject poppedItem = waveQueue[front];
			queueSize -=1;
			front += 1;
			if (front >= maxSize)
			{
				front = front % maxSize;
			}
			return poppedItem;
		}
	}

	private bool isFull()
	{
		if (queueSize > maxSize)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	private bool isEmpty()
	{
		if (queueSize <= 0)
		{
			return true;
		}
		else
		{
			return false;
		}
	}
}
