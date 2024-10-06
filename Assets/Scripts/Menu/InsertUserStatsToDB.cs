using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InsertUserStatsToDB : MonoBehaviour
{
	private bool gamePlayed = false;
	private bool gameWon = false;
	private int enemyKills = 0;
	private int completedLevel;
	private string url = "http://localhost:8080/KingdomRushB_Accounts/UserStats.php"; 
	private string url2 = "http://localhost:8080/KingdomRushB_Accounts/UserStatsUpdate.php";

	private GetDatabaseInfo getDatabaseInfo;
	private InsertDatabaseInfo insertDatabaseInfo;

	void Start()
	{
		getDatabaseInfo = GameObject.FindGameObjectWithTag("DatabaseInfo").GetComponent<GetDatabaseInfo>();
		insertDatabaseInfo = GameObject.FindGameObjectWithTag("AddDatabaseInfo").GetComponent<InsertDatabaseInfo>();
		UserStats.setEnemyKills(0);
		UserStats.setGameWon(false);
	}

	void Update()
	{
		if (gamePlayed == false)
		{
			if (SceneManager.sceneCount != 1) //game started
			{
				gamePlayed = true; 
			} 
		}

		else
		{
			if (SceneManager.sceneCount == 1) //game finished
			{
				gamePlayed = false;

				enemyKills = UserStats.getEnemyKills();
				gameWon = UserStats.getGameWon();

				string username = UserStats.getUser();
				string urlWithUsername = (url + "?usernamePost=" + username);

				//StartCoroutine(getDatabaseInfo.UploadUsername(urlWithUsername,username)); //

				getDatabaseInfo.getData(urlWithUsername, (string data) =>
	    		{

	    			int levelsUnlocked = GetAttribute(data, "LevelUnlock");
	    			int totalGamesPlayed = GetAttribute(data, "GamesPlayed");
	    			int totalGamesWon = GetAttribute(data, "GamesWon");
	    			int totalEnemyKills = GetAttribute(data, "EnemyKills");

	    			if (gameWon == true)
	    			{
	    				totalGamesWon++;
	    				completedLevel = UserStats.getCurrentLevel();
	    				if (completedLevel >= levelsUnlocked && completedLevel < 5) //(there are 5 levels) 
	    				{
	    					levelsUnlocked = completedLevel + 1; //Updates the user's highest unlocked level
	    				}
	    			}


	    			totalGamesPlayed++;
	    			totalEnemyKills += enemyKills;

					StartCoroutine(insertDatabaseInfo.UploadUserStats(url2,username,levelsUnlocked, totalGamesPlayed, totalGamesWon, totalEnemyKills));

	    		});



			


				UserStats.setEnemyKills(0);
				UserStats.setGameWon(false);
			}

		}
	}
	public int GetAttribute(string data, string name)
	{
		string value = data.Substring(data.IndexOf(name)+(name.Length+1)); 
		try
		{
			value = value.Remove(value.IndexOf("|"));
		}
		catch
		{
			value = value.Remove(value.IndexOf(";"));
		}
	    return Convert.ToInt32(value);
	}



}
