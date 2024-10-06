using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;
using TMPro;

public class ValidateRegistration : MonoBehaviour
{
	GetDatabaseInfo getDatabaseInfo;
	InsertDatabaseInfo insertDatabaseInfo;
	public InputField username;
	public InputField password;
	public InputField dob;
	public InputField email;
	public TextMeshProUGUI errorText;
	public GameObject currentMenu; //turn off this menu once finished
	public GameObject nextMenu; //switch to this menu once finished
	bool usernameValid;
	bool passwordValid;
	bool doBValid;
	bool emailValid;

	public static bool hasSpecialChar(string yourString)
	{
		foreach (var item in yourString)
		{
			if (Char.IsLetterOrDigit(item) == false)
			{
				return false;
			} 
		}
		return true;
	}

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
				password.text.Length != 0 && 
				dob.text.Length != 0 &&
				email.text.Length != 0)
			{


				//USERNAME VALIDATION
				if (hasSpecialChar(username.text) == true)
				{
					if (username.text.Length < 20)
					{
						bool duplicate = false; 
						for(int i = 0; i < ((accounts.Length)-1); i++)
						{
							if (GetDataValue(accounts[i],"Username:") == username.text)
							{
								duplicate = true;
							}

						}
						if (duplicate == false)
						{
							usernameValid = true;
						}
						else
						{
							errorText.text = "Username already taken";
						}
					}
					else
					{
						errorText.text = "Username must be under 20 characters";
					}

				}
				else
				{
					errorText.text = "Username cannot contain special characters";
				}



				//PASSWORD VALIDATION
				if (password.text.Length < 25)
				{
					passwordValid = true;
				}
				else
				{
					errorText.text = "Password must be under 25 characters";
				}



				//DATE OF BIRTH VALIDATION
				DateTime temporary;
				if (dob.text.Length == 10)
				{
					if (DateTime.TryParse(dob.text, out temporary))
					{
						try 
						{
							int year = Convert.ToInt16((dob.text).Substring((dob.text).Length -4));
							if (year > 2020 || year < 1900)
							{
								errorText.text = "Please enter a valid year of birth";
							}
							else
							{
								doBValid = true;
							}
						}
						catch
						{
							errorText.text = "Invalid date";
						}

					}
					else
					{
						errorText.text = "Invalid date";
					}
				}
				else
				{
					errorText.text = "Please use the format dd/mm/yyyy";
				}



				//EMAIL VALIDATION
				bool emailduplicate = false; 
				for(int i = 0; i < ((accounts.Length)-1); i++)
				{
					if (GetDataValue(accounts[i],"Email:") == email.text)
					{
						emailduplicate = true;
					}

				}
				if (emailduplicate == false)
				{
					if (email.text.Length < 50)
					{
						if (email.text.Contains("@") && email.text.Contains("."))
						{
							emailValid = true;
						}
						else
						{
							errorText.text = "Invalid email";
						}
					}
					else
					{
						errorText.text = "Email must be under 50 characters";
					}
				}
				else
				{
					errorText.text = "Email already taken";
				}
			}
			else
			{
				errorText.text = "Do not leave anything blank";
			}

			if (usernameValid == true &&
			passwordValid == true &&
			doBValid == true &&
			emailValid == true)
			{
				string urlInsert = "localhost:8080/KingdomRushB_Accounts/AccountsAdd.php";
				insertDatabaseInfo=GameObject.FindGameObjectWithTag("AddDatabaseInfo").GetComponent<InsertDatabaseInfo>();
				StartCoroutine(insertDatabaseInfo.Upload(urlInsert,username.text,password.text,dob.text,email.text)); //insert new user information into database

				UserStats.setUser(username.text);
				nextMenu.SetActive(true);
				currentMenu.SetActive(false);
				clear(username);
				clear(password);
				clear(dob);
				clear(email); //clear input fields 
				usernameValid = false;
				passwordValid = false;
				doBValid = false;
				emailValid = false; 
			}
		});
	}
}
