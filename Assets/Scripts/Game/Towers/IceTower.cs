using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class IceTower : MonoBehaviour
{
	private Vector3 projectileShootFromPosition;
	private string enemyTag = "Enemy";
	private float shootTimer;
	private bool targetFound;
	private GameObject nearestEnemy = null;
	private GameObject towerParent;

	public float shootTimerMax; //tower speed
	public int damageAmount; //tower damage 
	public float range; //tower ramge
	public float slowPercentage; //tower slow percentage (1 - %)



	private void Awake()
	{
		towerParent = GameObject.FindGameObjectWithTag("TowersParent");
		gameObject.transform.SetParent(towerParent.transform);
		projectileShootFromPosition = transform.Find("iceProjectileShootFromPosition").position; //start point for projectile
		InvokeRepeating ("UpdateTarget", 0f, 0.5f); //search for closest target every 0.5 seconds rather than every frame
	}

	private void Update()
	{
		if (GameManagement.gameOver == true)
			this.enabled = false;
		shootTimer -= Time.deltaTime; 
		if (shootTimer <= 0f) //shoots projectile when timer is 0
		{
			shootTimer = shootTimerMax;

			if (targetFound == false)
			{
				return;
			}
			else
			{
				IceProjectile.Create(projectileShootFromPosition, nearestEnemy, damageAmount, slowPercentage);
			}
		}

	}

	void UpdateTarget ()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
		float shortestDistance = Mathf.Infinity; //when no enemy found, infinity will not be in range
		targetFound = false;
		foreach(GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance (transform.position, enemy.transform.position);
			Enemy enemy_ = enemy.GetComponent<Enemy>();
			if (distanceToEnemy < shortestDistance && enemy_.getIsAir() == false)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy; 
			}
			if (nearestEnemy != null && shortestDistance <= range)
			{
				targetFound = true;
			}
		}
	}
}
