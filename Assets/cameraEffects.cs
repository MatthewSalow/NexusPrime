using UnityEngine;
using System.Collections;

public class cameraEffects : MonoBehaviour {

	public float pullBackAmount;
	public float pullBackFovAmount;
	public float camShakeAmount;
	public float particlesAmount;
	
	public float pitchLagAmount;
	public float rollLagAmount;
	public float yawLagAmount;

	private Camera cam = null;
	private Controls controls = null;
	private ParticleSystem particles = null;
	
	private Vector3 camHome;
	private float fovHome;
	private Quaternion camRotHome;

	// Use this for initialization
	void Start () {
		camHome = gameObject.transform.localPosition;
		fovHome = camera.fieldOfView;
		camRotHome = gameObject.transform.localRotation;
	
		cam = gameObject.GetComponent<Camera>();
		controls = gameObject.GetComponentInParent<Controls>();
		particles = gameObject.GetComponentInChildren<ParticleSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		//ratios
		float thrustRatio = controls.throttle.x / controls.throttle.y;
		float rollRatio = controls.roll.x / controls.roll.y;
		float pitchRatio = controls.pitch.x / controls.pitch.y;
		float yawRatio = controls.yaw.x / controls.yaw.y;
		float thrustPow5 = Mathf.Pow(thrustRatio, 5);
		
		//pull back
		gameObject.transform.localPosition = camHome - new Vector3(thrustPow5 * pullBackAmount, 0, 0);
		
		//fov
		float pull = fovHome - pullBackFovAmount * thrustRatio;
		float shake = thrustRatio * thrustRatio * camShakeAmount;
		cam.fieldOfView = Random.Range(pull - shake, pull + shake);
		
		//particles
		particles.emissionRate = particlesAmount * thrustRatio;
		
		//roll/pitch/yaw lag
		Quaternion addToRoll = Quaternion.AngleAxis(rollRatio * -rollLagAmount, new Vector3(0, 0, 1));
		Quaternion addToPitch = Quaternion.AngleAxis(pitchRatio * -pitchLagAmount, new Vector3(1, 0, 0));
		Quaternion addToYaw = Quaternion.AngleAxis(yawRatio * -yawLagAmount, new Vector3(0, 1, 0));
		
		gameObject.transform.localRotation = camRotHome * addToRoll * addToPitch * addToYaw;
	}
}
