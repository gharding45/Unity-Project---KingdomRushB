using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Pause : MonoBehaviour
{

	public GameObject pauseUI;
	public GameObject fastforwardButton; //disable the fast forward UI when pausing

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			TogglePauseMenu();
		}
	}

	public void TogglePauseMenu()
	{
		if (GameManagement.gameOver)
            return;

		pauseUI.SetActive(!pauseUI.activeSelf); //If pause UI is active, disable it. If it is disabled, activate it.
		if (pauseUI.activeSelf)
		{
			Time.timeScale = 0f; //game movement stops (0 still, 1 normal, 2 x2)
			fastforwardButton.SetActive(false);
		}
		else
		{
			Time.timeScale = 1f;
		}
	}

	public void Menu()
	{
		TogglePauseMenu();
	}
}
