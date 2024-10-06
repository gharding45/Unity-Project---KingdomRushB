using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class IceProjectile : MonoBehaviour
{
	private GameObject enemy;
	public GameObject hitEffect;
	private int damageAmount;
	private Transform target;
	private string towerType = "IceTower";
	private float slowPercentage;
	private GameObject effectParent;
	private GameObject projectileParent;



	public static void Create(Vector3 spawnPosition, GameObject enemy, int damageAmount, float slowPercentage)
	{
		Transform iceTransform = Instantiate(GameAssets.i.iceProjectilePrefab, spawnPosition, Quaternion.identity);
		IceProjectile iceProjectile = iceTransform.GetComponent<IceProjectile>();
		iceProjectile.Setup(enemy, damageAmount, slowPercentage);
	}


	private void Setup(GameObject enemy, int damageAmount, float slowPercentage)
	{
		this.enemy = enemy;
		this.damageAmount = damageAmount;
		this.slowPercentage = slowPercentage;
	}

	void Awake()
	{
		effectParent = GameObject.FindGameObjectWithTag("EffectsParent");
		projectileParent = GameObject.FindGameObjectWithTag("ProjectileParent");
		gameObject.transform.SetParent(projectileParent.transform);
	}

	private void Update()
	{
		if (enemy != null)
		{
			Vector3 targetPosition = enemy.transform.position;
			Vector3 moveDir = (targetPosition - transform.position).normalized; 
			float moveSpeed = 50f;
			transform.position += moveDir * moveSpeed * Time.deltaTime; //moving the projectile towards the target

			float angle = UtilsClass.GetAngleFromVectorFloat(moveDir); 
			transform.eulerAngles = new Vector3(0, 0, angle); //rotate sprite to face right direction

			float destroySelfDistance = 1f;
			if (Vector3.Distance(transform.position, targetPosition) < destroySelfDistance)
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
		
		DamageAndSlow(enemy.transform);

		Destroy(gameObject);
		//Enemy.TakeDamage(damageAmount);

	}

	void DamageAndSlow(Transform enemy)
	{
		Enemy enemy_ = enemy.GetComponent<Enemy>();
		enemy_.TakeDamage(damageAmount, towerType);
		enemy_.Slow(slowPercentage); //slow enemy 
	}
}
