using UnityEngine;
using System.Collections;

public class fire_from : MonoBehaviour {

	//public Transform Location;//location to shoot from
	public GameObject projectile;//what im shooting
	public float fire_rate;//rate of fire
	private float next_shot;//cool down till next fire
	public Transform owner;
	//public Transform Reticle;//get the reticle Position( 2D space )
	//public Camera mainCam;

	//RayCasting
    //private Vector3 origin;
    //private Vector3 direction;
    //private Ray _ray;
    //private RaycastHit hitInfo;
    //private float dist;

    //private Vector3 fire_dest;

	// Use this for initialization
	void Start () {
		//dist = 10;
		//_ray = new Ray( transform.position, transform.forward );//Reticle.position, Vector3.forward);
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetButton ( "Fire1" )/*Input.GetMouseButton( 0 )*/ && Time.time > next_shot )
		{

			next_shot = Time.time + fire_rate;

            //if( Physics.Raycast( _ray, out hitInfo, dist) )
            //    fire_dest = hitInfo.point;//( hitInfo.point - this.transform.position).normalized;
            //else
            //    fire_dest = transform.forward * dist;

            //this.transform.LookAt( fire_dest );
			GameObject  clone = Instantiate ( projectile, this.transform.position, this.transform.rotation )as GameObject;
			clone.transform.SetParent( owner );
			Physics.IgnoreCollision ( clone.collider, owner.transform.collider );
			audio.Play ();
		
		}
	}
}
