using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;


public class SnowFlakeVomitingListener : ImageResultsListener {
	private const float c_mouthOpenThreshold = 50.0f;

	private int m_flakesGenerated = 0;
	private const int c_flakesNeededToProceed = 6;

	private float lastEyes;
	private const float c_faceBuffer = 5.0f;

	private bool m_mouthOpen = false;

	private bool m_lastMouthOpen = false;

	public override void setTextLost (){
		SetInstructionSprite.FaceUndetected ();
	}
	public override void setTextFound(){
		SetInstructionSprite.FaceDetected ();
	}


	public override void onImageResults(Dictionary<int, Face> faces)
	{
		ReportSample (faces);
		if(faces.Count > 0)
		{
			DebugFeatureViewer dfv = GameObject.FindObjectOfType<DebugFeatureViewer>();

			if (dfv != null)
				dfv.ShowFace(faces[0]);
			if (lastEyes - c_faceBuffer > faces [0].Expressions [Affdex.Expressions.EyeClosure]) {
				FadeInFadeOut.AuthorizeFade ();
			}
			m_mouthOpen = faces [0].Expressions [Expressions.MouthOpen] < c_mouthOpenThreshold;
			if (m_mouthOpen && m_lastMouthOpen == false) {
				Debug.Log ("Spawn");

				SpawnSnowflakes.SpawnASnowflake ();
				m_flakesGenerated++;


			} 

			m_lastMouthOpen = m_mouthOpen;
		}

	}
}
