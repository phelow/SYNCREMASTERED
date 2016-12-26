using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;

public class SceneZeroListener : ImageResultsListener
{ 
	public static float s_smileMultiplier;
	public static bool s_emotionalIntensityValid;

	private float lastEyes;

	private const float c_smileThreshold = 10.0f;
	private const float c_faceBuffer = 5.0f;




	public override void setTextLost (){
		Debug.Log ("Line 21, setting lost text");
		m_text.text = "Move your face into the camera";
		SetInstructionSprite.FaceUndetected ();
	}
	public override void setTextFound(){
		m_text.text = "";
		SetInstructionSprite.FaceDetected ();
	}


	public override void onImageResults(Dictionary<int, Face> faces)
	{

		ReportSample (faces);
		if (faces.Count > 0) {
			Debug.Log (35 + "faces [0].Emotions[Emotions.Joy]:"+faces [0].Emotions[Emotions.Joy]);

			DebugFeatureViewer dfv = GameObject.FindObjectOfType<DebugFeatureViewer> ();

			if (dfv != null)
				dfv.ShowFace (faces [0]);

			if (faces [0].Emotions[Emotions.Joy] > c_smileThreshold || faces [0].Emotions[Emotions.Disgust] > c_smileThreshold || faces [0].Emotions[Emotions.Sadness] * 100 > c_smileThreshold|| faces [0].Emotions[Emotions.Anger] > c_smileThreshold  || faces [0].Emotions[Emotions.Surprise] > c_smileThreshold ) {
				s_emotionalIntensityValid = true;
			}

			if (s_emotionalIntensityValid) {
				s_smileMultiplier = .05f + Random.Range (80, 100) / 105.0f;

			} else {
				Debug.Log (faces [0].Emotions [Emotions.Joy]);
			}

			Debug.Log (s_smileMultiplier);
			if (lastEyes - c_faceBuffer > faces [0].Expressions [Affdex.Expressions.EyeClosure]) {
				FadeInFadeOut.AuthorizeFade ();
			}

			if (faces [0].Emotions[Emotions.Joy] > c_smileThreshold ) {
				SmileToAdvanceScene.SmileAtLover ();
			} else {
			}


			lastEyes = faces [0].Expressions [Affdex.Expressions.EyeClosure];
		}
	}

	public static void SetNotEmotionallySufficient(){
		s_emotionalIntensityValid = false;
	}

	public static bool IsEmotionalIntensitySufficient(){
		return s_emotionalIntensityValid;
	}


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}