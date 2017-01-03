using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Affdex;

public class SetInstructionSprite : MonoBehaviour
{
    public static SetInstructionSprite ms_instance;

    public enum FaceState
    {
        NEUTRAL,
        GRIN,
        BLINK,
        DISGUST,
        OPEN,
        FOCUS,
        JOY,
        ANGER,
        SURPRISE
    }

    [SerializeField]
    private Sprite m_neutral;
    [SerializeField]
    private Sprite[] m_blink;
    [SerializeField]
    private Sprite m_joy;
    [SerializeField]
    private Sprite m_anger;
    [SerializeField]
    private Sprite m_surprise;
    [SerializeField]
    private Sprite[] m_focus;
    [SerializeField]
    private Sprite[] m_open;

    [SerializeField]
    private Sprite[] m_angerSprites;
    [SerializeField]
    private Sprite[] m_joySprites;
    [SerializeField]
    private Sprite[] m_surpriseSprites;
    [SerializeField]
    private Sprite[] m_waitingForInputPetalSprites;

    [SerializeField]
    private Sprite m_currentFace;

    [SerializeField]
    private Sprite m_currentIcon;

    private static bool ms_isInUse = false;

    private IEnumerator m_facePrompt;
    private IEnumerator m_faceIcon;
    private const float c_fadeFaceTime = 1.0f;


    [SerializeField]
    private Image m_waitingForFaceIndicator;
    [SerializeField]
    private CanvasGroup m_emotionReadingCanvasRenderer;

    [SerializeField]
    private Image m_faceImage;

    [SerializeField]
    public Image m_angerImage;
    [SerializeField]
    public Image m_surpriseImage;
    [SerializeField]
    public Image m_joyImage;
    [SerializeField]
    private Image m_popInImage;

    [SerializeField]
    private Image[] m_angerScreen;
    [SerializeField]
    private Image[] m_joyScreen;
    [SerializeField]
    private Image[] m_surpriseScreen;

    [SerializeField]
    private Transform m_startPosition;
    [SerializeField]
    private Transform m_endPosition;

    private Color c_fadedOut = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    private Color c_fadedOutGrey = new Color(.5f, .5f, .5f, .5f);
    private Color c_fadedIn = new Color(1.0f, 1.0f, 1.0f, 1.0f);

    private const float c_fadeTime = 1.0f;

    private bool m_isFaceVisible = false;

    [SerializeField]
    private float m_maxSize = 5.0f;

    private const float c_mouthWaitTime = 2.0f;

    public delegate float? EmotionRatioDelegate();
    private float m_fadeInTime = 3.0f;

    private float emotionTimeSlice = .01f;
    private float emotionHoldTime = .1f;

    [SerializeField]
    public Image m_happyCheckMark;

    [SerializeField]
    public Image m_angryCheckMark;

    [SerializeField]
    public Image m_surprisedCheckMark;

    private float m_fadeOutTime = 1.0f;

    private float m_checkMarkInterpolationTime = 1.0f;

    [SerializeField]
    private AnimationCurve m_checkMarkAnimationCurve;

    [SerializeField]
    private CanvasGroup m_emotionSelectionCanvasGroup;

    [SerializeField]
    private CanvasGroup m_checkMarkCanvasGroup;


    public IEnumerator FadeOutTutorialIcons()
    {
        float t = 0.0f;

        float startingAlphaSelectionCanvas = m_emotionSelectionCanvasGroup.alpha;
        float startingalphaReadingCanvas = m_emotionReadingCanvasRenderer.alpha;
        float startingAlphaCheckMarkCanvasGroup = m_checkMarkCanvasGroup.alpha;

        while (t < m_fadeOutTime)
        {
            t += Time.deltaTime;
            m_emotionSelectionCanvasGroup.alpha = Mathf.Lerp(startingAlphaSelectionCanvas, 0.0f, t);
            m_emotionReadingCanvasRenderer.alpha = Mathf.Lerp(startingalphaReadingCanvas, 0.0f, t);
            m_checkMarkCanvasGroup.alpha = Mathf.Lerp(startingAlphaCheckMarkCanvasGroup, 0.0f, t);

            yield return new WaitForEndOfFrame();
        }
    }



