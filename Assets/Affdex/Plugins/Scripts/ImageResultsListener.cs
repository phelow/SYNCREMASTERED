using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Affdex
{
    /// <summary>
    /// Class that contains callback methods for asset results
    /// </summary>
    public abstract class ImageResultsListener : MonoBehaviour
    {
        private const float c_sampleTime = 3.0f;

        private const float c_sadnessModifier = 10.0f;

        private float m_joyThreshold = 50.0f;
        private float m_angerThreshold = 1.0f;
        private float m_surpriseThreshold = 50.0f;

        private static int m_samples = 0;
        private static float m_totalJoy = 0.0f;
        private static float m_totalDisgust = 0.0f;
        private static float m_totalAnger = 0.0f;
        private static float m_totalSadness = 0.0f;
        private static float m_totalSurprise = 0.0f;

        private static float ms_surpriseMultiplier = 1.0f;
        private static float ms_angerMultiplier = 10.0f;
        private static float ms_joyMultiplier = 1.0f;

        /// <summary>
        /// Indicates image results are available.
        /// </summary>
        /// <param name="faces">The faces.</param>
        public abstract void onImageResults(Dictionary<int, Face> faces);

        public static ImageResultsListener ms_instance;
        protected static bool s_faceLocated;
        [SerializeField]
        protected Text m_text;

        private static SetInstructionSprite.FaceState s_dominantFaceState;

        private IEnumerator samplerRoutine;

        public void Awake()
        {
            ms_instance = this;
        }

        public static void SetEmotionDeciding()
        {
            SetInstructionSprite.SetEmotionDeciding();
        }

        public static void SetDominantEmotion()
        {
            if (m_totalJoy * ms_joyMultiplier > m_totalAnger * ms_angerMultiplier && ms_joyMultiplier * m_totalJoy > ms_surpriseMultiplier * m_totalSurprise)
            {
                s_dominantFaceState = SetInstructionSprite.FaceState.JOY;
                SetInstructionSprite.SetFaceState(SetInstructionSprite.FaceState.JOY);
            }
            else if (ms_angerMultiplier * m_totalAnger > ms_surpriseMultiplier * m_totalSurprise)
            {
                s_dominantFaceState = SetInstructionSprite.FaceState.ANGER;
                SetInstructionSprite.SetFaceState(SetInstructionSprite.FaceState.ANGER);
            }
            else {
                s_dominantFaceState = SetInstructionSprite.FaceState.SURPRISE;
                SetInstructionSprite.SetFaceState(SetInstructionSprite.FaceState.SURPRISE);
            }
        }

        private float GetAveragedJoy()
        {
            return (m_totalJoy / m_samples) * ms_joyMultiplier;
        }

        private float GetAveragedAnger()
        {
            return (m_totalAnger / m_samples) * ms_angerMultiplier;
        }

        private float GetAveragedSurprise()
        {
            return (m_totalSurprise / m_samples) * ms_surpriseMultiplier;
        }


        public float? GetJoyRatio()
        {
            if(m_samples == 0)
            {
                return null;
            }
            return GetAveragedJoy() / Mathf.Max(m_joyThreshold, GetAveragedAnger(), GetAveragedSurprise());
        }

        public float? GetAngerRatio()
        {
            if(m_samples == 0)
            {
                return null;
            }

            return GetAveragedAnger() / Mathf.Max(m_angerThreshold, GetAveragedSurprise());
        }

        public float? GetSurpriseRatio()
        {
            if (m_samples == 0)
            {
                return null;
            }

            return GetAveragedSurprise() / m_surpriseThreshold;
        }

        public bool IsAngry()
        {
            return m_totalAnger > m_angerThreshold;
        }

        public bool IsHappy()
        {
            return m_totalJoy > m_joyThreshold && m_totalJoy * ms_joyMultiplier > m_totalAnger * ms_angerMultiplier && ms_joyMultiplier * m_totalJoy > ms_surpriseMultiplier * m_totalSurprise;
        }

        public bool IsSurprised()
        {
            return m_totalSurprise > m_surpriseThreshold && m_totalSurprise * ms_surpriseMultiplier > m_totalAnger * ms_angerMultiplier;
        }


        private IEnumerator ContinuousSampler;



        //This needs to be tossed into every child listener class
        public static void ReportSample(Dictionary<int, Face> faces)
        {
            Debug.Log("Base reporting sample");
            if (faces.Count > 0)
            {
                m_samples++;

                if (m_samples > 10)
                {
                    m_samples = m_samples / 5;
                    m_totalJoy = m_totalJoy / 5.0f;
                    m_totalAnger = m_totalAnger / 5.0f;
                    m_totalSurprise = m_totalSurprise / 5.0f;
                }

                m_totalJoy += faces[0].Emotions[Affdex.Emotions.Joy] * ms_joyMultiplier;
                m_totalAnger += faces[0].Emotions[Affdex.Emotions.Anger] * ms_angerMultiplier;
                m_totalSurprise += faces[0].Emotions[Affdex.Emotions.Surprise] * ms_surpriseMultiplier;
            }
            else {
                Debug.Log("No faces");
            }
        }

        /// <summary>
        /// Indicates that the face detector has started tracking a new face.
        /// <para>
        /// When the face tracker detects a face for the first time method is called.
        /// The receiver should expect that tracking continues until detection has stopped.
        /// </para>
        /// </summary>
        /// <param name="timestamp">Frame timestamp when new face was first observed.</param>
        /// <param name="faceId">Face identified.</param>
        public virtual void onFaceFound(float timestamp, int faceId)
        {
            s_faceLocated = true;
            setTextFound();
            Debug.Log("Found the face");
            SetInstructionSprite.FaceDetected();
        }
        /// <summary>
        /// Indicates that the face detector has stopped tracking a face.
        /// <para>
        /// When the face tracker no longer finds a face this method is called. The receiver should expect that there is no face tracking until the detector is
        /// started.
        /// </para>
        /// </summary>
        /// <param name="timestamp">Frame timestamp when previously observed face is no longer present.</param>
        /// <param name="faceId">Face identified.</param>
        public virtual void onFaceLost(float timestamp, int faceId)
        {
            s_faceLocated = false;
            setTextLost();
            Debug.Log("lost the face");
            SetInstructionSprite.FaceUndetected();
        }
        public abstract void setTextLost();
        public abstract void setTextFound();


    }
}
