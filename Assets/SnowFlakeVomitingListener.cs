using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;


public class SnowFlakeVomitingListener : ImageResultsListener {
	private const float c_mouthOpenThreshold = 10.0f;

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

    float tPassed = 0.0f;
	public override void onImageResults(Dictionary<int, Face> faces)
	{
		ReportSample (faces);
		if(faces.Count > 0)
		{
			m_mouthOpen = faces [0].Expressions [Expressions.MouthOpen] > c_mouthOpenThreshold;

            if (m_mouthOpen)
            {
                tPassed += Time.deltaTime;
            }

            Debug.Log("tPassed:" + tPassed + " m_mouthOpen:" + m_mouthOpen);

            if (tPassed > .1f) {
				Debug.Log ("Spawn");

                tPassed = 0.0f;

				SpawnSnowflakes.SpawnASnowflake ();
                TutorialEventSystem.playerOpenMouth();
                m_flakesGenerated++;


			} 

			m_lastMouthOpen = m_mouthOpen;
		}

	}
}
