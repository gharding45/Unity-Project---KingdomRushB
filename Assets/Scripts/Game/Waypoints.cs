using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoints : MonoBehaviour
{
	public List<Transform> waypoints; //list for every node enemies must pass through 
	public Transform startPos;
	private Vector3 eastPos;
	private Vector3 westPos;
	private Vector3 northPos;
	private Vector3 southPos;

	void Awake()
	{
		FindPathway(startPos);
	}

	private void FindPathway(Transform currentPos) //Every map has a single linear path, determined by which Nodes have the tag "PathNode" 
	{
		Transform target = null;

		eastPos = currentPos.position + new Vector3 (10f,0,0);
		westPos = currentPos.position + new Vector3 (-10f,0,0);
		northPos = currentPos.position + new Vector3 (0,10f,0);
		southPos = currentPos.position + new Vector3 (0,-10f,0);

		Vector3[] directionsPos = {eastPos,westPos,northPos,southPos};

		foreach (Vector3 item in directionsPos)
		{
			target = FindNode(item); 
			if (target != null)
				{
				FindPathway(target);
				return;
			}
		}


	}

	private Transform FindNode(Vector3 tempPos)
	{
		Collider[] hitGameObjects = Physics.OverlapSphere(tempPos, 2); //small radius to avoid containing multiple nodes
		foreach (Collider collider in hitGameObjects)
		{
			if (collider.tag == "EndPos")
			{
				waypoints.Add(collider.transform);
				return null;
			}
			if (collider.tag == "PathNode" && !waypoints.Contains(collider.transform)) 
			{
				waypoints.Add(collider.transform);
				return collider.transform;

			}
			else
			{
				return null;
			}
		}
		return null;

	}

}



