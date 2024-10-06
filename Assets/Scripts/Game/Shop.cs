using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
	public TurretBlueprint ArcherTower;
	public TurretBlueprint Barracks;
	public TurretBlueprint WizardTower;
	public TurretBlueprint Mortar;
	public TurretBlueprint RockThrower;
	public TurretBlueprint IceTower;
	public TurretBlueprint AirDefence;
	public TurretBlueprint InfernoTower;


	BuildManager buildManager;
	void Start ()
	{
		buildManager = BuildManager.instance;
	}

	public void Deselect ()
	{
		//Debug.Log("DESELECTED");
		buildManager.SelectTurretToBuild(null);

	}

	public void SelectArcherTower ()
	{
		//Debug.Log("Archer Tower selected!!!");
		buildManager.SelectTurretToBuild(ArcherTower);
	}
	public void SelectBarracks ()
	{
		//Debug.Log("Barracks selected!!!");
		buildManager.SelectTurretToBuild(Barracks);
	}
	public void SelectWizardTower ()
	{
		//Debug.Log("Wizard tower selected!!!");
		buildManager.SelectTurretToBuild(WizardTower);
	}
	public void SelectMortar ()
	{
		//Debug.Log("Mortar selected!!!");
		buildManager.SelectTurretToBuild(Mortar);
	}
	public void SelectRockThrower ()
	{
		//Debug.Log("Rock thrower selected!!!");
		buildManager.SelectTurretToBuild(RockThrower);
	}
	public void SelectIceTower ()
	{
		//Debug.Log("Ice tower selected!!!");
		buildManager.SelectTurretToBuild(IceTower);
	}
	public void SelectAirDefence ()
	{
		//Debug.Log("Air defence selected!!!");
		buildManager.SelectTurretToBuild(AirDefence);
	}
	public void SelectInfernoTower ()
	{
		//Debug.Log("Inferno tower selected!!!");
		buildManager.SelectTurretToBuild(InfernoTower);
	}
}

