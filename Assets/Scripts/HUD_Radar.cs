/////////////////////////////////////////////////////////
// Author(s): Jake Castillo
//
// Purpose: Creates the radar effect on the HUD using 
//          distances and half space tests to figure out 
//			the location of enemies.
/////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;

public class HUD_Radar : MonoBehaviour
{
	public GameObject player;
	public float MaxDistance;
	private GameObject[] enemyList;
	private uint listUpdateTimer;
	
	// Use this for initialization
	void Start ()
	{
		//enemyList = nullptr;
		listUpdateTimer = 20;
		MaxDistance = 150.0f;
	}
	
	// Update is called once per frame
	void Update ()
	{
		listUpdateTimer++;
		if (listUpdateTimer >= 20) 
		{
			enemyList = GameObject.FindGameObjectsWithTag("Enemy");
			listUpdateTimer = 0;
		}

		for (int i = 0; i < enemyList.Length; i++) 
		{
			float distance = Vector3.Dot(player.transform.position, enemyList[i].transform.position);
			if(distance > MaxDistance)
				continue;
		}
	}
}