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
	
	public Vector2 roll, pitch, yaw, strafe, throttle, upDown;
	private int throttleDirection = 0;
	private float directionChangeCool = 0.0f;
	
	public AudioSource lowHiss, highHiss, engineFadeUp, engineFadeDown, enginePulseUp, enginePulseDown;
		
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
	
	private void UpdateCenterPoint(ref Vector2 input, float amount, float max)
	{
		input.x += amount;
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
		float currMultiplier = 1 - (throttleDis / bestTurnSpeedRange);
		currMultiplier = Mathf.Max(currMultiplier, minTurnRatio);
		
		//Ship orientation
		if(controller)
		{
			UpdateAccumulator(ref pitch, (mY - a5) / (invertedY ? 8 : -8), 5 * Time.deltaTime, currMultiplier * pitch.y);
			UpdateAccumulator(ref yaw, kX / 32, 8 * Time.deltaTime, currMultiplier * yaw.y);
			
			gameObject.transform.Rotate(new Vector3(0, 1, 0), pitch.x);
			gameObject.transform.Rotate(new Vector3(0, 0, 1), yaw.x);
		}
		else if(controlStyle == ControlStyle.VELOCITY)
		{
			UpdateAccumulator(ref pitch, (mY - a5) / (invertedY ? 8 : -8), 5 * Time.deltaTime, currMultiplier * pitch.y);
			UpdateAccumulator(ref yaw, mX / 32, 8 * Time.deltaTime, currMultiplier * yaw.y);
				
			gameObject.transform.Rotate(new Vector3(0, 1, 0), pitch.x);
			gameObject.transform.Rotate(new Vector3(0, 0, 1), yaw.x);
		}
		else if(controlStyle == ControlStyle.CENTERPOINT)
		{
			UpdateCenterPoint(ref pitch, mY / (invertedY ? 8 : -8), currMultiplier * pitch.y);
			UpdateCenterPoint(ref yaw, mX / 32, currMultiplier * yaw.y);
			
			if(Mathf.Abs(pitch.x) > centerpointStyleDeadZone)
				gameObject.transform.Rotate(new Vector3(0, 1, 0), currMultiplier * pitch.x);
			
			if(Mathf.Abs(yaw.x) > centerpointStyleDeadZone)
				gameObject.transform.Rotate(new Vector3(0, 0, 1), currMultiplier * yaw.x);
		}
		
		int rollFaster = 1;
		if(roll.x > 0.0f && (kW + a4) < 0.0f || roll.x < 0.0f && (kW + a4) > 0.0f)
			rollFaster = 2;
		
		UpdateAccumulator(ref roll, rollFaster * (kW + a4) / 4, rollFaster * 8 * Time.deltaTime, currMultiplier * roll.y);
		UpdateAccumulator(ref strafe, kX / 32, 0.01f * Time.deltaTime, strafe.y);
		UpdateAccumulator(ref upDown, kZ, 0.01f * Time.deltaTime, upDown.y);
		
		//Throttle management
		UpdateAccumulator(ref throttle, kY / 800, 0.0f, throttle.y);
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
		gameObject.transform.Rotate(new Vector3(1, 0, 0), roll.x);
		gameObject.transform.Translate(throttle.x, (controller ? 0 : strafe.x), upDown.x);
		
		//Strafe sounds (high hiss)
		//if(kX != 0.0f || kZ != 0.0f)
		//	highHiss.Play();
		//else
		//	highHiss.Stop();
			
		//Engine sound
	}
}
