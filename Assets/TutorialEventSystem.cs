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
    
    private Color c_visible = new Color(1, 1, 1, 1);
    private Color c_inVisible = new Color(1, 1, 1, 0);

    private bool m_isCalibrated = false;

    // Use this for initialization
    void Start()
    {
        ms_instance = this;
        StartCoroutine(TutorialSequence());
    }

    bool happyCoroutineCompleted;

    private IEnumerator CalibrationCoroutine()
    {
        SetInstructionSprite.EmotionRatioDelegate joyDelegate = ImageResultsListener.ms_instance.GetJoyRatio;
        IEnumerator JoyCoroutine = SetInstructionSprite.ms_instance.CheckForEmotion(joyDelegate, SetInstructionSprite.ms_instance.m_joyImage, SetInstructionSprite.ms_instance.m_happyCheckMark,SetInstructionSprite.FaceState.JOY);
        StartCoroutine(JoyCoroutine);


        SetInstructionSprite.EmotionRatioDelegate angerDelegate = ImageResultsListener.ms_instance.GetAngerRatio;
        IEnumerator angerCoroutine = SetInstructionSprite.ms_instance.CheckForEmotion(angerDelegate, SetInstructionSprite.ms_instance.m_angerImage, SetInstructionSprite.ms_instance.m_angryCheckMark,SetInstructionSprite.FaceState.ANGER);
        StartCoroutine(angerCoroutine);


        SetInstructionSprite.EmotionRatioDelegate surpriseDelegate = ImageResultsListener.ms_instance.GetSurpriseRatio;
        IEnumerator surpriseCoroutine = SetInstructionSprite.ms_instance.CheckForEmotion(surpriseDelegate, SetInstructionSprite.ms_instance.m_surpriseImage, SetInstructionSprite.ms_instance.m_surprisedCheckMark, SetInstructionSprite.FaceState.SURPRISE);
        StartCoroutine(surpriseCoroutine);

        while (!(JoyCoroutine.MoveNext() == false && angerCoroutine.MoveNext() == false && surpriseCoroutine.MoveNext() == false))
        {
            yield return new WaitForEndOfFrame();
        }
    }
    
    public static IEnumerator WaitForOpenMouth()
    {
        SetInstructionSprite.StartWaitingForEmotion(SetInstructionSprite.FaceState.OPEN);

        SetInstructionSprite.m_playerOpenedMouth = false;
        while (SetInstructionSprite.m_playerOpenedMouth == false)
        {
            yield return new WaitForEndOfFrame();
        }

        SetInstructionSprite.StopWaitingForEmotion();
        yield return DialogueManager.Main.FadeOutRoutine();
    }

    private IEnumerator TutorialSequence()
    {
        yield return new WaitForSeconds(c_delayBeforeFadeOut);

        float t = 0.0f;
        while (t <= c_fadeTime)
        {
            t += Time.deltaTime;
            m_spriteRenderer.color = Color.Lerp(c_visible, c_inVisible, t);
            yield return new WaitForEndOfFrame();
        }


        yield return DialogueManager.Main.TutorialCoroutine(TutorialEvents.Calibration, true);
        SetInstructionSprite.ms_instance.StartCoroutine(SetInstructionSprite.ms_instance.PopInFadeIn());
        m_isCalibrated = false;
        yield return Calibrate();

        yield return DialogueManager.Main.FadeOutRoutine();
        

        yield return DialogueManager.Main.TutorialCoroutine(TutorialEvents.OpenMouthToContinue, true);
        yield return WaitForOpenMouth();



        yield return DialogueManager.Main.TutorialCoroutine(TutorialEvents.PromptEmotions, true);

        bool waiting = true;

        GameObject.Find("PopInIndicator").gameObject.SetActive(false);
        //SetInstructionSprite.HideIcon();    //Hide the petal icon now so that the mouth command can be displayed instead

        yield return WaitForOpenMouth();
             
        yield return DialogueManager.Main.TutorialCoroutine(TutorialEvents.PromptTutorial, true);
        
        yield return CalibrationCoroutine();
        
        yield return DialogueManager.Main.FadeOutRoutine();
        yield return SetInstructionSprite.ms_instance.FadeOutTutorialIcons();


        waiting = true;

        DialogueManager.Emotion emotion = DialogueManager.Emotion.Anger;
        
        yield return DialogueManager.Main.TutorialCoroutine(TutorialEvents.MakeAnyFaceToStart, true);
        
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        FadeInFadeOut.FadeOut();
    }
    
    public static void playerOpenMouth()
    {
        Debug.Log("ms_instance.m_playerOpenedMouth = true;");
        SetInstructionSprite.m_playerOpenedMouth = true;
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
