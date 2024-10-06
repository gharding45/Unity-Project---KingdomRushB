using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
	private float speed;
	private float health;
	private Waypoints wpoints;
	private int waypointIndex;
	private int count = 0;
	private SpriteRenderer sprite;
	private bool targetFound = false;
	private GameObject nearestSoldier = null;
	private bool engangedWithSoldier = false;
	private GameObject enemyParent;
	private GameObject effectParent;

	public float startHealth; //enemy health
	public float startSpeed; //enemy speed
	public int moneyOnDeath; //money given when killed
	public int playerDamage; //damage dealt to player health if reaches end of map. Also the damage dealt to barracks soldiers.
	public bool isAir; //flying unit
	public bool isArmoured; //armoured unity
	public Image healthBar; //enemy health bar
	public GameObject deathEffect; //enemy death particle effect
	public GameObject hitEffect; //damaging soldier particle effect;

	Color redColour = new Color(1.0f, 0.176f, 0.2f); //colours for health bar
	Color yellowColour = new Color(1.0f, 0.92f, 0.016f);


	void Start()
	{
		wpoints = GameObject.FindGameObjectWithTag("Waypoints").GetComponent<Waypoints>();
		enemyParent = GameObject.FindGameObjectWithTag("EnemiesParent");
		effectParent = GameObject.FindGameObjectWithTag("EffectsParent");
		gameObject.transform.SetParent(enemyParent.transform);
		speed = startSpeed;
		health = startHealth;
		sprite = GetComponent<SpriteRenderer>(); //used to change colour when slowed

		if (!isAir) //Air enemies don't attack soldiers
		{
			InvokeRepeating ("SearchForSoldier", 0f, 0.1f); //search for closest target every 0.5 seconds rather than every frame
		}	
	}

	void Update()
	{
		if (GameManagement.gameOver == true)
			this.enabled = false;

		if (engangedWithSoldier == false)
		{
			transform.position = Vector2.MoveTowards(transform.position, wpoints.waypoints[waypointIndex].position, speed * Time.deltaTime); //move character 
		}

		if(Vector2.Distance(transform.position, wpoints.waypoints[waypointIndex].position) < 0.1f) //when the enemy is approaching the next waypoint
		{
			if (waypointIndex < wpoints.waypoints.Count - 1)
			{
				waypointIndex++;
			}
			else
			{
				EndPath();
			}
		}
		if (speed != startSpeed)
			count += 1;

		if (speed != startSpeed && count >= 500)
		{
			speed = startSpeed;
			count = 0;
			sprite.color = new Color (1,1,1,1);
		}

	}

	public bool getIsAir()
	{
		return isAir;
	}	

	public void TakeDamage(float damageAmount, string tower)
	{
		if (tower == "WizardTower" || tower == "IceTower")
			if (isArmoured == true)
				damageAmount *= 4;

		if (tower == "RockThrower" && isArmoured == true)
			damageAmount *= .75f;

		health -= damageAmount;

		float healthPercentage = health / startHealth;
		healthBar.fillAmount = healthPercentage;

		if (healthPercentage <= 0.25)
		{
			healthBar.color = redColour;
		}
		else if (healthPercentage <= 0.5)
		{
			healthBar.color = yellowColour;
		}

		if (health <= 0)
		{
			Die();
		}
	}

	public void Slow(float slowPercentage)
	{
		speed = startSpeed * (1f - slowPercentage);
		count = 0;
		sprite.color = new Color (0.402f,0.789f,1,1);
	}
 

	private void Die()
	{
		Stats.Money += moneyOnDeath;

		GameObject effect = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
		effect.transform.SetParent(effectParent.transform);
		Destroy(effect, 5f);
		WaveSpawner.EnemiesAlive--;
		UserStats.incrementEnemyKills();
		Destroy(gameObject);
	}

	private void EndPath()
	{
		Stats.playerHealth -= playerDamage;
		WaveSpawner.EnemiesAlive--;
		Destroy(gameObject);
	}

	private void SearchForSoldier() //detect if a barracks soldier is in range
	{
		GameObject[] soldiers = GameObject.FindGameObjectsWithTag("Soldier");
		float shortestDistance = Mathf.Infinity; //when no soldier found, infinity will not be in range
		targetFound = false;
		foreach(GameObject soldier in soldiers)
		{
			float distanceToSoldier = Vector3.Distance (transform.position, soldier.transform.position);
			if (distanceToSoldier < shortestDistance)
			{
				shortestDistance = distanceToSoldier;
				nearestSoldier = soldier; 
			}
			if (nearestSoldier != null && shortestDistance <= (Random.Range(2,6))) //attack range of 2-6
			{
				targetFound = true;
			}
		}
		if (targetFound == false)
		{
			engangedWithSoldier = false;
			return;
		}
		else //when a soldier is in range
		{
			engangedWithSoldier = true;
			DamageSoldier(nearestSoldier.transform);

		}
	}

	private void DamageSoldier(Transform soldier)
	{
		GameObject effect = (GameObject)Instantiate(hitEffect, soldier.position, Quaternion.identity);
		effect.transform.SetParent(effectParent.transform);

		Destroy(effect, 5f);
		SoldierProjectile soldier_ = soldier.GetComponent<SoldierProjectile>();
		soldier_.TakeDamage(playerDamage * 2);
	}
}

