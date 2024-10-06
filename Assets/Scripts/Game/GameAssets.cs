using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour  //store game assets here, which can be taken from here by code 
{
	private static GameAssets _i;

    public static GameAssets i
    {
    	get 
    	{
    		if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>(); //if internal reference doesnt't exist (if first time using/switched scenes)
    		return _i;
    	}
    }

    public Transform archerProjectilePrefab;
    public Transform soldierProjectilePrefab;
    public Transform mageProjectilePrefab;
    public Transform bombProjectilePrefab;
    public Transform rockProjectilePrefab;
    public Transform iceProjectilePrefab;
    public Transform rocketProjectilePrefab;
    //public Transform beamProjectilePrefab;
}