    public IEnumerator FadeInTutorialEmotions()
    {
        float t = 0.0f;
        while (t < 1.0f)
        {
            t += Time.deltaTime;
            m_emotionSelectionCanvasGroup.alpha = t;
            yield return new WaitForEndOfFrame();
        }
    }

    // Use this for initialization
    void Awake()
    {
        m_emotionSelectionCanvasGroup.alpha = 0;
        ms_instance = this;
        m_faceIcon = BlinkIcon();
        m_facePrompt = BlinkPrompt();
        m_faceImage.CrossFadeAlpha(0.0f, 0.0f, false);

        m_happyCheckMark.CrossFadeAlpha(0.0f, 0.0f, false);
        m_surprisedCheckMark.CrossFadeAlpha(0.0f, 0.0f, false);
        m_angryCheckMark.CrossFadeAlpha(0.0f, 0.0f, false);
    }

    public void FadeInDialogImage()
    {
        this.m_faceImage.CrossFadeAlpha(1.0f, m_fadeInTime, false);
    }

    public void FadeOutDialogImage()
    {
        this.m_faceImage.CrossFadeAlpha(0.0f, m_fadeInTime, false);
    }

    public static void GrowIcon(float dt)
    {
        ms_instance.m_waitingForFaceIndicator.GetComponent<RectTransform>().localScale = Vector3.Lerp(ms_instance.m_waitingForFaceIndicator.GetComponent<RectTransform>().localScale, Vector3.one * ms_instance.m_maxSize, dt);
    }

    public static void ShrinkIcon(float dt)
    {
        ms_instance.m_waitingForFaceIndicator.GetComponent<RectTransform>().localScale = Vector3.Lerp(ms_instance.m_waitingForFaceIndicator.GetComponent<RectTransform>().localScale, Vector3.one, dt);
    }

    public static void FaceUndetected()
    {
        ms_instance.m_isFaceVisible = false;
    }

    public static void FaceDetected()
    {
        ms_instance.m_isFaceVisible = true;
    }

    private bool m_fading = false;


    public IEnumerator CheckForEmotion(EmotionRatioDelegate specifiedEmotionRatioDelegate, Image emotionIcon, Image checkMark, SetInstructionSprite.FaceState emotionToCheckFor)
    {
        float emotionHeldTime = 0.0f;
        while (true)
        {
            float? ratio = specifiedEmotionRatioDelegate();
            while (ratio == null)
            {
                ratio = specifiedEmotionRatioDelegate();
                yield return new WaitForEndOfFrame();
            }

            emotionIcon.CrossFadeAlpha(Mathf.Max(0.4f, Mathf.Min((float)ratio, 1.0f)), emotionTimeSlice, false);
            yield return new WaitForSeconds(emotionTimeSlice);

            if (ratio >= 1.0f)
            {
                emotionHeldTime += emotionTimeSlice;
            }

            if (emotionHeldTime > emotionHoldTime)
            {
                break;
            }
        }


        float t = 0.0f;

        checkMark.color = Color.white;
        checkMark.CrossFadeAlpha(1.0f, m_checkMarkInterpolationTime, false);

        while (t < m_checkMarkInterpolationTime)
        {
            t += Time.deltaTime;
            checkMark.transform.localScale = Vector3.one * m_checkMarkAnimationCurve.Evaluate(t / m_checkMarkInterpolationTime);
            yield return new WaitForEndOfFrame();
        }

        SetInstructionSprite.SetFaceState(emotionToCheckFor);
    }


