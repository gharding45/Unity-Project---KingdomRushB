using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
	public static int Money;
	public int startMoney; //set start money for level in inspector
	
	public static int playerHealth;
	private int startPlayerHealth = 25;

	void Start ()
	{
		Money = startMoney;
		playerHealth = startPlayerHealth;
	}

}
