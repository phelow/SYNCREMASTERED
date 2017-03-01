using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;

public class TutorialListener : ImageResultsListener
{
    public const float c_mouthOpenThreshold = 50.0f;
    private float m_lastEyes;
    private static TutorialListener ms_instance;
    private const float c_faceBuffer = 1.0f;

    private bool m_blinking = false;
    private const float c_blinkTime = 1.0f;


    private bool m_lastMouthOpen = false;
    public bool m_faceDetected = false;

    public override void setTextLost()
    {
    }
    public override void setTextFound()
    {
    }
    public override void onImageResults(Dictionary<int, Face> faces)
    {
        if (faces.Count > 0)
        {
            m_faceDetected = true;
            ImageResultsListener.ReportSample(faces);
            DebugFeatureViewer dfv = GameObject.FindObjectOfType<DebugFeatureViewer>();

            if (dfv != null)
                dfv.ShowFace(faces[0]);

            if (m_lastEyes - c_faceBuffer > faces[0].Expressions[Affdex.Expressions.EyeClosure])
            {
                WordDecay.TellToErase();
                FadeInFadeOut.AuthorizeFade();
                StartCoroutine(Blink());
            }


            bool m_mouthOpen = false;
            m_mouthOpen = faces[0].Expressions[Expressions.MouthOpen] > c_mouthOpenThreshold;
            if (m_mouthOpen)
            {
                Debug.Log("Spawn");
                TutorialEventSystem.playerOpenMouth();


            }


            m_lastEyes = faces[0].Expressions[Affdex.Expressions.EyeClosure];
        }
        else {
            m_faceDetected = false;
        }
    }

    public static bool FaceInRange()
    {
        return ms_instance.m_faceDetected;
    }

    private IEnumerator Blink()
    {
        m_blinking = true;
        yield return new WaitForSeconds(c_blinkTime);
        m_blinking = false;
    }

    // Use this for initialization
    void Start()
    {
        ms_instance = this;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
