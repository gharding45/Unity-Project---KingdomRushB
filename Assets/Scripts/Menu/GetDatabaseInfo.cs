using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class GetDatabaseInfo : MonoBehaviour
{
    public void getData(string url, System.Action<string> callback) 
    {
        StartCoroutine(GetRequest(url,callback));     
        
    }
	private IEnumerator GetRequest(string url,System.Action<string> callback)
	{	
    	UnityWebRequest request = UnityWebRequest.Get(url);
    	yield return request.SendWebRequest();
    	if(request.isNetworkError || request.isHttpError)
    	{
    		Debug.LogError(request.error + "  ...Cannot retrieve database information");
    	}
    	else
    	{
    		callback(request.downloadHandler.text);
    	}
	}



	public IEnumerator UploadUsername(string url, string username)
    {
    	WWWForm form = new WWWForm();
    	form.AddField("usernamePost",username);
    	UnityWebRequest request = UnityWebRequest.Post(url, form);
    	yield return request.SendWebRequest();
    	if (request.isNetworkError || request.isHttpError)
    	{
    		Debug.LogError(request.error + "  ...Cannot send form");
    	}
    }

}
