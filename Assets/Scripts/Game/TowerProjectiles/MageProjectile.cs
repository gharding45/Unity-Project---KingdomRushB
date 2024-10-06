using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class MageProjectile : MonoBehaviour
{
	private GameObject enemy;
	public GameObject hitEffect;
	private int damageAmount;
	private Transform target;
	private string towerType = "WizardTower";
	private GameObject effectParent;
	private GameObject projectileParent;


	public static void Create(Vector3 spawnPosition, GameObject enemy, int damageAmount)
	{
		Transform mageTransform = Instantiate(GameAssets.i.mageProjectilePrefab, spawnPosition, Quaternion.identity);
		MageProjectile mageProjectile = mageTransform.GetComponent<MageProjectile>();
		mageProjectile.Setup(enemy, damageAmount);
	}


	private void Setup(GameObject enemy, int damageAmount)
	{
		this.enemy = enemy;
		this.damageAmount = damageAmount;
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
