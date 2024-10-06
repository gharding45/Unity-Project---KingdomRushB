using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
	public static int loadedSceneIndex;

 	public void changeScene(int sceneIndex)
 	{
 		if (sceneIndex != 0) //when on the main menu
 		{
 			loadedSceneIndex = sceneIndex;
 			UserStats.setCurrentLevel(sceneIndex);
 			SceneManager.LoadScene(sceneIndex, LoadSceneMode.Additive);

 		}
 		else //when in a level
 		{
 			WaveSpawner.EnemiesAlive = 0;
 			SceneManager.UnloadScene(loadedSceneIndex); //switches back to main menu (index 0)
 		}
 	}
}