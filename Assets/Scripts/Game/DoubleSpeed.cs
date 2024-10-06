using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleSpeed : MonoBehaviour
{
	public void Enable()
	{
		Time.timeScale = 2f;
	}

	public void Disable()
	{
		Time.timeScale = 1f;
	}
}
