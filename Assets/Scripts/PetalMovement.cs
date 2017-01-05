using UnityEngine;
using Affdex;
using System.Collections;

public class PetalMovement : MonoBehaviour
{
    private static PetalMovement s_instance;

    float petalSize = 1.3f;
    public Transform leftPosition;
    public Transform rightPosition;
    public Rigidbody2D m_rb;

    public PetalWaypoint m_nextTarget;

    [SerializeField]
    private PetalWaypoint m_targetA;
    [SerializeField]
    private PetalWaypoint m_targetB;
    [SerializeField]
    private PetalWaypoint m_targetC;
    [SerializeField]
    private PetalWaypoint m_targetD;
    [SerializeField]
    private PetalWaypoint m_targetE;

    public static PetalMovement ms_instance;

    private static bool interactible;

    const float ms_windForce = 20.0f;
    const float ms_downDraftForce = 10.0f;
    const float c_horizontalWind = 1.2f;

    const float c_startSize = 1.0f;
    const float c_endSize = .5f;
    const float c_resizeDistance = 30.0f;

    const float c_startingWaitTime = 18.0f;

    private float ms_life = 0.0f;

    [SerializeField]
    private Animator m_animator;

    private IEnumerator m_bringToLife;

    [SerializeField]
    private float m_animationSpeedModifier = 3.0f;

    public static bool m_alive = false;
    public static bool m_moving = false;

    [SerializeField]
    private bool m_goodEnding = false;

    private bool notLerping = true;
    public GameObject m_targetPetal;

    // Use this for initialization
    void Start()
    {
        ms_instance = this;
        s_instance = this;
        interactible = false;
        m_rb.AddTorque(Random.Range(.1f, 1.0f)); //add a random tortional force

        //Show the beginning dialogue
        if (m_goodEnding == false)
        {
            StartCoroutine(ShowBeginningDialogue());
        }
        else {
            StartCoroutine(ShowGoodEndingDialogue());
        }
    }
    static Vector2 vel;

    private IEnumerator ShowGoodEndingDialogue()
    {

        DialogueManager.Emotion em = DialogueManager.Emotion.Anger;

        DialogueManager.Main.DisplayGoodEndingText(GoodEndingEvents.OnPetalBeginningToFall, false, em);
        yield return new WaitForSeconds(10.0f);
        DialogueManager.Main.DisplayGoodEndingText(GoodEndingEvents.OnPetalFalling, false, em);
        yield return new WaitForSeconds(10.0f);
        DialogueManager.Main.DisplayGoodEndingText(GoodEndingEvents.OnPetalLanding, false, em);
        yield return new WaitForSeconds(10.0f);
        DialogueManager.Main.DisplayGoodEndingText(GoodEndingEvents.OnPetalDying1, false, em);
        yield return new WaitForSeconds(10.0f);
        DialogueManager.Main.DisplayGoodEndingText(GoodEndingEvents.OnPetalDying2, false, em);
        yield return new WaitForSeconds(10.0f);

        FadeInFadeOut.FadeOut();
    }