    public IEnumerator WaitForAnEmotionToBeSet()
    {
        m_happyCheckMark.CrossFadeAlpha(0.0f, 0.0f, false);
        m_surprisedCheckMark.CrossFadeAlpha(0.0f, 0.0f, false);
        m_angryCheckMark.CrossFadeAlpha(0.0f, 0.0f, false);
        m_checkMarkCanvasGroup.alpha = 1.0f;

        yield return SetInstructionSprite.ms_instance.FadeInTutorialEmotions();

        EmotionRatioDelegate joyDelegate = ImageResultsListener.ms_instance.GetJoyRatio;
        IEnumerator JoyCoroutine = CheckForEmotion(joyDelegate, ms_instance.m_joyImage, ms_instance.m_happyCheckMark, SetInstructionSprite.FaceState.JOY);
        StartCoroutine(JoyCoroutine);


        EmotionRatioDelegate angerDelegate = ImageResultsListener.ms_instance.GetAngerRatio;
        IEnumerator angerCoroutine = CheckForEmotion(angerDelegate, ms_instance.m_angerImage, ms_instance.m_angryCheckMark, SetInstructionSprite.FaceState.ANGER);
        StartCoroutine(angerCoroutine);


        EmotionRatioDelegate surpriseDelegate = ImageResultsListener.ms_instance.GetSurpriseRatio;


        IEnumerator surpriseCoroutine = CheckForEmotion(surpriseDelegate, ms_instance.m_surpriseImage, ms_instance.m_surprisedCheckMark, SetInstructionSprite.FaceState.SURPRISE);
        StartCoroutine(surpriseCoroutine);
        
        while ((JoyCoroutine.MoveNext() && angerCoroutine.MoveNext() && surpriseCoroutine.MoveNext()))
        {
            yield return new WaitForEndOfFrame();
        }

        StopCoroutine(JoyCoroutine);
        StopCoroutine(angerCoroutine);
        StopCoroutine(surpriseCoroutine);

        yield return DialogueManager.Main.FadeOutRoutine();
        yield return FadeOutTutorialIcons();
    }

    private IEnumerator ScaleUpForTime(float targetTime)
    {
        float t = 0.0f;

        while (t < targetTime)
        {
            yield return new WaitForEndOfFrame();
            float dt = Time.deltaTime;
            GrowIcon(dt);
            t += Time.deltaTime;
        }
        t = 0.0f;
        while (ms_instance.m_waitingForFaceIndicator.GetComponent<RectTransform>().localScale != Vector3.one)
        {
            yield return new WaitForEndOfFrame();
            float dt = Time.deltaTime;
            ShrinkIcon(dt);
            t += Time.deltaTime;

        }
    }
    private IEnumerator ScaleUpPopIn;
    public static void ScaleUpPopInIndicator()
    {
        try
        {
            ms_instance.StopCoroutine(ms_instance.ScaleUpPopIn);

        }
        catch
        {

        }
        ms_instance.ScaleUpPopIn = ms_instance.ScaleUpForTime(1.0f);

        ms_instance.StartCoroutine(ms_instance.ScaleUpPopIn);
    }

    private IEnumerator PopInAnimate()
    {
        while (true)
        {

            if (!m_isFaceVisible)
            {
                m_popInImage.sprite = m_neutral;
            }
            else {
                for (int i = 0; i < m_waitingForInputPetalSprites.Length; i++)
                {
                    m_popInImage.sprite = m_waitingForInputPetalSprites[i];
                    yield return new WaitForSeconds(.1f);
                }
            }
        }
    }

    public IEnumerator PopInFadeIn()
    {

        float t = 0.0f;
        while (t <= c_fadeTime * Time.timeScale)
        {
            t += Time.deltaTime;
            m_popInImage.color = Color.Lerp(c_fadedOut, c_fadedIn, t / Time.timeScale);
            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator FadeInIndicator()
    {
        m_fading = true;
        float t = 0.0f;
        m_waitingForFaceIndicator.transform.position = m_startPosition.transform.position;

        while (t <= c_fadeTime)
        {
            t += Time.deltaTime * (1 / Time.timeScale);
            m_waitingForFaceIndicator.transform.position = Vector3.Lerp(m_startPosition.transform.position, m_endPosition.transform.position, (t / c_fadeTime) * Time.timeScale);
            m_waitingForFaceIndicator.color = Color.Lerp(c_fadedOut, c_fadedIn, t / Time.timeScale);

            //todo: replace this with the three icons that show facial recoginition

            yield return new WaitForEndOfFrame();
        }


        m_fading = false;
    }

    public void Start()
    {
        StartCoroutine(AngerEffectAnimation());


        StartCoroutine(JoyEffectAnimation());


        StartCoroutine(SurpriseEffectAnimation());
    }

    private IEnumerator AngerEffectInterpolation()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
        }
    }
    private bool m_currentlySampling = false;

