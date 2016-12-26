using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

	void TouchScreen(){
		FadeInFadeOut.FadeOut ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0) || Input.GetMouseButtonDown (1) || Input.GetMouseButtonDown (2) || Input.touches.Length > 0) {
			TouchScreen ();
		}
	}
}
