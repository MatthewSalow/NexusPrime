using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ReticleMoveScript : MonoBehaviour {

	private GameObject playerShip;
	private Controls controls;
	private RectTransform rect;
	private RectTransform hashRect;
	private Image hashImage;
	
	public float maxX;
	public float maxY;
	
	private Quaternion rotHome;
	

	// Use this for initialization
	void Start () {
		playerShip = GameObject.FindGameObjectWithTag("Player");
		controls = playerShip.GetComponent<Controls>();
		rect = gameObject.GetComponent<RectTransform>();
				
		hashImage = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
		hashRect = gameObject.transform.GetChild(0).gameObject.GetComponent<RectTransform>();
		
		rotHome = hashRect.localRotation;
	}
	
	// Update is called once per frame
	void Update () {
		float yawRatio = -controls.roll.x / controls.roll.y;
		float pitchRatio = controls.pitch.x / controls.pitch.y;
		
		Vector3 recticlePos =  new Vector3( yawRatio * maxX, pitchRatio * maxY, 0.0f );
		
		rect.localPosition = recticlePos;
		
		float disRatio = (rect.localPosition.magnitude - 30.0f) / (new Vector3(maxX, maxY, 0.0f).magnitude);
		hashImage.fillAmount = disRatio;
		hashRect.localRotation = rotHome;
		if(pitchRatio < 0.0f)
			hashRect.Rotate(0.0f, 0.0f, Vector3.Angle(recticlePos, new Vector3(-1.0f, 0.0f, 0.0f)));
		else
			hashRect.Rotate(0.0f, 0.0f, Vector3.Angle(recticlePos, new Vector3(1.0f, 0.0f, 0.0f)) + 180.0f);
	}
}
