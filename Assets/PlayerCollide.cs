using UnityEngine;
using System.Collections;

public class PlayerCollide : MonoBehaviour {

	public bool touched = false;
	public Material firstMat, secMat, thirdMat;
	public int RingNum, CurrRing;
	public bool Next;

	// Use this for initialization
	void Start () 
	{
		if(RingNum != 1)
			gameObject.renderer.material = thirdMat;
		else
		{
			gameObject.renderer.material = firstMat;
			Next = true;
		}
	}
	
	// Update is called once per frame
	void Update () {

	
	}

	void OnTriggerEnter(Collider other)
	{
		if(touched == false)
		{
			if(other.gameObject.tag == "Player" && Next == true)
			{
				audio.Play();
				touched = true;
				gameObject.renderer.material = secMat;
				GameObject FRing = GameObject.FindGameObjectWithTag("Final Ring");
				FRing.GetComponent<PlayerCollide>().CurrRing = RingNum;
				GameObject[] obj = GameObject.FindGameObjectsWithTag("Ring");
				foreach(GameObject item in obj)
				{
					if(item.GetComponent<PlayerCollide>().RingNum == FRing.GetComponent<PlayerCollide>().CurrRing + 1)
					{
						item.renderer.material = firstMat;
						item.GetComponent<PlayerCollide>().Next = true;
					}
				}
				if(gameObject.tag == "Final Ring")
				{
					foreach(GameObject item in obj)
					{
						item.gameObject.GetComponent<PlayerCollide>().touched = false;
						item.gameObject.renderer.material = item.gameObject.GetComponent<PlayerCollide>().firstMat;
					}
					touched = false;
					gameObject.renderer.material = firstMat;
				}
			}
		}
	}
}
