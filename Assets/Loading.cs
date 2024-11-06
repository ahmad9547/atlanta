using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loading : MonoBehaviour
{
	public GameObject loading;

	public static Loading Instance;

	private void Awake()
	{
		Instance = this;
	}
}