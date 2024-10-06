using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{

	public static BuildManager instance; //singleton, make sure there is only 1 instance of this script, avaliabke everywhere (otherwise each node prefab will open this)
	
	void Awake ()
	{
		instance = this;
	}


	private TurretBlueprint turretToBuild;
	private Node selectedNode;

	public bool CanBuild { get { return turretToBuild != null; } }

	public bool HasMoney { get { return Stats.Money >= turretToBuild.cost; } }

	public TurretBlueprint GetTurretToBuild()
	{
		return turretToBuild;
	}

	public void SelectTurretToBuild (TurretBlueprint turret)
	{
		turretToBuild = turret;
		DeselectNode();
	}

	public TowerSelectUI towerSelectUI;

	public void DeselectNode()
	{
		selectedNode = null;
		towerSelectUI.Hide();
	}

	public void SelectNode (Node node)
	{
		if (selectedNode == node)
		{
			DeselectNode();
			return;
		}
		selectedNode = node;
		turretToBuild = null;
		towerSelectUI.SetTarget(node);



		Info1.SetActive(false); //User interface, closing all tower info and deselect buttons when upgrade/sell menu is opened
		Info2.SetActive(false);
		Info3.SetActive(false);
		Info4.SetActive(false);
		Info5.SetActive(false);
		Info6.SetActive(false);
		Info7.SetActive(false);
		Info8.SetActive(false); 
		DeselectButton1.SetActive(false);
		DeselectButton2.SetActive(false);
		DeselectButton3.SetActive(false);
		DeselectButton4.SetActive(false);
		DeselectButton5.SetActive(false);
		DeselectButton6.SetActive(false);
		DeselectButton7.SetActive(false);
		DeselectButton8.SetActive(false);
	}

	public GameObject Info1; //Used for user interface 
	public GameObject Info2;
	public GameObject Info3;
	public GameObject Info4;
	public GameObject Info5;
	public GameObject Info6;
	public GameObject Info7;
	public GameObject Info8;
	public GameObject DeselectButton1; 
	public GameObject DeselectButton2; 
	public GameObject DeselectButton3; 
	public GameObject DeselectButton4; 
	public GameObject DeselectButton5; 
	public GameObject DeselectButton6; 
	public GameObject DeselectButton7; 
	public GameObject DeselectButton8; 

}

