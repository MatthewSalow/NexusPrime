using UnityEngine;
using System.Collections;

public class Shipflight : MonoBehaviour {
    public GameObject ship;
    public int speed = 1;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
        if(Input.GetKey(KeyCode.W))
        {
            ship.transform.position += ship.transform.forward*Time.deltaTime*5;
        }
        if (Input.GetKey(KeyCode.S))
        {
            ship.transform.position += -ship.transform.forward * Time.deltaTime*5;
        }
        if (Input.GetKey(KeyCode.A))
        {
            ship.transform.Translate(new Vector3(-1*Time.deltaTime,0, 0));
        }
        if (Input.GetKey(KeyCode.D))
        {
            ship.transform.Translate(new Vector3(1*Time.deltaTime, 0, 0));
        }
        //this.transform.position = ship.transform.position;

        //Do a barrel roll
        if (Input.GetKey(KeyCode.Q))
            ship.transform.Rotate(new Vector3(0, 0, 1), 250 * Time.deltaTime);

        if (Input.GetKey(KeyCode.E))
            ship.transform.Rotate(new Vector3(0, 0, 1), -250 * Time.deltaTime);

        if (Input.GetMouseButton(1))
        {
            //Pitch
            float mY = Input.GetAxis("Mouse Y");
            ship.transform.Rotate(new Vector3(1, 0, 0), -mY*3);
            //Yaw
            float mX = Input.GetAxis("Mouse X");
            ship.transform.Rotate(new Vector3(0, 1, 0), mX*3); 
        }
	}
}
