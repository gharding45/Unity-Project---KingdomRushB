using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Node : MonoBehaviour
{
	public Color hoverColor;
	private Renderer rend;
	private Color startColor;
	private string pathTag = "PathNode";
	
	[HideInInspector]
	public GameObject turret;
	[HideInInspector]
	public TurretBlueprint turretBlueprint;
	[HideInInspector]
	public bool isUpgraded = false;


	BuildManager buildManager;

	void Start ()
	{
		rend = GetComponent<Renderer>();
		startColor = rend.material.color;
		buildManager = BuildManager.instance;
	}

	public Vector3 GetBuildPosition()
	{
		return transform.position; 
	}

	void BuildTurret(TurretBlueprint _turretBlueprint)
	{
		if (Stats.Money < _turretBlueprint.cost)
		{
			//Debug.Log("Not enough money");
			return;
		}
		Stats.Money -= _turretBlueprint.cost;
		GameObject _turret = (GameObject)Instantiate(_turretBlueprint.prefab, GetBuildPosition(), Quaternion.identity);
		turretBlueprint = _turretBlueprint;
		turret = _turret;

		//Debug.Log("Turret built! Money left:" + Stats.Money);
	}

	public void UpgradeTurret()
	{
		if (Stats.Money < turretBlueprint.upgradeCost)
		{
			//Debug.Log("Not enough money to upgrade");
			return;
		}
		Stats.Money -= turretBlueprint.upgradeCost;
		Destroy(turret); //remove old turret
		GameObject _turret = (GameObject)Instantiate(turretBlueprint.upgradedPrefab, GetBuildPosition(), Quaternion.identity); //building new turret
		turret = _turret;
		isUpgraded = true;

		//Debug.Log("Turret upgraded! Money left:" + Stats.Money);
	}

	public void SellTurret()
	{
		int sellMoney = turretBlueprint.GetSellAmount();
		if (isUpgraded == true)
			sellMoney = sellMoney * 2;
		Stats.Money += sellMoney; 
		Destroy(turret); 
		isUpgraded = false;
		turretBlueprint = null;
	}

	void OnMouseEnter ()
	{
		if (gameObject.tag == pathTag)
		{
			return;
		}
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (!buildManager.CanBuild)
		{
			return;
		}

		if (buildManager.HasMoney)
		{
			rend.material.color = hoverColor;
		}
		else
		{
			rend.material.color = Color.red;
		}
	}

	void OnMouseExit ()
	{
		rend.material.color = startColor;
	}

	void OnMouseDown ()
	{
		if (gameObject.tag == pathTag)
		{
			return;
		}
		if (EventSystem.current.IsPointerOverGameObject())
			return;

		if (turret != null)
		{
			buildManager.SelectNode(this); //open/close the shop and upgrade pop up for the tower
			return;
		}

		if (!buildManager.CanBuild)
		{
			return;
		}
		BuildTurret(buildManager.GetTurretToBuild()); //builds turret on current node

	}
}
