using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class InfernoTower : MonoBehaviour
{
	private Vector3 projectileShootFromPosition;
	public LineRenderer lineRenderer;
	public ParticleSystem impactEffect;
	private bool keepTarget = false;
	private GameObject oldTarget = null;
	private int damageOverTime;
	private int targetTime = 0;
	private string towerType = "InfernoTower";
	private string enemyTag = "Enemy";
	private float shootTimer;
	private bool targetFound;
	private GameObject nearestEnemy = null;
	private GameObject towerParent;

	public float shootTimerMax; //tower speed
	public int damageAmount; //tower damage 
	public float range; //tower ramge
	public int maxDamage; //maxium damage which the tower can ramp up to
	public int damageMultiplier; //how much the damage increases each tick



	private void Awake()
	{
		towerParent = GameObject.FindGameObjectWithTag("TowersParent");
		gameObject.transform.SetParent(towerParent.transform);
		projectileShootFromPosition = transform.Find("infernoProjectileShootFromPosition").position; //start point for projectile
		InvokeRepeating ("UpdateTarget", 0f, 0.5f); //search for closest target every 0.5 seconds rather than every frame
		damageOverTime = damageAmount;
	}

	private void Update()
	{
		if (GameManagement.gameOver == true)
			this.enabled = false;
		if (targetFound == false)
		{
			keepTarget = false;
			if (lineRenderer.enabled)
			{
				lineRenderer.enabled = false;
				impactEffect.Stop();
			}
			return;
		}
		if (keepTarget == true)
		{
			keepTarget = false;
			nearestEnemy = oldTarget;
			targetTime ++;

			if (targetTime == 2)
			{
				damageOverTime *= damageMultiplier;
				targetTime = 0;
				if (damageOverTime >= maxDamage)
					damageOverTime = maxDamage;
			}
		}
		if (nearestEnemy != oldTarget)
		{
			targetTime = 0;
			damageOverTime = damageAmount;
		}
		Laser();
	}

	void Laser ()
	{
		try 
		{
			nearestEnemy.GetComponent<Enemy>().TakeDamage(damageOverTime * Time.deltaTime, towerType);
			if (!lineRenderer.enabled)
			{
				lineRenderer.enabled = true;
				impactEffect.Play();
			}
			lineRenderer.SetPosition(0, projectileShootFromPosition);
			lineRenderer.SetPosition(1, nearestEnemy.transform.position);
			impactEffect.transform.position = new Vector3(nearestEnemy.transform.position.x, nearestEnemy.transform.position.y, -5);
		}
		catch
		{
			return;
		}
	}

	void UpdateTarget ()
	{
		if (nearestEnemy != null)
		{
			float stillInRange = Vector3.Distance (transform.position, nearestEnemy.transform.position); 
			if (stillInRange <= range) 
			{
				keepTarget = true;
				oldTarget = nearestEnemy; //Lock onto an in range target, rather than switching to the closest 
			}
		}
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
