using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UserStats //stores variables for different scenes to access
{
	private static int enemyKills, currentLevel, highestLevel;
	private static bool gameWon;
	private static string currentUser;

	public static int getEnemyKills()
	{
		return enemyKills;
	}

	public static void setEnemyKills(int kills)
	{
		enemyKills = kills;
	}

	public static void incrementEnemyKills()
	{
		enemyKills++;
	}


	public static int getCurrentLevel()
	{
		return currentLevel;
	}

	public static void setCurrentLevel(int level)
	{
		currentLevel = level;
	}

	public static bool getGameWon()
	{
		return gameWon;
	}

	public static void setGameWon(bool won)
	{
		gameWon = won;
	}

	public static string getUser()
	{
		return currentUser;
	}

	public static void setUser(string username)
	{
		currentUser = username;
	}

	public static int getHighestLevel()
	{
		return highestLevel;
	}

	public static void setHighestLevel(int level)
	{
		highestLevel = level;
	} 


}
