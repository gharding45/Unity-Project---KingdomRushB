using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TowerSelectUI : MonoBehaviour
{
	private Node targetNode;
	public TextMeshProUGUI upgradeCostText;
	public TextMeshProUGUI sellCostText;
	public Button upgradeButton;
	private int sellMoney;

	public void SetTarget(Node target)
	{
		targetNode = target;
		transform.position = targetNode.transform.position;
		transform.position += new Vector3 (0, -2, 0);
		sellMoney = target.turretBlueprint.GetSellAmount();

		if (!target.isUpgraded)
		{
			upgradeCostText.text = "£" + target.turretBlueprint.upgradeCost;
			upgradeButton.interactable = true;
		}
		else
		{
			sellMoney = sellMoney * 2;
			upgradeCostText.text = "MAXED";
			upgradeButton.interactable = false;
		}

		sellCostText.text = "£" + sellMoney;
		UI.SetActive(true);
	}

	public GameObject UI;

	public void Hide()
	{
		UI.SetActive(false);
	}

	public void Upgrade()
	{
		targetNode.UpgradeTurret();
		BuildManager.instance.DeselectNode(); //close menu
	}

	public void Sell()
	{
		targetNode.SellTurret();

		BuildManager.instance.DeselectNode(); //close menu

	}
}
