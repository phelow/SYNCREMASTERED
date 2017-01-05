using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;

public class GenericListener : ImageResultsListener
{ 
	public static float s_smileMultiplier;
	public static bool s_emotionalIntensityValid;

	private float lastEyes;

	private const float c_smileThreshold = 10.0f;
	private const float c_faceBuffer = 5.0f;




	public override void setTextLost (){
		Debug.Log ("Line 21, setting lost text");
//		m_text.text = "Move your face into the camera";
		SetInstructionSprite.FaceUndetected ();
	}
	public override void setTextFound(){
		SetInstructionSprite.FaceDetected ();
	}


	public override void onImageResults(Dictionary<int, Face> faces)
	{
		Debug.Log ("ReportSample(faces)");
		ReportSample (faces);
	
	}

	public static void SetNotEmotionallySufficient(){
		s_emotionalIntensityValid = false;
	}

	public static bool IsEmotionalIntensitySufficient(){
		return s_emotionalIntensityValid;
	}

	// Update is called once per frame
	void Update () {

	}
}