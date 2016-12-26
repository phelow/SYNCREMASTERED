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

        private float m_joyThreshold = 2.0f;
        private float m_angerThreshold = 2.0f;
        private float m_surpriseThreshold = 2.0f;

        private static int m_samples = 0;
        private static float m_totalJoy = 0.0f;
        private static float m_totalDisgust = 0.0f;
        private static float m_totalAnger = 0.0f;
        private static float m_totalSadness = 0.0f;
        private static float m_totalSurprise = 0.0f;

        private static float ms_surpriseMultiplier = 1.0f;
        private static float ms_angerMultiplier = 1.0f;
        private static float ms_joyMultiplier = 1.0f;

        /// <summary>
        /// Indicates image results are available.
        /// </summary>
        /// <param name="faces">The faces.</param>
        public abstract void onImageResults(Dictionary<int, Face> faces);

        private static ImageResultsListener m_instance;
        protected static bool s_faceLocated;
        [SerializeField]
        protected Text m_text;

        private static SetInstructionSprite.FaceState s_dominantFaceState;

        private IEnumerator samplerRoutine;

        public void Awake()
        {
            m_instance = this;
            samplerRoutine = m_instance.FacialSampler();
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

        public static void StartContinousSample()
        {
            m_instance.StartCoroutine(m_instance.ContinuousSampling());
        }

        public IEnumerator ContinuousSampling()
        {
            bool sampleTaken = false;
            int checks = 0;
            while (true)
            {
                m_totalJoy = 0.0f;
                m_totalDisgust = 0.0f;
                m_totalAnger = 0.0f;
                m_totalSadness = 0.0f;
                m_totalSurprise = 0.0f;

                float t = 0;
                while (t < c_sampleTime * Time.timeScale)
                {
                    Debug.Log("Sampling");
                    t += Time.deltaTime;
                    ProgressParticleSystem.SetEmotions(m_totalAnger / m_samples, m_totalJoy / m_samples, m_totalSadness / m_samples, m_totalSurprise / m_samples);
                    DialogueChoiceTracker.AddIntensityBoost(m_totalAnger / m_samples, m_totalJoy / m_samples, m_totalSurprise / m_samples);
                    yield return new WaitForEndOfFrame();
                }

                //yield return new WaitForSeconds (c_sampleTime * Time.timeScale);
                checks++;
                if (m_samples > 0)
                {
                    m_totalJoy *= 1.0f / m_samples;
                    m_totalDisgust *= 1.0f / m_samples;
                    m_totalAnger *= 1.0f / m_samples;
                    m_totalSadness *= 1.0f / m_samples;
                    m_totalSurprise *= 1.0f / m_samples;

                    m_totalSadness *= c_sadnessModifier;

                    if (m_totalJoy > m_joyThreshold && m_totalJoy * ms_joyMultiplier > m_totalAnger * ms_angerMultiplier && ms_joyMultiplier * m_totalJoy > ms_surpriseMultiplier * m_totalSurprise)
                    {
                        Debug.Log("Joy");
                        sampleTaken = true;
                        DialogueChoiceTracker.AddJoy();
                        DialogueManager.SetCurrentEmotion(DialogueManager.Emotion.Joy);
                        ProgressParticleSystem.SetEmotions(0, 0, 0, 0);
                        SetDominantEmotion();
                        yield break;
                    }
                    else if (m_totalAnger > m_angerThreshold && ms_angerMultiplier * m_totalAnger > ms_surpriseMultiplier * m_totalSurprise)
                    {
                        Debug.Log("Anger");
                        sampleTaken = true;
                        DialogueChoiceTracker.AddAnger();
                        DialogueManager.SetCurrentEmotion(DialogueManager.Emotion.Anger);
                        ProgressParticleSystem.SetEmotions(0, 0, 0, 0);
                        SetDominantEmotion();
                        yield break;
                    }
                    else if (m_totalSurprise > m_surpriseThreshold)
                    {
                        Debug.Log("Surprise");
                        sampleTaken = true;
                        DialogueChoiceTracker.AddSurprise();
                        DialogueManager.SetCurrentEmotion(DialogueManager.Emotion.Surprise);
                        ProgressParticleSystem.SetEmotions(0, 0, 0, 0);
                        SetDominantEmotion();
                        yield break;
                    }
                }
            }
        }

        public IEnumerator FacialSampler(bool idle = false)
        {
            SetInstructionSprite.SetInUse();
            SetEmotionDeciding();
            Debug.Log("--STarting Facial Sampler--");
            bool sampleTaken = false;
            int checks = 0;
            while (sampleTaken == false)
            {
                m_samples = 0;

                m_totalJoy = 0.0f;
                m_totalDisgust = 0.0f;
                m_totalAnger = 0.0f;
                m_totalSadness = 0.0f;
                m_totalSurprise = 0.0f;
                Debug.Log("51 m_totalJoy:" + m_totalJoy + "m_totalAnger:" + m_totalAnger + "m_totalSurprise:" + m_totalSurprise);

                float t = 0;
                while (t < c_sampleTime)
                {
                    t += Time.deltaTime * (1 / Time.timeScale);
                    Debug.Log("t:" + t + "c_sampleTime:" + c_sampleTime);
                    ProgressParticleSystem.SetEmotions(m_totalAnger / m_samples, m_totalJoy / m_samples, m_totalSadness / m_samples, m_totalSurprise / m_samples);
                    DialogueChoiceTracker.AddIntensityBoost(m_totalAnger / m_samples, m_totalJoy / m_samples, m_totalSurprise / m_samples);
                    yield return new WaitForEndOfFrame();
                }

                //yield return new WaitForSeconds (c_sampleTime * Time.timeScale);
                checks++;
                if (m_samples > 0)
                {
                    m_totalJoy *= 1.0f / m_samples;
                    m_totalDisgust *= 1.0f / m_samples;
                    m_totalAnger *= 1.0f / m_samples;
                    m_totalSadness *= 1.0f / m_samples;
                    m_totalSurprise *= 1.0f / m_samples;

                    m_totalSadness *= c_sadnessModifier;


                    Debug.Log("idle: " + idle + " m_totalJoy:" + m_totalJoy + " m_totalDisgust:" + m_totalDisgust + " m_totalAnger:" + m_totalAnger + " m_totalSadness:" + m_totalSadness + " m_totalSurprise:" + m_totalSurprise);

                    if (m_totalJoy > m_joyThreshold && m_totalJoy * ms_joyMultiplier > m_totalAnger * ms_angerMultiplier && ms_joyMultiplier * m_totalJoy > ms_surpriseMultiplier * m_totalSurprise)
                    {
                        sampleTaken = true;
                        DialogueChoiceTracker.AddJoy();
                        DialogueManager.SetCurrentEmotion(DialogueManager.Emotion.Joy);
                        ProgressParticleSystem.SetEmotions(0, 0, 0, 0);
                        SetDominantEmotion();
                        yield break;
                    }
                    else if (m_totalAnger > m_angerThreshold && ms_angerMultiplier * m_totalAnger > ms_surpriseMultiplier * m_totalSurprise)
                    {
                        sampleTaken = true;
                        DialogueChoiceTracker.AddAnger();
                        DialogueManager.SetCurrentEmotion(DialogueManager.Emotion.Anger);
                        ProgressParticleSystem.SetEmotions(0, 0, 0, 0);
                        SetDominantEmotion();
                        yield break;
                    }
                    else if (m_totalSurprise > m_surpriseThreshold)
                    {
                        sampleTaken = true;
                        DialogueChoiceTracker.AddSurprise();
                        DialogueManager.SetCurrentEmotion(DialogueManager.Emotion.Surprise);
                        ProgressParticleSystem.SetEmotions(0, 0, 0, 0);
                        SetDominantEmotion();
                        yield break;

                    }
                }

                if (checks >= 2 && idle == false)
                {
                    sampleTaken = true;
                    DialogueManager.SetCurrentEmotion(DialogueManager.GetLastEmotion());
                    ProgressParticleSystem.SetEmotions(0, 0, 0, 0);
                    yield break;
                }
            }
            SetInstructionSprite.SetOutOfUse();
            ProgressParticleSystem.SetEmotions(0, 0, 0, 0);
        }

        //This needs to be tossed into every child listener class
        public static void ReportSample(Dictionary<int, Face> faces)
        {
            if (faces.Count > 0)
            {

                m_samples++;
                m_totalJoy += faces[0].Emotions[Affdex.Emotions.Joy] * ms_joyMultiplier;
                m_totalAnger += faces[0].Emotions[Affdex.Emotions.Anger] * ms_angerMultiplier;
                m_totalSurprise += faces[0].Emotions[Affdex.Emotions.Surprise] * ms_surpriseMultiplier;
                Debug.Log("m_totalJoy:" + m_totalJoy + " m:_totalAnger" + m_totalAnger + " m_totalSurprise:" + m_totalSurprise);
                SetInstructionSprite.SetFaceRatios(m_totalJoy / m_samples, m_totalAnger / m_samples, m_totalSurprise / m_samples);
            }
            else {
                Debug.Log("No faces");
            }
        }

        public static void TakeSample(bool idle = false, bool keepOldIcon = false)
        {

            if (keepOldIcon == false)
            {
                SetInstructionSprite.StartWaitingForEmotion();
            }
            else {
                Debug.Log("TAking another sample with keepOldIcon:" + keepOldIcon);
            }
            try
            {
                m_instance.StopCoroutine(m_instance.samplerRoutine);
            }
            catch
            {
            }
            Debug.Log(m_instance);
            ProgressParticleSystem.SetEmotions(0, 0, 0, 0);
            m_instance.samplerRoutine = m_instance.FacialSampler(idle);
            m_instance.StartCoroutine(m_instance.samplerRoutine);

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