    private IEnumerator AngerEffectAnimation()
    {
        while (true)
        {

            if (m_currentlySampling)
            {
                for (int i = 0; i < m_angerScreen.Length; i++)
                {
                    int cur = i;
                    int next = (i + 1) % m_angerScreen.Length;

                    float t = 0;
                    while (t < m_lerpScreenTime)
                    {
                        Color maxColor = ms_instance.m_angerImage.color;
                        m_angerScreen[cur].color = Color.Lerp(m_angerScreen[cur].color, Color.clear, Time.deltaTime);

                        m_angerScreen[next].color = Color.Lerp(m_angerScreen[next].color, maxColor, Time.deltaTime);
                        t += Time.deltaTime;
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_angerScreen.Length; i++)
                {
                    m_angerScreen[i].color = Color.Lerp(m_angerScreen[i].color, Color.clear, Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
            }
        }

    }

    [SerializeField]
    private float m_lerpScreenTime = 5.0f;

    private IEnumerator JoyEffectAnimation()
    {
        while (true)
        {

            if (m_currentlySampling)
            {

                for (int i = 0; i < m_joyScreen.Length; i++)
                {
                    int cur = i;
                    int next = (i + 1) % m_joyScreen.Length;
                    float t = 0;
                    while (t < m_lerpScreenTime)
                    {
                        Color maxColor = ms_instance.m_joyImage.color;
                        if (m_currentlySampling)
                        {
                            m_joyScreen[cur].color = Color.Lerp(m_joyScreen[cur].color, Color.clear, Time.deltaTime);
                        }
                        m_joyScreen[next].color = Color.Lerp(m_joyScreen[next].color, maxColor, Time.deltaTime);
                        t += Time.deltaTime;
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_joyScreen.Length; i++)
                {
                    m_joyScreen[i].color = Color.Lerp(m_joyScreen[i].color, Color.clear, Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }
            }
        }

    }


    private IEnumerator SurpriseEffectAnimation()
    {
        while (true)
        {


            if (m_currentlySampling)
            {
                for (int i = 0; i < m_surpriseScreen.Length; i++)
                {
                    int cur = i;
                    int next = (i + 1) % m_surpriseScreen.Length;
                    float t = 0;
                    while (t < m_lerpScreenTime)
                    {
                        Color maxColor = ms_instance.m_surpriseImage.color;
                        if (m_currentlySampling)
                        {
                            m_surpriseScreen[cur].color = Color.Lerp(m_surpriseScreen[cur].color, Color.clear, Time.deltaTime);
                        }
                        m_surpriseScreen[next].color = Color.Lerp(m_surpriseScreen[next].color, maxColor, Time.deltaTime);
                        t += Time.deltaTime;
                        yield return new WaitForEndOfFrame();
                    }
                }
            }
            else
            {
                for (int i = 0; i < m_surpriseScreen.Length; i++)
                {
                    m_surpriseScreen[i].color = Color.Lerp(m_surpriseScreen[i].color, Color.clear, Time.deltaTime);
                    yield return new WaitForEndOfFrame();
                }

            }
        }

    }

    bool paused = false;

    public static void SetInUse()
    {
        ms_isInUse = true;
    }

    public static void SetOutOfUse()
    {
        ms_isInUse = false;
    }

    public void Update()
    {
        if (m_isFaceVisible)
        {
            m_popInImage.sprite = m_currentIcon;
            m_faceImage.sprite = m_currentFace;
            m_angerImage.sprite = m_anger;
            m_joyImage.sprite = m_joy;
            m_surpriseImage.sprite = m_surprise;
        }
        else {
            m_popInImage.sprite = m_neutral;
            m_faceImage.sprite = m_neutral;
            m_angerImage.sprite = m_neutral;
            m_joyImage.sprite = m_neutral;
            m_surpriseImage.sprite = m_neutral;
        }
    }

    public IEnumerator PauseRoutine()
    {
        while (true)
        {
            if (paused == false && Input.GetKeyDown(KeyCode.P))
            {
                paused = true;
                Time.timeScale = 0.00f;
            }
            else if (Input.GetKeyDown(KeyCode.P))
            {
                paused = false;
                Time.timeScale = 1.0f;
            }
            yield return null;
        }
    }

    private IEnumerator FadeOutIndicator()
    {
        m_fading = true;
        float t = 0.0f;
        Color origColor = m_waitingForFaceIndicator.color;
        Color origColor2 = m_popInImage.color;

        float startingAlpha = m_emotionReadingCanvasRenderer.alpha;
        m_currentlySampling = false;

        while (t <= c_fadeTime)
        {
            t += Time.deltaTime * (1 / Time.timeScale);
            m_waitingForFaceIndicator.color = Color.Lerp(origColor, c_fadedOut, t / c_fadeTime);
            m_popInImage.color = Color.Lerp(origColor2, c_fadedOut, t / c_fadeTime);
            m_emotionReadingCanvasRenderer.alpha = startingAlpha - t / c_fadeTime;
            yield return new WaitForEndOfFrame();
        }

        m_fading = false;
    }

    public static void StartWaitingForEmotion(SetInstructionSprite.FaceState fps = SetInstructionSprite.FaceState.NEUTRAL)
    {
        switch (fps)
        {
            case SetInstructionSprite.FaceState.BLINK:
                ms_instance.StartCoroutine(ms_instance.BlinkPrompt());
                JournalWordTracker.CanBlink();
                ms_instance.StartCoroutine(ms_instance.FadeInIndicator());
                break;
            case SetInstructionSprite.FaceState.FOCUS:
                ms_instance.StartCoroutine(ms_instance.FocusPrompt());
                ms_instance.StartCoroutine(ms_instance.FadeInIndicator());
                break;
            case SetInstructionSprite.FaceState.OPEN:
                ms_instance.StartCoroutine(ms_instance.OpenPrompt());
                ms_instance.StartCoroutine(ms_instance.FadeInIndicator());
                break;
        }

        if (fps == SetInstructionSprite.FaceState.NEUTRAL)
        {
            Debug.LogError("Emotional Sampling ");
            ms_instance.m_currentlySampling = true;
            SetInstructionSprite.SetWaitingForFaceIndicator();
        }
    }

    public static void StopWaitingForEmotion()
    {
        ms_instance.StartCoroutine(ms_instance.FadeOutIndicator());
    }

    private IEnumerator OpenPrompt()
    {


        for (int i = 0; i < 999; i = (i + 1) % ms_instance.m_open.Length)
        {
            if (!m_isFaceVisible)
            {
                ms_instance.m_waitingForFaceIndicator.sprite = ms_instance.m_neutral;
            }
            else
            {
                ms_instance.m_waitingForFaceIndicator.sprite = ms_instance.m_open[i];
            }
            yield return new WaitForSeconds(c_mouthWaitTime);
        }
    }

    private IEnumerator FocusPrompt()
    {

        for (int i = 0; i < 999; i = (i + 1) % ms_instance.m_focus.Length)
        {
            if (!m_isFaceVisible)
            {
                ms_instance.m_waitingForFaceIndicator.sprite = ms_instance.m_neutral;
            }
            else
            {
                ms_instance.m_waitingForFaceIndicator.sprite = ms_instance.m_focus[i];
            }
            yield return new WaitForSeconds(.2f);
        }
    }

    private IEnumerator BlinkPrompt()
    {

        for (int i = 0; i < 999; i = (i + 1) % ms_instance.m_blink.Length)
        {
            if (!m_isFaceVisible)
            {
                ms_instance.m_waitingForFaceIndicator.sprite = ms_instance.m_neutral;
            }
            else
            {
                ms_instance.m_waitingForFaceIndicator.sprite = ms_instance.m_blink[i];

            }

            yield return new WaitForSeconds(.2f);
        }
    }

    private IEnumerator OpenIcon()
    { 
        for (int i = 0; i < 999; i = (i + 1) % ms_instance.m_open.Length)
        {
            yield return new WaitForSeconds(.2f);
            //ms_instance.m_faceImage.sprite = ms_instance.m_open [i];
        }
    }

    private IEnumerator FocusIcon()
    {
        for (int i = 0; i < 999; i = (i + 1) % ms_instance.m_focus.Length)
        {
            //ms_instance.m_faceImage.sprite = ms_instance.m_focus [i];
            yield return new WaitForSeconds(.2f);
        }
    }

    private IEnumerator BlinkIcon()
    {

        for (int i = 0; i < 999; i = (i + 1) % ms_instance.m_blink.Length)
        {
            //ms_instance.m_faceImage.sprite = ms_instance.m_blink [i];
            yield return new WaitForSeconds(.2f);
        }
    }

    public static void SetWaitingForFaceIndicator()
    {
        ms_instance.StartCoroutine(ms_instance.FadeInFacialDetectionCanvas());
        ms_instance.StopCoroutine(ms_instance.m_facePrompt);

    }

    public IEnumerator FadeInFacialDetectionCanvas()
    {
        float t = 0.0f;

        while (t < m_fadeInTime)
        {
            t += Time.deltaTime;

            m_emotionReadingCanvasRenderer.alpha = t / m_fadeInTime;
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator AnimatePetalAtWaitingForFaceIndicator()
    {
        while (true)
        {
            if (!m_isFaceVisible)
            {
                m_waitingForFaceIndicator.sprite = m_neutral;
            }
            else
            {

                for (int i = 0; i < m_waitingForInputPetalSprites.Length; i++)
                {
                    m_waitingForFaceIndicator.sprite = m_waitingForInputPetalSprites[i];
                    yield return new WaitForSeconds(.5f * (1 / Time.timeScale));
                }
            }

        }
    }

    public static void SetFaceRatios(float joy, float anger, float surprise)
    { //TODO: currently this is the only thing controlling fade in
        float totalEmotion = joy + anger + surprise;

        ms_instance.m_joyImage.color = Color.Lerp(ms_instance.c_fadedOutGrey, ms_instance.c_fadedIn, joy / totalEmotion);
        ms_instance.m_angerImage.color = Color.Lerp(ms_instance.c_fadedOutGrey, ms_instance.c_fadedIn, anger / totalEmotion);
        ms_instance.m_surpriseImage.color = Color.Lerp(ms_instance.c_fadedOutGrey, ms_instance.c_fadedIn, surprise / totalEmotion);
    }

    public static void SetEmotionDeciding()
    {
        ms_instance.StartCoroutine(ms_instance.LoseHalfOpacity());
    }


    public static void SetFaceState(FaceState fs)
    {



        ms_instance.StopCoroutine(ms_instance.m_faceIcon);
        switch (fs)
        {
            case FaceState.JOY:
                ms_instance.FaceStateChange(ms_instance.m_joy);
                break;
            case FaceState.ANGER:
                ms_instance.FaceStateChange(ms_instance.m_anger);
                break;
            case FaceState.SURPRISE:
                ms_instance.FaceStateChange(ms_instance.m_surprise);
                break;
        }
    }

    private IEnumerator LoseHalfOpacity()
    {
        float t = 0.0f;
        while (t < c_fadeFaceTime / 4)
        {
            t += Time.deltaTime;
            ms_instance.m_faceImage.color = Color.Lerp(c_fadedIn, c_fadedOut, t / c_fadeFaceTime);
            yield return new WaitForEndOfFrame();
        }
    }

    private void FaceStateChange(Sprite m_image)
    {
        //Change the image
        ms_instance.m_currentFace = m_image;
        ms_instance.m_faceImage.sprite = m_image;
    }
}
