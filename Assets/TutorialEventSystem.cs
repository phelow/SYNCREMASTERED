using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Affdex;

public class TutorialEventSystem : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_spriteRenderer;
    private const float c_delayBeforeFadeOut = 4.0f;
    private const float c_fadeTime = 1.0f;
    private static TutorialEventSystem ms_instance;

    [SerializeField]
    private Image m_happyIcon;
    [SerializeField]
    private Image m_angryIcon;
    [SerializeField]
    private Image m_surprisedIcon;

    [SerializeField]
    private Image m_happyCheckMark;

    [SerializeField]
    private Image m_angryCheckMark;

    [SerializeField]
    private Image m_surprisedCheckMark;

    private Color c_visible = new Color(1, 1, 1, 1);
    private Color c_inVisible = new Color(1, 1, 1, 0);

    private bool m_isCalibrated = false;
    private bool m_playerOpenedMouth = false;

    private float m_checkMarkInterpolationTime = 1.0f;

    [SerializeField]
    private AnimationCurve m_checkMarkAnimationCurve;

    [SerializeField]
    private CanvasGroup m_tutorialImageGroup;

    private float m_fadeOutTime = 1.0f;

    // Use this for initialization
    void Start()
    {
        ms_instance = this;
        StartCoroutine(TutorialSequence());
    }

    bool happyCoroutineCompleted;
    private IEnumerator CheckForHappy()
    {
        while (true)
        {
            float? ratio = ImageResultsListener.ms_instance.GetJoyRatio();
            while(ratio == null)
            {
                ratio = ImageResultsListener.ms_instance.GetJoyRatio();
                yield return new WaitForEndOfFrame();
            }

            m_happyIcon.CrossFadeAlpha(Mathf.Max(.1f,Mathf.Min((float)ratio, 1.0f)), .1f, false);
            yield return new WaitForSeconds(.1f);

            if(ratio >= 1.0f)
            {
                break;
            }
        }


        float t = 0.0f;
        while (t < m_checkMarkInterpolationTime)
        {
            t += Time.deltaTime;

            m_happyCheckMark.color = Color.Lerp(Color.clear, Color.white, t / m_checkMarkInterpolationTime);
            m_happyCheckMark.transform.localScale = Vector3.one * m_checkMarkAnimationCurve.Evaluate(t / m_checkMarkInterpolationTime);
            yield return new WaitForEndOfFrame();
        }

        happyCoroutineCompleted = true;
    }

    bool angryCoroutineCompleted;
    private IEnumerator CheckForAngry()
    {
        while (true)
        {
            float? ratio = ImageResultsListener.ms_instance.GetAngerRatio();
            while (ratio == null)
            {
                ratio = ImageResultsListener.ms_instance.GetAngerRatio();
                yield return new WaitForEndOfFrame();
            }

            m_angryIcon.CrossFadeAlpha(Mathf.Max(.1f, Mathf.Min((float)ratio, 1.0f)), .1f, false);
            yield return new WaitForSeconds(.1f);

            if (ratio >= 1.0f)
            {
                break;
            }
        }



        float t = 0.0f;
        while (t < m_checkMarkInterpolationTime)
        {
            t += Time.deltaTime;

            m_angryCheckMark.color = Color.Lerp(Color.clear, Color.white, t / m_checkMarkInterpolationTime);
            m_angryCheckMark.transform.localScale = Vector3.one * m_checkMarkAnimationCurve.Evaluate(t / m_checkMarkInterpolationTime);
            yield return new WaitForEndOfFrame();
        }

        angryCoroutineCompleted = true;
    }

    bool surprisedCoroutineCompleted;
    private IEnumerator CheckForSurprised()
    {
        while (true)
        {
            float? ratio = ImageResultsListener.ms_instance.GetSurpriseRatio();
            while (ratio == null)
            {
                ratio = ImageResultsListener.ms_instance.GetSurpriseRatio();
                yield return new WaitForEndOfFrame();
            }

            m_surprisedIcon.CrossFadeAlpha(Mathf.Max(.1f, Mathf.Min((float)ratio, 1.0f)), .1f, false);
            yield return new WaitForSeconds(.1f);

            if (ratio >= 1.0f)
            {
                break;
            }
        }



        float t = 0.0f;
        while (t < m_checkMarkInterpolationTime)
        {
            t += Time.deltaTime;

            m_surprisedCheckMark.color = Color.Lerp(Color.clear, Color.white, t / m_checkMarkInterpolationTime);
            m_surprisedCheckMark.transform.localScale = Vector3.one * m_checkMarkAnimationCurve.Evaluate(t / m_checkMarkInterpolationTime);
            yield return new WaitForEndOfFrame();
        }

        surprisedCoroutineCompleted = true;
    }

    private IEnumerator FadeOutTutorialIcons()
    {
        float t = 0.0f;
        while (t < m_fadeOutTime)
        {
            t += Time.deltaTime;
            m_happyCheckMark.color = Color.Lerp(Color.white, Color.clear, t / m_fadeOutTime);
            m_angryCheckMark.color = Color.Lerp(Color.white, Color.clear, t / m_fadeOutTime);
            m_surprisedCheckMark.color = Color.Lerp(Color.white, Color.clear, t / m_fadeOutTime);
            m_happyIcon.color = Color.Lerp(Color.white, Color.clear, t / m_fadeOutTime);
            m_angryIcon.color = Color.Lerp(Color.white, Color.clear, t / m_fadeOutTime);
            m_surprisedIcon.color = Color.Lerp(Color.white, Color.clear, t / m_fadeOutTime);


            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator TutorialSequence()
    {
        m_tutorialImageGroup.alpha = 0;
        yield return new WaitForSeconds(c_delayBeforeFadeOut);

        float t = 0.0f;
        while (t <= c_fadeTime)
        {
            m_spriteRenderer.color = Color.Lerp(c_visible, c_inVisible, t);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }


        DialogueManager.Main.DisplayTutorialText(TutorialEvents.Calibration, true); //TODO: streamline these coroutines
        SetInstructionSprite.ms_instance.StartCoroutine(SetInstructionSprite.ms_instance.PopInFadeIn());
        m_isCalibrated = false;
        yield return new WaitForSeconds(2.0f);
        yield return Calibrate();

        DialogueManager.Main.FadeOut();
        yield return new WaitForSeconds(3.0f);


        SetInstructionSprite.StartWaitingForEmotion(SetInstructionSprite.FaceState.OPEN);
        DialogueManager.Main.DisplayTutorialText(TutorialEvents.OpenMouthToContinue, true);
        yield return new WaitForSeconds(3.0f);

        m_playerOpenedMouth = false;
        while (m_playerOpenedMouth == false)
        {
            yield return new WaitForEndOfFrame();
        }
        SetInstructionSprite.StopWaitingForEmotion();
        DialogueManager.Main.FadeOut();
        yield return new WaitForSeconds(3.0f);



        DialogueManager.Main.DisplayTutorialText(TutorialEvents.PromptEmotions, true);
        yield return new WaitForSeconds(5.0f);
        bool waiting = true;

        GameObject.Find("PopInIndicator").gameObject.SetActive(false);
        //SetInstructionSprite.HideIcon();    //Hide the petal icon now so that the mouth command can be displayed instead

        SetInstructionSprite.StartWaitingForEmotion(SetInstructionSprite.FaceState.OPEN);
        m_playerOpenedMouth = false;
        while (m_playerOpenedMouth == false)
        {
            Debug.Log(78);
            yield return new WaitForEndOfFrame();
        }
        SetInstructionSprite.StopWaitingForEmotion();
        DialogueManager.Main.FadeOut();
        yield return new WaitForSeconds(3.0f);

        happyCoroutineCompleted = false;
        angryCoroutineCompleted = false;
        surprisedCoroutineCompleted = false;

        DialogueManager.SetCurrentEmotion(DialogueManager.Emotion.Disgust);

        DialogueManager.Main.DisplayTutorialText(TutorialEvents.PromptTutorial, true);

        yield return FadeInTutorialEmotions();

        StartCoroutine(CheckForHappy());
        StartCoroutine(CheckForAngry());
        StartCoroutine(CheckForSurprised());

        while (!(happyCoroutineCompleted && angryCoroutineCompleted && surprisedCoroutineCompleted))
        {
            yield return new WaitForEndOfFrame();
        }

        yield return FadeOutTutorialIcons();


        waiting = true;

        DialogueManager.Emotion emotion = DialogueManager.Emotion.Anger;

        yield return new WaitForSeconds(5.0f);

        DialogueManager.Main.DisplayTutorialText(TutorialEvents.MakeAnyFaceToStart, true);

        yield return new WaitForSeconds(3.0f);


        waiting = true;

        emotion = DialogueManager.Emotion.Anger;
        while (waiting)
        {
            yield return new WaitForEndOfFrame();
            if (DialogueManager.CanGetCurrentEmotion())
            {
                emotion = DialogueManager.GetCurrentEmotion();
                waiting = false;
                DialogueManager.DisableCurrentEmotion();
            }
            yield return new WaitForEndOfFrame();
        }
        FadeInFadeOut.FadeOut();
    }

    private IEnumerator FadeInTutorialEmotions()
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime;
            m_tutorialImageGroup.alpha = t;
            yield return new WaitForEndOfFrame();
        }
    }


    public static void playerOpenMouth()
    {
        Debug.Log("ms_instance.m_playerOpenedMouth = true;");
        ms_instance.m_playerOpenedMouth = true;
    }

    private IEnumerator Calibrate()
    {
        bool faceFullyDetected = false;
        while (faceFullyDetected == false)
        {
            float tDetected = 0.0f;
            yield return new WaitForEndOfFrame();
            while (tDetected < 2.0f && TutorialListener.FaceInRange())
            {
                tDetected += Time.deltaTime;
                yield return new WaitForEndOfFrame();
            }
            if (TutorialListener.FaceInRange())
            {
                faceFullyDetected = true;
            }

        }
        m_isCalibrated = true;
    }
}
