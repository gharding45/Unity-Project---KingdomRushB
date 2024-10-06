using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using TMPro;

public class ValidateLogin : MonoBehaviour
{
	GetDatabaseInfo getDatabaseInfo;
	public InputField username;
	public InputField password;
	public GameObject currentMenu;
	public GameObject nextMenu;
	public TextMeshProUGUI errorText;


	string GetDataValue(string data, string index) //For the database information taken from the url: Data is an item in the list, index is the attribute (such as username) you wish to return.
	{
		string value = data.Substring(data.IndexOf(index)+index.Length); //eg if "Username:" is the index, creates substring after the colon. 
		if(value.Contains("|")) //removes everything after the | which seperates different attributes 
		{

			value = value.Remove(value.IndexOf("|"));
		}
		return value; 
	}


	public static void clear(InputField inputField)
	{
		inputField.Select();
		inputField.text = "";
	}

	public void validate()
	{
		string url = "http://localhost:8080/KingdomRushB_Accounts/Accounts.php";
		//Calling subroutine in GetDatabaseInfo to retrieve the information on the above url
		getDatabaseInfo=GameObject.FindGameObjectWithTag("DatabaseInfo").GetComponent<GetDatabaseInfo>();
		getDatabaseInfo.getData(url, (string data) =>
	    {
			string[] accounts;
			accounts = data.Split(';'); //Creates 1 extra item on the end consisting of nothing 
			//Debug.Log(GetDataValue(accounts[0],"ID:"));

			if (username.text.Length != 0 &&
				password.text.Length != 0)
			{
				bool usernameInDatabase = false;
				int indexOfUser = 0;
				for(int i = 0; i < ((accounts.Length)-1); i++)
				{
					if (GetDataValue(accounts[i],"Username:") == username.text)
					{
						usernameInDatabase = true;
						indexOfUser = i;
					}
				}
				if (usernameInDatabase == true)
				{
					if (GetDataValue(accounts[indexOfUser],"Password:") == password.text)
					{
						UserStats.setUser(username.text);
						nextMenu.SetActive(true);
						currentMenu.SetActive(false);
						clear(username);
						clear(password); //clear input fields
					}
					else
					{
						errorText.text = "Incorrect password";
					}
				}
				else
				{
					errorText.text = "User does not exist"; 
				}
			}

			else
			{
				errorText.text = "Do not leave anything blank";
			}


		});

	}


   
}

