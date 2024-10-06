using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class RocketProjectile : MonoBehaviour
{
	private GameObject enemy;
	public GameObject hitEffect;
	private int damageAmount;
	private Vector3 spawnPosition;
	private string towerType = "AirDefence";
	private GameObject effectParent;
	private GameObject projectileParent;

	public static void Create(Vector3 spawnPosition, GameObject enemy, int damageAmount)
	{
		Transform rocketTransform = Instantiate(GameAssets.i.rocketProjectilePrefab, spawnPosition, Quaternion.identity);
		RocketProjectile rocketProjectile = rocketTransform.GetComponent<RocketProjectile>();
		rocketProjectile.Setup(spawnPosition, enemy, damageAmount);
	}


	private void Setup(Vector3 spawnPosition, GameObject enemy, int damageAmount)
	{
		this.spawnPosition = spawnPosition;
		this.enemy = enemy;
		this.damageAmount = damageAmount;
	}

	void Awake()
	{
		effectParent = GameObject.FindGameObjectWithTag("EffectsParent");
		projectileParent = GameObject.FindGameObjectWithTag("ProjectileParent");
		gameObject.transform.SetParent(projectileParent.transform);
	}

	float count = 0.0f;
	Vector3 previousPos;

	private void Update()
	{
		if (enemy != null)
		{
			previousPos = transform.position;
			Vector3 point0 = spawnPosition; //start of curve
			Vector3 point2 = enemy.transform.position; //end of curve
			Vector3 point1 = point0 +(point2 -point0)/2 +Vector3.up *15.0f; //point used to create curve. Multiplier increases curve height

    		if (count < 1.0f) //executes curve motion 
    		{
		        count += 0.7f *Time.deltaTime; //+= 0.5f determines speed
		        Vector3 m1 = Vector3.Lerp( point0, point1, count );
		        Vector3 m2 = Vector3.Lerp( point1, point2, count );


		        transform.position = Vector3.Lerp(m1, m2, count); //movement 


		        Vector3 dir = (transform.position - previousPos).normalized; //face direction of movement:
 				float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;          
 				transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

		    }
		    if (count > 1.0f) //when curve is complete
		    {
		    	HitTarget();
		    }
		}
		else
		{
			Destroy(gameObject);
		}
	}

	void HitTarget()
	{
		GameObject effectIns = (GameObject)Instantiate(hitEffect, transform.position, transform.rotation); //particle effect on hit
		effectIns.transform.SetParent(effectParent.transform);
		Destroy(effectIns, 2f);

		Damage(enemy.transform);

		Destroy(gameObject);
		//Enemy.TakeDamage(damageAmount);

	}

	void Damage(Transform enemy)
	{
		Enemy enemy_ = enemy.GetComponent<Enemy>();
		enemy_.TakeDamage(damageAmount, towerType);
	}
}