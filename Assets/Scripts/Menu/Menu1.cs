﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu1 : MonoBehaviour
{
	public void QuitGame()
	{
		Debug.Log("QUIT!");
		Application.Quit();
	}
}
