using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Affdex;
using UnityEngine.SceneManagement;

public class WordDecay : MonoBehaviour
{
    [SerializeField]
    private Text m_text;
    [SerializeField]
    private float m_timeDelay = 10.0f;
    [SerializeField]
    private float m_waitTime;
    [SerializeField]
    private float m_intervalTime;
    [SerializeField]
    private Rigidbody2D m_rb;

    [SerializeField]
    private bool m_erase;
    [SerializeField]
    private int m_nextScene;
    [SerializeField]
    private bool m_lastLevel = false;
    [SerializeField]
    private bool m_goodEnding = false;

    [SerializeField]
    private static int s_eraseInput;
    private static int s_totalErased = 0;

    private string m_origText;

    private static Color c_fadedOut = new Color(1.0f, 1.0f, 1.0f, 0.0f);
    private static Color c_fadedIn = Color.black;

    private const int c_erasedNeeded = 10;

    [SerializeField]
    private float m_textRatioX;
    [SerializeField]
    private float m_textRatioY;
    [SerializeField]
    private bool m_triggerEnding = true;
    [SerializeField]
    private bool m_invisAtStart = false;

    // Use this for initialization
    void Start()
    {
        if (m_erase)
        {
            s_totalErased = 0;
            s_eraseInput = 0;
            m_origText = m_text.text;

            StartCoroutine(EraseText());

            m_erase = false;
            //StartCoroutine (SpamLetters ());
        }
    }

    public IEnumerator FadeIn()
    {
        float t = 0;
        float interpTime = Random.Range(0.2f, 2.0f);
        while (t < interpTime)
        {
            t += Time.deltaTime;

            m_text.color = Color.Lerp(c_fadedOut, c_fadedIn, t / interpTime);
            yield return new WaitForEndOfFrame();

        }
    }

    public static void TellToErase()
    {
        s_eraseInput++;
        if (erpo)
        {
            SetInstructionSprite.ScaleUpPopInIndicator();
        }
    }

    public IEnumerator SpamLetters()
    {
        while (true)
        {
            GameObject go = GameObject.Instantiate(this.gameObject);
            go.GetComponent<WordDecay>().StartCoroutine(go.GetComponent<WordDecay>().FadeAway());
            go.transform.position = transform.position;
            go.transform.parent = transform.parent;
            go.transform.localScale = transform.localScale;

            string lastLetter = "";

            int l = m_text.text.Length - 1;
            for (int j = 0; j < l; j++)
            {
                lastLetter += " ";
            }

            lastLetter += "" + m_text.text[m_text.text.Length - 1];

            go.GetComponent<Text>().text = "";
            go.GetComponent<Text>().text += lastLetter;
            yield return new WaitForSeconds(.2f);
        }
    }

    private static bool erpo = false;

