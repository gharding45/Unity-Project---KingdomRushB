using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SoldierProjectile : MonoBehaviour
{
	private float healthAmount;
	private float startHealth;
	private int damageAmount;
	private GameObject originTower;
	private GameObject enemy;
	private bool targetFound;
	private GameObject nearestEnemy;
	private float soldierSpeed = 10f;
	private int soldierRange = 6;
	private bool moveToPath;
	private GameObject nearestPathNode = null;
	public Image healthBar;
	public GameObject deathEffect;
	public GameObject hitEffect;
	private GameObject effectParent;


	Color redColour = new Color(1.0f, 0.176f, 0.2f); //colours for health bar
	Color yellowColour = new Color(1.0f, 0.92f, 0.016f);

	public static void Create(Vector3 spawnPosition, float healthAmount, int damageAmount, GameObject originTower)
	{
		Transform soldierTransform = Instantiate(GameAssets.i.soldierProjectilePrefab, spawnPosition, Quaternion.identity);
		SoldierProjectile soldierProjectile = soldierTransform.GetComponent<SoldierProjectile>();
		soldierProjectile.Setup(healthAmount, damageAmount, originTower);
	}
	
	private void Setup(float healthAmount, int damageAmount, GameObject originTower)
	{
		this.healthAmount = healthAmount;
		this.startHealth = healthAmount;
		this.damageAmount = damageAmount;
		this.originTower = originTower;
	}

	void Start()
	{
		effectParent = GameObject.FindGameObjectWithTag("EffectsParent");
		gameObject.transform.SetParent(originTower.transform);
		moveToPath = true;
		GameObject[] pathNodes = GameObject.FindGameObjectsWithTag("PathNode"); //Move soldier to closest Path Node at start
		float shortestPathDistance = Mathf.Infinity;
		foreach(GameObject pathNode in pathNodes)
		{
			float distanceToPathNode = Vector3.Distance (transform.position, pathNode.transform.position);
			if (distanceToPathNode < shortestPathDistance)
			{
				shortestPathDistance = distanceToPathNode;
				nearestPathNode = pathNode; 
			}
		}
		InvokeRepeating ("SearchForEnemy", 0f, 0.4f); //search for closest target every 0.3 seconds rather than every frame
	}

	void Update()
	{
		if (moveToPath == true)
		{
			Vector3 targetPathPosition = nearestPathNode.transform.position;
			Vector3 moveDir = (targetPathPosition - transform.position).normalized; 
			transform.position += moveDir * soldierSpeed * Time.deltaTime; //moving the projectile towards the closest pathNode
			if (Vector3.Distance(transform.position, targetPathPosition) <= 1)
			{
				moveToPath = false;
			}
		}
		else
		{
			return;
		}

	}

	private void SearchForEnemy()
	{
		GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
		float shortestDistance = Mathf.Infinity; //when no enemy found, infinity will not be in range
		targetFound = false;
		foreach(GameObject enemy in enemies)
		{
			float distanceToEnemy = Vector3.Distance (transform.position, enemy.transform.position);
			if (distanceToEnemy < shortestDistance)
			{
				shortestDistance = distanceToEnemy;
				nearestEnemy = enemy; 
			}
			if (nearestEnemy != null && shortestDistance <= soldierRange && nearestEnemy.GetComponent<Enemy>().isAir == false)
			{
				targetFound = true;
			}
		}
		if (targetFound == false)
		{
			return;
		}
		else //when an enemy is in range
		{
			DamageEnemy(nearestEnemy.transform);

		}
	}

	public void TakeDamage(int damageAmount_)
	{
		healthAmount -= damageAmount_;
		float healthPercentage = healthAmount / startHealth;
		healthBar.fillAmount = healthPercentage;

		if (healthPercentage <= 0.25)
		{
			healthBar.color = redColour;
		}
		else if (healthPercentage <= 0.5)
		{
			healthBar.color = yellowColour;
		}

		if (healthAmount <= 0)
		{
			Die();
		} 

	}

	public void DamageEnemy(Transform enemy)
	{
		GameObject effect = (GameObject)Instantiate(hitEffect, enemy.position, transform.rotation); //particle effect on hit
		effect.transform.SetParent(effectParent.transform);
		Destroy(effect, 5f);
		Enemy enemy_ = enemy.GetComponent<Enemy>();
		enemy_.TakeDamage(damageAmount, "Barracks");
	}

	public void Die()
	{
		GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
		effect.transform.SetParent(effectParent.transform);
		Destroy(effect, 5f);
		originTower.GetComponent<Barracks>().SoldierDied(); 
		Destroy(gameObject);
	}
}
