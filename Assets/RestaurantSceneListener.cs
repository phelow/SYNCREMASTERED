using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;
public class RestaurantSceneListener : ImageResultsListener {

	private float m_lastEyes;
	private const float c_faceBuffer = 1.0f;
	public static bool m_blinking = false;
	public static RestaurantSceneListener ms_instance;

	private const float c_blinkTime = 1.0f;

	public static float ms_attention;
	private static float m_disgustLevel;
	private const float c_disgustThreshold = 5.0f;
	private static float m_joyLevel;
	private const float c_joyThreshold = 50.0f;
	private static float m_terrorLevel;
	private const float c_terrorThreshold = 10.0f;


	[SerializeField]private Animator m_blinkerAnimator;

	public override void setTextLost (){
	}
	public override void setTextFound(){
	}

	public static bool IsJoyful(){
		return m_joyLevel > c_joyThreshold;
	}


	public static bool IsTerrified(){
		return m_terrorLevel > c_terrorThreshold;
	}

	public static bool Disgusted(){
		return m_disgustLevel > c_disgustThreshold;
	}

	public override void onImageResults(Dictionary<int, Face> faces)
	{
		if (faces.Count > 0) {
			ReportSample (faces);
			SetInstructionSprite.FaceDetected ();
			DebugFeatureViewer dfv = GameObject.FindObjectOfType<DebugFeatureViewer> ();

			if (dfv != null)
				dfv.ShowFace (faces [0]);

			if (m_lastEyes - c_faceBuffer > faces [0].Expressions [Affdex.Expressions.EyeClosure]) {
				FadeInFadeOut.AuthorizeFade ();
				StartCoroutine (Blink ());
			}
			m_disgustLevel = faces [0].Emotions [Affdex.Emotions.Disgust];
			m_joyLevel = faces [0].Emotions [Affdex.Emotions.Joy];
			m_terrorLevel = faces [0].Emotions [Affdex.Emotions.Fear];

			m_lastEyes = faces [0].Expressions [Affdex.Expressions.EyeClosure];

			ms_attention = 100 -faces [0].Emotions [Affdex.Emotions.Engagement];
		} else {
			SetInstructionSprite.FaceUndetected ();
		}


	}

	private IEnumerator Blink(){
		m_blinking = true;
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