    private IEnumerator EraseText()
    {
        
        yield return new WaitForSeconds(5.0f);
        yield return SetInstructionSprite.ms_instance.WaitForOpenMouth();
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        yield return SetInstructionSprite.ms_instance.FadeOutTutorialIcons();

        SetInstructionSprite.StartWaitingForEmotion(SetInstructionSprite.FaceState.BLINK);


        Debug.Log("Setting s_eraseInput to 0");
        s_eraseInput = 0;
        m_erase = false;
        s_eraseInput = 0;
        while (m_text.text.Length > 0 && s_totalErased < c_erasedNeeded)
        {
            Debug.Log("s_eraseInput:" + s_eraseInput);
            if (s_eraseInput > 0)
            {
                //erase one word
                while (m_text.text.Length > 0 && m_text.text[m_text.text.Length - 1] != ' ')
                {

                    int nClones = Random.Range(0, 3);

                    for (int i = 0; i < nClones; i++)
                    {
                        GameObject go = GameObject.Instantiate(this.gameObject);
                        go.GetComponent<WordDecay>().StartCoroutine(go.GetComponent<WordDecay>().FadeAway());
                        go.transform.position = transform.position;

                        go.transform.SetParent(transform.parent);
                        go.transform.localScale = transform.localScale;

                        string lastLetter = "";

                        int l = m_text.text.Length - 1;


                        lastLetter += m_text.text;

                        go.GetComponent<Text>().text = "";
                        go.GetComponent<Text>().text += lastLetter;

                    }
                    m_text.text = m_text.text.Substring(0, m_text.text.Length - 1);


                    yield return new WaitForSeconds(.1f / (1 + s_eraseInput));
                }

                if (m_text.text.Length > 0 && m_text.text[m_text.text.Length - 1] == ' ')
                {
                    m_text.text = m_text.text.Substring(0, m_text.text.Length - 1);
                }
                s_totalErased++;
                s_eraseInput--;
            }
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Terminating");
        yield return new WaitForSeconds(m_waitTime);
        float waitTime = .01f;
        if (m_goodEnding == true)
        {
            while (m_text.text.Length > 5)
            {

                int nClones = Random.Range(0, 2);

                for (int i = 0; i < nClones; i++)
                {
                    GameObject go = GameObject.Instantiate(this.gameObject);
                    go.GetComponent<WordDecay>().StartCoroutine(go.GetComponent<WordDecay>().FadeAway());
                    go.transform.position = transform.position;

                    go.transform.parent = transform.parent;
                    go.transform.localScale = transform.localScale;

                    string lastLetter = "";

                    int l = m_text.text.Length - 1;


                    lastLetter += m_text.text;

                    go.GetComponent<Text>().text = "";
                    go.GetComponent<Text>().text += lastLetter;

                }
                int n = 5;

                if (m_text.text.Length < n)
                {
                    n = 1;
                }

                m_text.text = m_text.text.Substring(0, m_text.text.Length - n);

                waitTime *= .5f;

                yield return new WaitForSeconds(waitTime);
            }
            yield return new WaitForSeconds(1.0f);
            SetInstructionSprite.StopWaitingForEmotion();
            StartCoroutine(SpawnText());
        }
        else if (m_lastLevel == false)
        {
            while (m_text.text.Length > 0)
            {

                int nClones = Random.Range(0, 2);

                for (int i = 0; i < nClones; i++)
                {
                    GameObject go = GameObject.Instantiate(this.gameObject);
                    go.GetComponent<WordDecay>().StartCoroutine(go.GetComponent<WordDecay>().FadeAway());
                    go.transform.position = transform.position;

                    go.transform.parent = transform.parent;
                    go.transform.localScale = transform.localScale;

                    string lastLetter = "";

                    int l = m_text.text.Length - 1;


                    lastLetter += m_text.text;

                    go.GetComponent<Text>().text = "";
                    go.GetComponent<Text>().text += lastLetter;

                }
                int n = 5;

                if (m_text.text.Length < n)
                {
                    n = 1;
                }

                m_text.text = m_text.text.Substring(0, m_text.text.Length - n);

                waitTime *= .5f;

                yield return new WaitForSeconds(waitTime);
            }

            SetInstructionSprite.StopWaitingForEmotion();

            if (m_triggerEnding)
            {
                FadeInFadeOut.FadeOut();
            }
        }
        else {
            while (m_text.text.Length > 5)
            {

                int nClones = Random.Range(0, 2);

                for (int i = 0; i < nClones; i++)
                {
                    GameObject go = GameObject.Instantiate(this.gameObject);
                    go.GetComponent<WordDecay>().StartCoroutine(go.GetComponent<WordDecay>().FadeAway());
                    go.transform.position = transform.position;

                    go.transform.parent = transform.parent;
                    go.transform.localScale = transform.localScale;

                    string lastLetter = "";

                    int l = m_text.text.Length - 1;


                    lastLetter += m_text.text;

                    go.GetComponent<Text>().text = "";
                    go.GetComponent<Text>().text += lastLetter;

                }
                int n = 5;

                if (m_text.text.Length < n)
                {
                    n = 1;
                }

                m_text.text = m_text.text.Substring(0, m_text.text.Length - n);

                waitTime *= .5f;

                yield return new WaitForSeconds(waitTime);
            }
            yield return new WaitForSeconds(1.0f);
            SetInstructionSprite.StopWaitingForEmotion();

            StartCoroutine(MutateText());
            StartCoroutine(SpawnText());
        }
    }

    private IEnumerator MutateText()
    {
        yield return new WaitForSeconds(5.0f);
        for (int i = 0; i < 1000; i++)
        {
            m_text.text = m_text.text.Insert(Random.Range(0, m_text.text.Length), "" + m_text.text[Random.Range(0, m_text.text.Length)]);
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(10.0f);
        FadeInFadeOut.FadeOut();
    }

    public void SetOrigText(string origText)
    {
        m_origText = origText;
    }

    private IEnumerator SpawnText()
    {
        while (m_text.text.Length < m_origText.Length)
        {
            float waitTime = .01f;

            int nClones = Random.Range(0, 2);

            for (int i = 0; i < nClones; i++)
            {
                GameObject go = GameObject.Instantiate(this.gameObject);
                go.GetComponent<WordDecay>().StartCoroutine(go.GetComponent<WordDecay>().FadeAway());
                go.transform.position = transform.position;

                go.transform.parent = transform.parent;
                go.transform.localScale = transform.localScale;

                string lastLetter = "";

                int l = m_text.text.Length - 1;


                lastLetter += m_text.text;

                go.GetComponent<Text>().text = "";
                go.GetComponent<Text>().text += lastLetter;

            }
            int n = 5;

            if (m_text.text.Length < n)
            {
                n = 1;
            }

            m_text.text += m_origText[m_text.text.Length];

            waitTime *= .5f;

            yield return new WaitForSeconds(waitTime);
        }
        if (m_goodEnding == false)
        {
            for (int i = 0; i < 100; i++)
            {
                yield return new WaitForSeconds(Random.Range(0.1f, 3.0f));
                GameObject go = GameObject.Instantiate(this.gameObject);
                go.GetComponent<WordDecay>().SetOrigText(m_origText);

                go.transform.parent = transform.parent;
                go.transform.localPosition = new Vector3(transform.localPosition.x + Random.Range(-500.0f, 500.0f), transform.localPosition.y + Random.Range(-500.0f, 500.0f), transform.localPosition.z + Random.Range(-100.0f, 100.0f));
                go.transform.localScale = transform.localScale;

                go.GetComponent<WordDecay>().StartCoroutine(go.GetComponent<WordDecay>().FadeIn());
                go.GetComponent<WordDecay>().StartCoroutine(go.GetComponent<WordDecay>().MutateText());


                string lastLetter = "";

                int l = m_text.text.Length - 1;


                lastLetter += m_text.text;

                go.GetComponent<Text>().text = "";
                go.GetComponent<Text>().text += lastLetter;
            }
        }
        else {

            if (m_triggerEnding)
            {
                FadeInFadeOut.FadeOut();
            }
        }
    }


    public IEnumerator FadeAway()
    {
        Color startColor = new Color(m_text.color.r, m_text.color.g, m_text.color.b, Random.Range(.1f, .5f));
        Color endColor = new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), 0.0f);
        m_rb.gravityScale = Random.Range(.1f, 2.0f);
        m_rb.AddTorque(Random.Range(-20.0f, 20.0f));
        m_rb.AddForce(new Vector2(Random.Range(-800.0f, 800.0f), Random.Range(-800.0f, 800.0f)));

        float interpTime = Random.Range(.1f, 5.0f);
        float tPassed = 0.0f;

        while (tPassed < interpTime)
        {
            m_text.color = Color.Lerp(startColor, endColor, tPassed / interpTime);
            tPassed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        Destroy(gameObject);

    }

}
