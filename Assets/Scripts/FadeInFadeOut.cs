using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeInFadeOut : MonoBehaviour
{
    private const float c_opacityTimeMultiplier = .5f;

    private static FadeInFadeOut instance;
    [SerializeField]
    Image m_image;
    [SerializeField]
    private AudioSource m_source;

    [SerializeField]
    private AudioClip m_horn;
    [SerializeField]
    private AudioClip m_crash;

    private IEnumerator m_fadeInFadeOut;

    private static bool ms_fadeAuthorized = false;
     
    private static bool ms_fading = false;

    [SerializeField]
    private int nextSceneOverride = -1;

    private IEnumerator FadeOutCoroutine(int nextScene = -1)
    {

        bool fadeOutAudio = PersistantAudioSource.CheckToFadeOutAudio(SceneManager.GetActiveScene().buildIndex);
        //TODO: hack fix
        float opacity = 0.0f;
        while (opacity < 1.0f)
        {
            opacity += Time.deltaTime * c_opacityTimeMultiplier;
            if (fadeOutAudio)
            {
                AudioListener.volume -= Time.deltaTime * c_opacityTimeMultiplier;
                if (AudioListener.volume < 0)
                {
                    AudioListener.volume = 0.0f;
                }
            }

            float alpha = Mathf.Lerp(0.0f, 1.0f, opacity);

            m_image.color = new Color(Color.black.r, Color.black.g, Color.black.b, alpha);
            yield return new WaitForEndOfFrame();
        }
#if blinkToChangeScenes
		ms_fadeAuthorized = false;
		while (ms_fadeAuthorized == false) {
			yield return new WaitForEndOfFrame ();
		}
#endif

        if (nextSceneOverride != -1)
        {

            SceneManager.LoadScene(nextSceneOverride);

            yield break;
        }

        else if (nextScene != -1)
        {
            SceneManager.LoadScene(nextScene);

            yield break;
        }
        else {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
        ms_fading = false;

    }

    private IEnumerator FadeOutToCarCrashCoroutine()
    {
        //TODO: hack fix
        float opacity = 0.0f;
        //Fade out
        while (opacity < .7f)
        {
            opacity += Time.deltaTime * c_opacityTimeMultiplier;
            float alpha = Mathf.Lerp(0.0f, 1.0f, opacity);
            //AudioListener.volume = Mathf.Lerp (5.0f, 1.0f, opacity);
            m_image.color = new Color(Color.black.r, Color.black.g, Color.black.b, alpha);
            yield return new WaitForEndOfFrame();
        }

        //Fade back in
        while (opacity > 0.0f)
        {
            opacity -= Time.deltaTime * c_opacityTimeMultiplier;
            float alpha = Mathf.Lerp(0.0f, 1.0f, opacity);
            //AudioListener.volume = Mathf.Lerp (5.0f, 1.0f, opacity);
            m_image.color = new Color(Color.black.r, Color.black.g, Color.black.b, alpha);
            yield return new WaitForEndOfFrame();
        }

        //Start playing a car horn
        m_source.PlayOneShot(m_horn);

        //Fade in the headlights

        while (opacity < .7f)
        {
            opacity += Time.deltaTime * c_opacityTimeMultiplier;
            float alpha = Mathf.Lerp(0.0f, 1.0f, opacity);
            //AudioListener.volume = Mathf.Lerp (5.0f, 1.0f, opacity);
            m_image.color = new Color(Color.white.r, Color.white.g, Color.white.b, alpha);
            yield return new WaitForEndOfFrame();
        }



        //Play a crash sound effect
        m_source.PlayOneShot(m_crash);


        //Instantly change to black
        m_image.color = Color.black;

        yield return new WaitForSeconds(3.0f);

#if blinkToChangeScenes
		ms_fadeAuthorized = false;
		while (ms_fadeAuthorized == false) {
		yield return new WaitForEndOfFrame ();
		}
#endif
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

    public static void AuthorizeFade()
    {
        ms_fadeAuthorized = true;
    }

    public static bool FadeAuthorized()
    {
        return ms_fadeAuthorized;
    }

    private IEnumerator FadeInCoroutine()
    {
        float opacity = 1.0f;
        while (opacity > 0.0f)
        {
            opacity -= Time.deltaTime * c_opacityTimeMultiplier;
            float alpha = Mathf.Lerp(0.0f, 1.0f, opacity);
            AudioListener.volume += Time.deltaTime * c_opacityTimeMultiplier;
            if (AudioListener.volume > 1)
            {
                AudioListener.volume = 1.0f;
            }
            //AudioListener.volume = Mathf.Lerp (1.0f, 5.0f, opacity);
            m_image.color = new Color(Color.black.r, Color.black.g, Color.black.b, alpha);
            yield return new WaitForEndOfFrame();
        }
        ms_fading = false;
    }

    public static void FadeIn()
    {
        if (ms_fading == false)
        {
            instance.m_fadeInFadeOut = instance.FadeInCoroutine();
            instance.StartCoroutine(instance.m_fadeInFadeOut);
        }
    }

    public static void FadeOut()
    {

        if (ms_fading == false)
        {
            try
            {
                instance.StopCoroutine(instance.m_fadeInFadeOut);
            }
            catch
            {

            }
            instance.m_fadeInFadeOut = instance.FadeOutCoroutine();
            instance.StartCoroutine(instance.m_fadeInFadeOut);
        }
    }

    public static void FadeOutToCrash()
    {

        if (ms_fading == false)
        {
            try
            {
                instance.StopCoroutine(instance.m_fadeInFadeOut);
            }
            catch
            {

            }
            instance.m_fadeInFadeOut = instance.FadeOutToCarCrashCoroutine();
            instance.StartCoroutine(instance.m_fadeInFadeOut);
        }
    }

    public static void FadeOut(int nextScene)
    {
        if (ms_fading == false)
        {
            instance.StopCoroutine(instance.m_fadeInFadeOut);
            instance.StartCoroutine(instance.FadeOutCoroutine(nextScene));
        }
    }

    void Start()
    {
        instance = this;
        instance.m_fadeInFadeOut = instance.FadeInCoroutine();
        FadeIn();
    }
}
