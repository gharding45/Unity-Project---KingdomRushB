using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DisplayUserStats : MonoBehaviour
{
	public Text usernameText;	
	public Text gamesPlayedText;
	public Text gamesWonText;
	public Text enemyKillsText;
	private string username;
	private string url = "http://localhost:8080/KingdomRushB_Accounts/UserStats.php"; 
	private GetDatabaseInfo getDatabaseInfo;

	void Start()
	{
		getDatabaseInfo = GameObject.FindGameObjectWithTag("DatabaseInfo").GetComponent<GetDatabaseInfo>();
	}

    void Update()
    {
    	if (SceneManager.sceneCount != 1) //game started
    	{
    		return;
    	}

    	username = UserStats.getUser();
    	string urlWithUsername = (url + "?usernamePost=" + username);
    	StartCoroutine(getDatabaseInfo.UploadUsername(urlWithUsername,username)); 
    	getDatabaseInfo.getData(urlWithUsername, (string data) =>
		{

			int levelsUnlocked = GetAttribute(data, "LevelUnlock");
			int totalGamesPlayed = GetAttribute(data, "GamesPlayed");
			int totalGamesWon = GetAttribute(data, "GamesWon");
			int totalEnemyKills = GetAttribute(data, "EnemyKills");
			UserStats.setHighestLevel(levelsUnlocked);


			//display text to user
			usernameText.text = username; 
			gamesPlayedText.text = Convert.ToString(totalGamesPlayed);
			gamesWonText.text = Convert.ToString(totalGamesWon);
			enemyKillsText.text = Convert.ToString(totalEnemyKills);
		});



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

