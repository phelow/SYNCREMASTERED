using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;
public class CarFaceListener : ImageResultsListener {

	private float m_lastEyes;
	private const float c_faceBuffer = 1.0f;
	private bool m_blinking = false;
	public static CarFaceListener ms_instance;

	private const float c_blinkTime = 1.0f;

	public static float ms_attention;

	[SerializeField]private Animator m_blinkerAnimator;

	public override void setTextLost (){
		SetInstructionSprite.FaceUndetected ();
	}
	public override void setTextFound(){
		SetInstructionSprite.FaceDetected ();
	}

	public override void onImageResults(Dictionary<int, Face> faces)
	{
		ReportSample (faces);
		if (faces.Count > 0) {
			SetInstructionSprite.FaceDetected ();
			DebugFeatureViewer dfv = GameObject.FindObjectOfType<DebugFeatureViewer> ();

			if (dfv != null)
				dfv.ShowFace (faces [0]);

			if (m_lastEyes - c_faceBuffer > faces [0].Expressions [Affdex.Expressions.EyeClosure]) {
				FadeInFadeOut.AuthorizeFade ();
				StartCoroutine (Blink ());
			}
			m_lastEyes = faces [0].Expressions [Affdex.Expressions.EyeClosure];
			ms_attention = faces [0].Emotions [Affdex.Emotions.Engagement];
		} else {
			ms_attention = 0.0f;
			SetInstructionSprite.FaceUndetected ();
		}
	}

	public static void SetShouldWipeFalse(){

		ms_instance.m_blinkerAnimator.SetBool ("ShouldWipe", false);

	}

	public static void SetShouldWipe(){
		
		ms_instance.m_blinkerAnimator.SetBool ("ShouldWipe", true);

	}

	private IEnumerator Blink(){
		m_blinking = true;
		SetShouldWipe ();
		SpawnSnowflakes.StartSpawningSnowFlakes ();

		yield return new WaitForSeconds (c_blinkTime);
		m_blinking = false;
	}

	// Use this for initialization
	void Start () {
		ms_instance = this;
	}

	// Update is called once per frame
	void Update () {
	}
}
