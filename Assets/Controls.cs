using UnityEngine;
using System.Collections;

public class Controls : MonoBehaviour {
	public enum ControlStyle { VELOCITY, CENTERPOINT };
	public float centerpointStyleDeadZone;
	
	public float bestTurnSpeed;
	public float bestTurnSpeedRange;
	public float minTurnRatio;
	
	public ControlStyle controlStyle;
	public bool controller;
	public bool invertedY;
	
	public Vector4 roll, pitch, yaw, strafe, throttle, upDown;
	private int throttleDirection = 0;
	private float directionChangeCool = 0.0f;
	
	public AudioSource lowHiss, highHiss, engineFadeUp, engineFadeDown, enginePulseUp, enginePulseDown;
		
	private void UpdateAccumulator(ref Vector4 input, float amount, float max)
	{
		input.x = Mathf.Max(Mathf.Min(max, input.x + amount * input.z), -max);
		
		if(Mathf.Abs(amount) <= 0.0f)
		{
			if(input.x > 0.0f)
				input.x = Mathf.Max(0.0f, input.x - input.w * Time.deltaTime);
			else if(input.x < 0.0f)
				input.x = Mathf.Min(0.0f, input.x + input.w * Time.deltaTime);
		}
	}
	
	private void UpdateCenterPoint(ref Vector4 input, float amount, float max)
	{
		input.x += amount * input.z;
		input.x = Mathf.Max(Mathf.Min(input.x, max), -max);
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
		
		float a4 = -Input.GetAxisRaw("4th Axis");
		float a5 = Input.GetAxisRaw("5th Axis");
		
		//RF
		float kZ = 0.0f;
		if(Input.GetKey(KeyCode.R)) kZ = 0.2f;
		else if(Input.GetKey(KeyCode.F)) kZ = -0.2f;
		
		//QE
		float kW = 0.0f;
		if(Input.GetKey(KeyCode.Q)) kW = -0.2f;
		else if(Input.GetKey(KeyCode.E)) kW = 0.2f;
				
		//Turn speed multiplier
		float throttleDis = Mathf.Abs(throttle.x - bestTurnSpeed);
		float currMultiplier = 1 - (throttleDis / bestTurnSpeedRange);
		currMultiplier = Mathf.Max(currMultiplier, minTurnRatio);
		
		//Ship orientation
		if(controller)
		{			
			pitch.x = currMultiplier * a5 * pitch.y * (invertedY ? -1 : 1);
			roll.x = currMultiplier * a4 * roll.y;
			
			gameObject.transform.Rotate(new Vector3(0, 1, 0), pitch.x);
			gameObject.transform.Rotate(new Vector3(1, 0, 0), roll.x);
		}
		else if(controlStyle == ControlStyle.VELOCITY)
		{			
			UpdateAccumulator(ref pitch, (mY - a5) * (invertedY ? 1 : -1), pitch.y * currMultiplier);
			UpdateAccumulator(ref roll, a4, roll.y * currMultiplier);
				
			gameObject.transform.Rotate(new Vector3(0, 1, 0), pitch.x);
			gameObject.transform.Rotate(new Vector3(1, 0, 0), roll.x);
		}
		else if(controlStyle == ControlStyle.CENTERPOINT)
		{			
			UpdateCenterPoint(ref pitch, (mY - a5) * (invertedY ? 1 : -1), pitch.y * currMultiplier);
			if(Mathf.Abs(pitch.x) > centerpointStyleDeadZone)
				gameObject.transform.Rotate(new Vector3(0, 1, 0), currMultiplier * pitch.x);
			
			float yawControlRatio = roll.x / roll.y;
			UpdateCenterPoint(ref roll, -mX, roll.y * currMultiplier);
			if(Mathf.Abs(roll.x) > centerpointStyleDeadZone)
				gameObject.transform.Rotate(new Vector3(1, 0, 0), currMultiplier * roll.x);
		}
		
		UpdateAccumulator(ref yaw, kX, currMultiplier * yaw.y);
		UpdateAccumulator(ref strafe, kW, strafe.y);
		UpdateAccumulator(ref upDown, kZ, upDown.y);
		
		//Throttle management
		UpdateAccumulator(ref throttle, kY, throttle.y);
		if(throttleDirection > 0 && throttle.x < 0.0f) throttle.x = 0.0f;
		if(throttleDirection < 0 && throttle.x > 0.0f) throttle.x = 0.0f;
		
		if(throttle.x < 0.01f && throttleDirection == 1 || throttle.x > -0.01f && throttleDirection == -1)
		{
			directionChangeCool -= Time.deltaTime;
		}
		
		if(directionChangeCool < 0.0f || throttleDirection == 0)
		{
			if(kY > 0.02f)
			{
				throttleDirection = 1;
				directionChangeCool = 1.0f;
			}
			else if(kY < -0.02f)
			{
				throttleDirection = -1;
				directionChangeCool = 1.0f;
			}
		}
		
		//Update orientation
		gameObject.transform.Rotate(new Vector3(0, 0, 1), yaw.x);
		gameObject.transform.Translate(throttle.x, (controller ? 0 : strafe.x), upDown.x);
		
		//Strafe sounds (high hiss)
		//if(kX != 0.0f || kZ != 0.0f)
		//	highHiss.Play();
		//else
		//	highHiss.Stop();
			
		//Engine sound
	}
}
