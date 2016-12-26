using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;

public class Scene1_CityTracker : ImageResultsListener {
	private float lastEyes;
	private const float c_faceBuffer = 1.0f;


	private bool m_blinking = false;
	private const float c_blinkTime = 1.0f;

	public override void setTextLost (){
	}
	public override void setTextFound(){
	}
	public override void onImageResults(Dictionary<int, Face> faces)
	{
		if(faces.Count > 0)
		{
			ReportSample (faces);
			DebugFeatureViewer dfv = GameObject.FindObjectOfType<DebugFeatureViewer>();

			if (dfv != null)
				dfv.ShowFace(faces[0]);

			if (lastEyes - c_faceBuffer > faces [0].Expressions [Affdex.Expressions.EyeClosure]) {

				FadeInFadeOut.AuthorizeFade ();
				StartCoroutine (Blink ());
			}


			lastEyes = faces [0].Expressions [Affdex.Expressions.EyeClosure];
		}
	}

	public static void SampleFace(){

	}


	private IEnumerator Blink(){
		m_blinking = true;
		yield return new WaitForSeconds (c_blinkTime);
		m_blinking = false;
	}

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
	}
}
