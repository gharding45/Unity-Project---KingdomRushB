using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class BombProjectile : MonoBehaviour
{
	private GameObject enemy;
	public GameObject hitEffect;
	private int damageAmount;
	private float explosionRadius = 5;
	private Transform target;
	private Vector3 spawnPosition;
	private string towerType = "Mortar";
	private Vector3 oldPoint2;
	private GameObject effectParent;
	private GameObject projectileParent;


	public static void Create(Vector3 spawnPosition, GameObject enemy, int damageAmount)
	{
		Transform bombTransform = Instantiate(GameAssets.i.bombProjectilePrefab, spawnPosition, Quaternion.identity);
		BombProjectile bombProjectile = bombTransform.GetComponent<BombProjectile>();
		bombProjectile.Setup(spawnPosition, enemy, damageAmount);
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

	private void Update()
	{
		Vector3 point2; //point2 is the end of the curve / enemy position
		if (enemy != null)
		{
			point2  = enemy.transform.position; //if enemy is alive
		}
		else
		{
			point2 = oldPoint2; //if enemy is killed, continue to point of death
		}

		Vector3 point0 = spawnPosition; //point0 is the start of the curve
		Vector3 point1 = point0 +(point2 -point0)/2 +Vector3.up *25.0f; //point1 is used to create the curve. Multiplier increases curve height
		oldPoint2 = point2;

		if (count < 1.0f) //executes curve motion 
		{
	        count += 0.5f *Time.deltaTime; //+= 0.5f determines speed
	        Vector3 m1 = Vector3.Lerp( point0, point1, count );
	        Vector3 m2 = Vector3.Lerp( point1, point2, count );
	        transform.position = Vector3.Lerp(m1, m2, count);
	    }
	    if (count > 1.0f) //when curve is complete
	    {
	    	HitTarget();
	    }
	}

	void HitTarget()
	{
		GameObject effectIns = (GameObject)Instantiate(hitEffect, transform.position, transform.rotation); //particle effect on hit
		effectIns.transform.SetParent(effectParent.transform);

		Destroy(effectIns, 2f);

		Explode();

		Destroy(gameObject);

	}

	void Explode ()
	{
		Collider[] hitObjects = Physics.OverlapSphere(transform.position, explosionRadius);
		foreach (Collider collider in hitObjects)
		{
			if (collider.tag == "Enemy")
			{
				Damage(collider.transform);
			}
		}

	}

	void Damage(Transform enemy)
	{
		Enemy enemy_ = enemy.GetComponent<Enemy>();
		enemy_.TakeDamage(damageAmount, towerType);
	}

	void OnDrawGizmosSelected ()
	{
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(transform.position, explosionRadius);
	}
}
