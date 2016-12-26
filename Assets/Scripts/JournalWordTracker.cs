using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;

public class JournalWordTracker : ImageResultsListener
{ 
	private float m_lastEyes;
	private static bool m_canBlink = false;
	private const float c_faceBuffer = 1.0f;

	private bool m_blinking = false;
	private const float c_blinkTime = 1.0f;

	public static void CanBlink(){
		m_canBlink = true;
	}

	public override void setTextLost (){
		m_text.text = "Move your face into the camera";
	}
	public override void setTextFound(){
		m_text.text = "";
	}
	public override void onImageResults(Dictionary<int, Face> faces)
	{
		if(faces.Count > 0)
		{
			ImageResultsListener.ReportSample (faces);
			DebugFeatureViewer dfv = GameObject.FindObjectOfType<DebugFeatureViewer>();

			if (dfv != null)
				dfv.ShowFace(faces[0]);

			if (m_lastEyes - c_faceBuffer > faces [0].Expressions [Affdex.Expressions.EyeClosure]) {

				if (m_canBlink == true) {
					WordDecay.TellToErase ();
					FadeInFadeOut.AuthorizeFade ();
					StartCoroutine (Blink ());
				}
			}


			m_lastEyes = faces [0].Expressions [Affdex.Expressions.EyeClosure];
		}
	}


	private IEnumerator Blink(){
		Debug.Log ("Blinking");
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