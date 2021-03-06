﻿using UnityEngine;
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

    public void Awake()
    {
        base.Awake();
    }
    public static void CanBlink(){
		m_canBlink = true;
	}

	public override void setTextLost (){
	}

	public override void setTextFound(){
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

            bool m_mouthOpen = false;
            m_mouthOpen = faces[0].Expressions[Expressions.MouthOpen] > TutorialListener.c_mouthOpenThreshold;
            if (m_mouthOpen)
            {
                Debug.Log("Spawn");
                TutorialEventSystem.playerOpenMouth();


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
}