    //Handle the beginning dialogue sequence
    IEnumerator ShowBeginningDialogue()
    {
        yield return new WaitForSeconds(c_startingWaitTime);

        yield return DialogueManager.Main.Scene0Coroutine(Scene0Events.OnSceneLoad, true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();


        yield return BringToLife();
        yield return DialogueManager.Main.Scene0Coroutine(Scene0Events.OnCommandToSmile, true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

        yield return DialogueManager.Main.Scene0Coroutine(Scene0Events.OnDeadPetal, true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();


        yield return FlyToTarget(m_targetA);
        yield return DialogueManager.Main.Scene0Coroutine(Scene0Events.OnPetalRising, true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

        yield return FlyToTarget(m_targetB);
        yield return DialogueManager.Main.Scene0Coroutine(Scene0Events.OnPetalInTree, true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

        yield return FlyToTarget(m_targetC);
        yield return DialogueManager.Main.Scene0Coroutine(Scene0Events.PromptOnSceneEnd1, true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

        yield return FlyToTarget(m_targetD);
        yield return DialogueManager.Main.Scene0Coroutine(Scene0Events.OnSceneEnd2, true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

        yield return FlyToTarget(m_targetD);
        FadeInFadeOut.FadeOut();
    }

    private IEnumerator FlyToTarget(PetalWaypoint target)
    {
        m_nextTarget = target;

        while (Vector2.Distance(target.transform.position, this.transform.position) > 5.0f)
        {
            StartCoroutine(AddWind());
            yield return new WaitForSeconds(Random.Range(0.3f, 2.0f));
        }
    }

    private IEnumerator LerpToPosition()
    {
        Destroy(m_rb);
        m_moving = true;
        float moveTime = 0;
        float totalMoveTime = 6.0f;
        while (moveTime < totalMoveTime)
        {
            moveTime += Time.deltaTime;
            Debug.Log("Lerping to target petal");
            if (m_targetPetal != null)
            {
                transform.position = Vector3.Slerp(transform.position, m_targetPetal.transform.position, moveTime / totalMoveTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, m_targetPetal.transform.rotation, moveTime / totalMoveTime);
            }
            yield return new WaitForEndOfFrame();
        }

        yield return new WaitForSeconds(c_startingWaitTime);

        Debug.Log("End Scene");
    }

    public IEnumerator AddWind()
    {
        Debug.Log("Adding Wind");
        for (int i = 0; i < 10; i++)
        {
            float leftDist = Vector3.Distance(new Vector3(leftPosition.position.x, leftPosition.position.y, 0), new Vector3(transform.position.x, transform.position.y, 0));
            float rightDist = Vector3.Distance(new Vector3(rightPosition.position.x, rightPosition.position.y, 0), new Vector3(transform.position.x, transform.position.y, 0));

            float targetDistance = float.PositiveInfinity;

            if (m_nextTarget != null)
            {
                targetDistance = Vector3.Distance(m_nextTarget.m_transform.position, transform.position);
            }
            if (m_nextTarget != null && m_nextTarget.m_nextPetalWaypoint == null)
            {
                m_rb.velocity *= Mathf.Lerp(1, .3f, 1 / Vector3.Distance(m_nextTarget.m_transform.position, transform.position));
            }

            //TODO: End level when final target is reached


            if (m_nextTarget != null)
            {
                Vector2 targetVec = new Vector2(m_nextTarget.transform.position.x, m_nextTarget.transform.position.y) - new Vector2(transform.position.x, transform.position.y);
                //Blow from whichever side we are closer too.
                if (leftDist < rightDist)
                {

                    GameObject[] backgroundPetals = GameObject.FindGameObjectsWithTag("Background Petal");
                    Vector2 wind = ((targetVec + Vector2.right * c_horizontalWind) * ms_windForce * .1f);
                    foreach (GameObject bg in backgroundPetals)
                    {
                        bg.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(wind.x * .9f, wind.x * 1.1f), Random.Range(wind.y * .9f, wind.y * 1.1f)));
                    }

                    m_rb.AddForce(wind * Mathf.Lerp(.5f, 1.0f, SceneZeroListener.s_smileMultiplier));
                }
                else {
                    GameObject[] backgroundPetals = GameObject.FindGameObjectsWithTag("Background Petal");
                    Vector2 wind = ((targetVec + Vector2.left * c_horizontalWind) * ms_windForce * .1f);
                    foreach (GameObject bg in backgroundPetals)
                    {
                        bg.GetComponent<Rigidbody2D>().AddForce(new Vector2(Random.Range(wind.x * .9f, wind.x * 1.1f), Random.Range(wind.y * .9f, wind.y * 1.1f)));
                    }
                    m_rb.AddForce(wind * Mathf.Lerp(.5f, 1.0f, SceneZeroListener.s_smileMultiplier));
                }
            }
            yield return new WaitForEndOfFrame();
        }

    }


    public static void freezeAnimation()
    {
        s_instance.m_animator.speed = 0.0f;
        s_instance.m_animator.SetBool("Alive", false);
    }

    private IEnumerator BringToLife()
    {
        m_animator.SetBool("Alive", false);

        m_animator.speed = 0.0f;
        float t = 0.0f;
        while (t < 5.0f)
        {
            t += Time.deltaTime;
            if (ms_life < 0.01f)
            {
                ms_life = 0.01f;
            }

            m_animator.speed = ((SceneZeroListener.s_smileMultiplier) / 2.0f) * m_animationSpeedModifier;

            Debug.Log(m_animator.speed);

            yield return new WaitForEndOfFrame();
        }
    }

    //Handles displaying the last two lines of dialogue at the end of the scene
    IEnumerator ShowEndDialogue()
    {
        bool waiting = true;
        yield return DialogueManager.Main.Scene0Coroutine(Scene0Events.PromptOnSceneEnd1, true);
        yield return new WaitForSeconds(3f);
        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds(3f);

        yield return new WaitForSeconds(3.0f);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds(4.0f);



        yield return DialogueManager.Main.Scene0Coroutine(Scene0Events.OnSceneEnd2, false);
        yield return new WaitForSeconds(6.0f);

        FadeInFadeOut.FadeOut();
    }
}
