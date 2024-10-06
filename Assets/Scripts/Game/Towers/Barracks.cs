using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barracks : MonoBehaviour
{
	private Vector3 soliderSpawnPosition;
	private int soldiersAlive = 0;
	private GameObject towerParent;

	public float spawnTimer; //solider spawning speed (once the current solider is dead)
	public float soldierHealthAmount;
	public int soldierDamageAmount;

	void Awake()
	{
		towerParent = GameObject.FindGameObjectWithTag("TowersParent");
		gameObject.transform.SetParent(towerParent.transform);
		soliderSpawnPosition = transform.Find("soliderProjectileShootFromPosition").position; 
	}

	void Update()
	{
		if (soldiersAlive != 0 && soldiersAlive != 1)
		{
			soldiersAlive = 0;
		}
		if (soldiersAlive == 0)
		{
			soldiersAlive += 1;
			StartCoroutine(spawnDelay());
		}
		
		//SoldierProjectile.Create(soliderSpawnPosition, soldierHealthAmount, soldierDamageAmount, gameObject);
		//soldiersAlive += 1;
	}

	private IEnumerator spawnDelay() //delay between soldier death and new soldier spawning
	{
		yield return new WaitForSeconds(spawnTimer);
		SoldierProjectile.Create(soliderSpawnPosition, soldierHealthAmount, soldierDamageAmount, gameObject);
	}

	public void SoldierDied()
	{

		soldiersAlive-=1;
	}
}