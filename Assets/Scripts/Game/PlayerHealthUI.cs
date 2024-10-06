using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{

	public Text healthText;	

	Color redColour = new Color(1.0f, 0.176f, 0.2f);
	Color orangeColour = new Color(1.0f, 0.64f, 0.0f);
	Color yellowColour = new Color(1.0f, 0.92f, 0.016f);
	Color greenColour = new Color(0.148f, 0.836f, 0.098f);


	void Update ()
	{

		healthText = gameObject.GetComponent<Text>();
		healthText.text = "Health: " + Stats.playerHealth;

		if (Stats.playerHealth <= 5)
		{
			healthText.color = redColour;
			return;
		}
		if (Stats.playerHealth <= 10)
		{
			healthText.color = orangeColour;
			return;
		}
		if (Stats.playerHealth <= 15)
		{
			healthText.color = yellowColour;
			return;
		}
		healthText.color = greenColour;
	}

}
