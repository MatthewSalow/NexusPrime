using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpeedUIBar : MonoBehaviour
{
	public GameObject ship;
	private Controls controls = null;
	private Image settings;
	private float perc;

		// Use this for initialization
		void Start ()
		{
			controls = ship.GetComponent<Controls> ();
			settings = gameObject.GetComponent<Image> ();
			perc = 0.0f;
		}
	
		// Update is called once per frame
		void Update ()
		{
			perc = (controls.throttle.x / controls.throttle.y);
			if(perc >= 0.0f)
			{
				settings.color = Color.white;
				settings.fillOrigin = 0;
				settings.fillAmount = perc;
			}
			else
			{
				settings.color = Color.magenta;
				settings.fillOrigin = 1;
				settings.fillAmount = -perc;
			}
		}
}

