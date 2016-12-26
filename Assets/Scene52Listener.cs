using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;

public class Scene52Listener : ImageResultsListener {
	public static float s_smileMultiplier;
	public static bool s_smileValid;

	private float lastEyes;

	private const float c_smileThreshold = 10.0f;
	private const float c_faceBuffer = 5.0f;

	public override void setTextLost (){
		Debug.Log ("Line 21, setting lost text");
		SetInstructionSprite.FaceUndetected ();
	}
	public override void setTextFound(){
		SetInstructionSprite.FaceDetected ();
	}


	public override void onImageResults(Dictionary<int, Face> faces)
	{
		if(faces.Count > 0)
		{
			ReportSample (faces);
			DebugFeatureViewer dfv = GameObject.FindObjectOfType<DebugFeatureViewer>();

			if (dfv != null)
				dfv.ShowFace(faces[0]);

			s_smileMultiplier = .05f + faces [0].Expressions [Affdex.Expressions.Smile] / 105.0f;
			s_smileValid = faces [0].Expressions [Affdex.Expressions.Smile] > c_smileThreshold;

			if (lastEyes - c_faceBuffer > faces [0].Expressions [Affdex.Expressions.EyeClosure]) {
				FadeInFadeOut.AuthorizeFade ();
			}


			lastEyes = faces [0].Expressions [Affdex.Expressions.EyeClosure];
		}

	}

	public static bool IsSmileSufficient(){
		return s_smileValid;
	}


	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}
}
