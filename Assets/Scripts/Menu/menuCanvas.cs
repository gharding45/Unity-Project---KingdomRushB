using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class menuCanvas : MonoBehaviour
{

	public GameObject menuCanvas_;
	public GameObject menuCamera;

	void Update()
	{
		if (SceneManager.sceneCount != 1)
		{
			menuCanvas_.SetActive(false);
			menuCamera.SetActive(false);
		}
		else
		{
			menuCanvas_.SetActive(true);
			menuCamera.SetActive(true);
		}
	}
}
