using UnityEngine;
using Affdex;
using System.Collections;

public class SmileToAdvanceScene : MonoBehaviour {
	private static SmileToAdvanceScene m_instance;
	private const float c_left = -13.0f;
	private const float c_right = 3.0f;
	private const float c_impulseForce = 50.0f;
	private static Transform m_transform;

	private static bool m_sceneCompleted = false;

	private static bool m_waitingForSmile = false;

	private Color m_invisColor = new Color(0,0,0,0);

	[SerializeField]private SpriteRenderer m_spriteRenderer;

	[SerializeField]private Sprite m_lookingAtPlayer;
	[SerializeField]private Sprite m_walkingAway;
	[SerializeField]private Sprite m_outstretchingHand;

	[SerializeField]private SpriteRenderer m_pedestrian;

	[SerializeField]private Rigidbody2D m_rigidbody; 

	[SerializeField]private ParticleSystem m_ps;

	// Use this for initialization
	void Start () {
		m_transform = transform;
		m_instance = this;
		StartCoroutine (AddForceAfterTime ());
	}

	private IEnumerator SlowDownTime(){
		float t = 0;
		Color origColor = m_pedestrian.color;
		while (t < 5.0f) {
			t += Time.deltaTime* 1.0f;
			m_ps.gravityModifier = Mathf.Lerp (.1f, .0001f, t / 5.0f);
			m_pedestrian.color = Color.Lerp (origColor, m_invisColor, t / 5.0f);
			yield return new WaitForEndOfFrame ();
		}
	}

	private IEnumerator SpeedUpTime(){
		float t = 0;
		while (t < 5.0f) {
			t += Time.deltaTime * 1.0f;
			m_ps.gravityModifier = Mathf.Lerp (.0001f, 0.1f, t / 5.0f);
			yield return new WaitForEndOfFrame ();
		}

	}

	private IEnumerator FadeInLover(){

		float t = 0;
		Color origColor = m_pedestrian.color;
		while (t < 3.0f) {
			t += Time.deltaTime* 1.0f;
			m_spriteRenderer.color = Color.Lerp (m_invisColor,origColor, t / 3.0f);
			yield return new WaitForEndOfFrame ();
		}
	}

	private IEnumerator FastSpeedUpTime(){
		float t = 0;
		while (t < .5f) {
			t += Time.deltaTime * 1.0f;
			Time.timeScale = Mathf.Lerp (.1f, 1.0f, t / .5f);
			yield return new WaitForEndOfFrame ();
		}

	}

	private IEnumerator AddForceAfterTime(){
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
		yield return DialogueManager.Main.DisplayScene5Text (Scene5Events.OnPlayerWaiting,false);
		

		yield return new WaitForSeconds(4.0f);
        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds(2f);
		float t = 0.0f;

		float c_maxMoveTime = 3.0f;

        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        StartCoroutine (SlowDownTime ());

		yield return FadeInLover ();
		
		yield return DialogueManager.Main.DisplayScene5Text (Scene5Events.OnLoverAppear,true);

        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();


        yield return DialogueManager.Main.DisplayScene5Text (Scene5Events.OnPlayerDeciding,true);

        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

        yield return DialogueManager.Main.DisplayScene5Text (Scene5Events.OnCommandToSmile,true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();



        if (DialogueManager.GetCurrentEmotion() != DialogueManager.Emotion.Joy) {
			StartCoroutine (FastSpeedUpTime ());

			yield return DialogueManager.Main.DisplayBadEndingText (BadEndingEvents.OnRefuseToSmile,false);

            
			m_instance.m_spriteRenderer.sprite = m_instance.m_walkingAway;
            yield return LoverFadeOut();

            yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
            yield return DialogueManager.Main.DisplayBadEndingText (BadEndingEvents.OnIvyResponds, false);


            yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
            yield return DialogueManager.Main.DisplayBadEndingText (BadEndingEvents.OnLoverGone, false);

            yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
            FadeInFadeOut.FadeOut (28);

		} else {
			StartCoroutine (SpeedUpTime ());
			m_instance.m_spriteRenderer.sprite = m_instance.m_outstretchingHand;


            yield return DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnPlayerSmile,false);


            yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
            yield return DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnAlexTalks,false);

            yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
            yield return DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnIvyResponds1, false);

            yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
            yield return DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnIvyResponds2, false);
			FadeInFadeOut.FadeOut ();

		}


	}

	private IEnumerator LoverFadeOut(){
        float t = 0.0f;
		while(t < 5.0f){
            t += Time.deltaTime;
			m_instance.m_spriteRenderer.color = Color.Lerp (m_instance.m_spriteRenderer.color, m_instance.m_invisColor, t/5.0f);
			yield return new WaitForEndOfFrame();
		}
	}

	// Update is called once per frame
	void Update () {
		if (transform.position.x > c_right) {
			PlayerPrefs.SetInt ("FailedGame", 1);
		}
	}

	private static IEnumerator CompleteScene(){
		yield return new WaitForEndOfFrame ();
		if (m_sceneCompleted == false && m_waitingForSmile == true) {
			m_sceneCompleted = true;
	//		m_instance.m_rigidbody.AddForce (Vector2.right * c_impulseForce);
		}
	}

	public static void SmileAtLover(){
		if (m_transform != null && m_transform.position.x < c_right && m_transform.position.x > c_left) {
			m_instance.StartCoroutine (CompleteScene());
		}
	}
}
