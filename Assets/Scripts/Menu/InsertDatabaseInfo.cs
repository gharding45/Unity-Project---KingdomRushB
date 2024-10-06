using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class InsertDatabaseInfo : MonoBehaviour
{
  public IEnumerator Upload(string url, string username, string password, string dob, string email)
  {
  	WWWForm form = new WWWForm();
  	form.AddField("usernamePost",username);
  	form.AddField("passwordPost",password);
  	form.AddField("dobPost",dob);
  	form.AddField("emailPost",email);
  	UnityWebRequest request = UnityWebRequest.Post(url, form);
  	yield return request.SendWebRequest();
  	if (request.isNetworkError || request.isHttpError)
  	{
  		Debug.LogError(request.error + "  ...Cannot send form");
  	}
  }

  public IEnumerator UploadUserStats(string url, string username, int levels, int games, int wins, int kills)
  {
    WWWForm form = new WWWForm();
    form.AddField("usernamePost",username);
    form.AddField("levelsPost",levels);
    form.AddField("gamesPost",games);
    form.AddField("winsPost",wins);
    form.AddField("killsPost",kills);
    UnityWebRequest request = UnityWebRequest.Post(url, form);
    yield return request.SendWebRequest();
    if (request.isNetworkError || request.isHttpError)
    {
      Debug.LogError(request.error + "  ...Cannot send form");
    }
  }
}
