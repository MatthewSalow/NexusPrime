using UnityEngine;
using System.Collections;

public class Practice_Target : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () 
	{

	}

	void OnCollisionEnter(Collision Col)
	{
		if (Col.gameObject.tag == "Projectile")
						Destroy (gameObject);
	}

}
