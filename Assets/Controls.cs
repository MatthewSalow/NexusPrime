using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {
	
	public float bestTurnSpeed;
	public float bestTurnSpeedRange;
	public float minTurnRatio;
	
	public bool controller;
	public bool invertedY;
	
	public Vector2 roll, pitch, yaw, strafe, throttle, upDown;
	private int throttleDirection = 0;
	private float directionChangeCool = 0.0f;
		
	private void UpdateAccumulator(ref Vector2 input, float amount, float cool, float max)
	{
		input.x = Mathf.Max(Mathf.Min(max, input.x + amount), -max);
		
		if(Mathf.Abs(amount) <= 0.0f)
		{
			if(input.x > 0.0f)
				input.x = Mathf.Max(0.0f, input.x - cool);
			else if(input.x < 0.0f)
				input.x = Mathf.Min(0.0f, input.x + cool);
		}
	}
	
	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		Screen.showCursor = false;
	}
	
	// Update is called once per frame
	void Update () {
		float kX = Input.GetAxis("Horizontal");
		float kY = Input.GetAxis("Vertical");
		float mX = Input.GetAxis("Mouse X");
		float mY = Input.GetAxis("Mouse Y");
		
		float a4 = Input.GetAxisRaw("4th Axis") * -0.2f;
		float a5 = Input.GetAxisRaw("5th Axis") * 0.2f;
		
		//RF
		float kZ = 0.0f;
		if(Input.GetKey(KeyCode.R)) kZ = 0.2f;
		else if(Input.GetKey(KeyCode.F)) kZ = -0.2f;
		
		//QE
		float kW = 0.0f;
		if(Input.GetKey(KeyCode.Q)) kW = 0.2f;
		else if(Input.GetKey(KeyCode.E)) kW = -0.2f;
				
		//Turn speed multiplier
		float throttleDis = Mathf.Abs(throttle.x - bestTurnSpeed);
		float currMultiplier = bestTurnSpeedRange / throttleDis;
		currMultiplier = Mathf.Max(currMultiplier, minTurnRatio);
		
		//Ship orientation
		UpdateAccumulator(ref roll, (kW + a4) / 8, 3 * Time.deltaTime, currMultiplier * roll.y);
		UpdateAccumulator(ref pitch, (mY - a5) / (invertedY ? 16 : -16), 5 * Time.deltaTime, currMultiplier * pitch.y);
		UpdateAccumulator(ref strafe, kX / 32, 0.01f * Time.deltaTime, strafe.y);
		UpdateAccumulator(ref upDown, kZ, 0.01f * Time.deltaTime, upDown.y);
		
		if(!controller)
			UpdateAccumulator(ref yaw, mX / 32, 2 * Time.deltaTime, currMultiplier * yaw.y);
		else
			UpdateAccumulator(ref yaw, kX / 32, 2 * Time.deltaTime, currMultiplier * yaw.y);
		
		
		//Throttle management
		UpdateAccumulator(ref throttle, kY / 800, 0.0f, throttle.y);
		if(throttleDirection > 0 && throttle.x < 0.0f) throttle.x = 0.0f;
		if(throttleDirection < 0 && throttle.x > 0.0f) throttle.x = 0.0f;
		
		if(throttle.x < 0.01f && throttleDirection == 1 || throttle.x > -0.01f && throttleDirection == -1)
		{
			directionChangeCool -= Time.deltaTime;
		}
		
		if(directionChangeCool < 0.0f)
		{
			if(kY > 0.02f)
			{
				throttleDirection = 1;
				directionChangeCool = 0.2f;
			}
			else if(kY < -0.02f)
			{
				throttleDirection = -1;
				directionChangeCool = 0.2f;
			}
		}
		
		//Update orientation
		gameObject.transform.Rotate(new Vector3(0, 1, 0), pitch.x);
		gameObject.transform.Rotate(new Vector3(0, 0, 1), yaw.x);
		gameObject.transform.Rotate(new Vector3(1, 0, 0), roll.x);
		gameObject.transform.Translate(throttle.x, (controller ? 0 : strafe.x), upDown.x);
	}
}
