using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelUnlock : MonoBehaviour
{
	public List<Button> levelButtons = new List<Button>();
	private int levelsUnlocked;
	private int unlockCount;

	void Start()
	{
		for (int i = 0; i < levelButtons.Count; i++)
		{
			levelButtons[i].interactable = false; //prevent the button from being clicked
		}
		unlockCount = 0;
	}

	void Update()
	{
		levelsUnlocked = UserStats.getHighestLevel();
		if (unlockCount != levelsUnlocked) //when the user has changed
		{
			for (int i = 0; i < levelButtons.Count; i++)
			{
				levelButtons[i].interactable = false; //prevent the button from being clicked
			}
			for (int i = 0; i < levelsUnlocked; i++)
			{
				levelButtons[i].interactable = true;
			}
			unlockCount = levelsUnlocked;
		}
	}
}
