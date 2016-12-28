using UnityEngine;
using System.Collections;
using Affdex;

public class TutorialEventSystem : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer m_spriteRenderer;
    private const float c_delayBeforeFadeOut = 4.0f;
    private const float c_fadeTime = 1.0f;
    private static TutorialEventSystem ms_instance;

    private Color c_visible = new Color(1, 1, 1, 1);
    private Color c_inVisible = new Color(1, 1, 1, 0);

    private bool m_isCalibrated = false;
    private bool m_playerOpenedMouth = false;

    // Use this for initialization
    void Start()
    {
        ms_instance = this;
        StartCoroutine(TutorialSequence());
    }

    private IEnumerator TutorialSequence()
    {
        yield return new WaitForSeconds(c_delayBeforeFadeOut);

        float t = 0.0f;
        while (t <= c_fadeTime)
        {
            m_spriteRenderer.color = Color.Lerp(c_visible, c_inVisible, t);
            t += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        DialogueManager.Main.DisplayTutorialText(TutorialEvents.Calibration, true);
        m_isCalibrated = false;
        StartCoroutine(Calibrate());
        yield return new WaitForSeconds(8.0f);
        while (m_isCalibrated == false)
        {
            yield return new WaitForEndOfFrame();
        }
        DialogueManager.Main.FadeOut();
        yield return new WaitForSeconds(3.0f);


        SetInstructionSprite.StartWaitingForEmotion(SetInstructionSprite.FaceState.OPEN);
        DialogueManager.Main.DisplayTutorialText(TutorialEvents.OpenMouthToContinue, true);
        yield return new WaitForSeconds(3.0f);

        m_playerOpenedMouth = false;
        while (m_playerOpenedMouth == false)
        {
            Debug.Log(78);
            yield return new WaitForEndOfFrame();
        }
        SetInstructionSprite.StopWaitingForEmotion();
        DialogueManager.Main.FadeOut();
        yield return new WaitForSeconds(3.0f);
        SetInstructionSprite.PopInWaitingForFaceIndicator();



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

        waiting = true;

        DialogueManager.Emotion emotion = DialogueManager.Emotion.Anger;

        yield return new WaitForSeconds(5.0f);

        DialogueManager.Main.DisplayTutorialText(TutorialEvents.MakeAnyFaceToStart, true);

        yield return new WaitForSeconds(3.0f);


        waiting = true;

        emotion = DialogueManager.Emotion.Anger;
        ImageResultsListener.TakeSample(true);
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
