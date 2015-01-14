using UnityEngine;
using System.Collections;

public class laser_movement : MonoBehaviour {

	//private GameObject owner;
	public float speed;
	public float duration;// how long b4 projectile destroys it self if no collision
	//public float damage;//amount of damage reciever takes

	// Use this for initialization
	void Start () {
		rigidbody.velocity = transform.forward * speed;

		if( duration == 0 )//if not set
			duration = 2;//default to 2
		Destroy ( this.gameObject, duration );
	}
		
	void OnCollisionEnter( Collision col_with )
	{
		if( col_with.gameObject.tag == "Projectile" )
			Physics.IgnoreCollision( this.collider, col_with.collider );
	
		if( col_with.gameObject.tag != "Player" 
		   && col_with.gameObject.tag != "Projectile")
			Destroy( this.gameObject );

	}
}
