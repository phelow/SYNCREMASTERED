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
		DialogueManager.Emotion em = DialogueChoiceTracker.GetMostExpressedEmotion ();
		yield return new WaitForSeconds(2.0f);
		bool waiting = true;
		DialogueManager.Emotion emotion = DialogueManager.Emotion.Joy;
		ImageResultsListener.TakeSample ();
		while (waiting) {
			if (DialogueManager.CanGetCurrentEmotion ()) {
				emotion = DialogueManager.GetCurrentEmotion ();
				waiting = false;
				DialogueManager.DisableCurrentEmotion ();
			}
			yield return new WaitForEndOfFrame ();
		}
		SetInstructionSprite.StopWaitingForEmotion ();

        yield return new WaitForSeconds(2f);

		DialogueManager.Main.DisplayScene5Text (Scene5Events.OnPlayerWaiting,false,em,true);
		waiting = true;
		emotion = DialogueManager.Emotion.Joy;

		yield return new WaitForSeconds(4.0f);
        DialogueManager.Main.FadeOut();
        yield return new WaitForSeconds(2f);

    
        /*Debug.Log (80);
		ImageResultsListener.TakeSample ();
		while (waiting) {
			if (DialogueManager.CanGetCurrentEmotion ()) {
				emotion = DialogueManager.GetCurrentEmotion ();
				waiting = false;
				DialogueManager.DisableCurrentEmotion ();
			}
			yield return new WaitForEndOfFrame ();
		}
		Debug.Log (90);
		SetInstructionSprite.StopWaitingForEmotion ();

		//DialogueManager.Main.FadeOut();
		//yield return new WaitForSeconds(1.0f);
		
		yield return new WaitForSeconds(2.0f);
		waiting = true;
		emotion = DialogueManager.Emotion.Joy;

		yield return new WaitForSeconds(1.0f);
		ImageResultsListener.TakeSample ();*/



		float t = 0.0f;

		float c_maxMoveTime = 3.0f;

		while (waiting && t < c_maxMoveTime) {
			t += Time.deltaTime * (1/Time.timeScale);
			if (DialogueManager.CanGetCurrentEmotion ()) {
				emotion = DialogueManager.GetCurrentEmotion ();
				waiting = false;
				DialogueManager.DisableCurrentEmotion ();
			}
			yield return new WaitForEndOfFrame ();
		}
		SetInstructionSprite.StopWaitingForEmotion ();

		//yield return new WaitForSeconds(2.0f);

		/*m_rigidbody.velocity = Vector2.zero;
		m_instance.m_spriteRenderer.sprite = m_instance.m_lookingAtPlayer;*/
		StartCoroutine (SlowDownTime ());

		StartCoroutine (FadeInLover ());
		t = 0.0f;
		while (t < 7.0f ) {
			t += Time.deltaTime * (1/Time.timeScale);
			yield return new WaitForEndOfFrame ();
		}
		DialogueManager.Main.DisplayScene5Text (Scene5Events.OnLoverAppear,true,em,true,DialogueManager.TextType.Dialog);
		t = 0.0f;
		while (t < 7.0f ) {
			t += Time.deltaTime * (1/Time.timeScale);
			yield return new WaitForEndOfFrame ();
		}

        DialogueManager.Main.FadeOut();
		t = 0.0f;
		while (t < 1.0f) {
			t += Time.deltaTime * (1/Time.timeScale);
			yield return new WaitForEndOfFrame ();
		}

		ImageResultsListener.TakeSample ();
		t = 0.0f;
		while (t < 7.0f ) {
			t += Time.deltaTime * (1/Time.timeScale);
			yield return new WaitForEndOfFrame ();
		}
		waiting = true;
		emotion = DialogueManager.Emotion.Joy;
		t = 0.0f;
		while (waiting) {
			if (DialogueManager.CanGetCurrentEmotion ()) {
				emotion = DialogueManager.GetCurrentEmotion ();
				waiting = false;
				DialogueManager.DisableCurrentEmotion ();
			}
			yield return new WaitForEndOfFrame ();
		}
		SetInstructionSprite.StopWaitingForEmotion ();
		
		t = 0.0f;
		while (t < 4.0f ) {
			t += Time.deltaTime * (1/Time.timeScale);
			yield return new WaitForEndOfFrame ();
		}
		//DialogueManager.Main.ActivateSlowmo ();
		t = 0.0f;
		while (t < 4.0f ) {
			t += Time.deltaTime * (1/Time.timeScale);
			yield return new WaitForEndOfFrame ();
		}

		DialogueManager.DisableCurrentEmotion ();

		DialogueManager.Main.DisplayScene5Text (Scene5Events.OnPlayerDeciding,true,em,true);

		t = 0.0f;
		while (t < 4.0f ) {
			t += Time.deltaTime * (1/Time.timeScale);
			yield return new WaitForEndOfFrame ();
		}

        DialogueManager.Main.FadeOut();

		t = 0.0f;
		while (t < 4.0f ) {
			t += Time.deltaTime * (1/Time.timeScale);
			yield return new WaitForEndOfFrame ();
		}

		waiting = true;
		emotion = DialogueManager.Emotion.Joy;
		ImageResultsListener.TakeSample ();
		while (waiting) {
			if (DialogueManager.CanGetCurrentEmotion ()) {
				emotion = DialogueManager.GetCurrentEmotion ();
				waiting = false;
				DialogueManager.DisableCurrentEmotion ();
			}
			yield return new WaitForEndOfFrame ();
		}
		SetInstructionSprite.StopWaitingForEmotion ();
        //DialogueManager.Main.FadeOut();

        yield return new WaitForSeconds(4.0f);

        DialogueManager.Main.DisplayScene5Text (Scene5Events.OnCommandToSmile,true,em,true,DialogueManager.TextType.Command);

		m_waitingForSmile = true;
		waiting = true;
		emotion = DialogueManager.Emotion.Joy;
		ImageResultsListener.TakeSample ();
		t = 0.0f;
		while (t < 7.0f) {
			t += Time.deltaTime * (1/Time.timeScale);
			yield return new WaitForEndOfFrame ();
		}
		while (waiting) {
			if (DialogueManager.CanGetCurrentEmotion ()) {
				emotion = DialogueManager.GetCurrentEmotion ();
				waiting = false;
				DialogueManager.DisableCurrentEmotion ();
			}
			yield return new WaitForEndOfFrame ();
		}
		SetInstructionSprite.StopWaitingForEmotion ();

        yield return new WaitForSeconds(1f);

		DialogueManager.Main.FadeOut();

		yield return new WaitForSeconds(1f);


		if (emotion != DialogueManager.Emotion.Joy) {
			StartCoroutine (FastSpeedUpTime ());

			DialogueManager.Main.DisplayBadEndingText (BadEndingEvents.OnRefuseToSmile,true,em,true);
			waiting = true;
			emotion = DialogueManager.Emotion.Joy;
			ImageResultsListener.TakeSample ();
			while (waiting) {
				if (DialogueManager.CanGetCurrentEmotion ()) {
					emotion = DialogueManager.GetCurrentEmotion ();
					waiting = false;
					DialogueManager.DisableCurrentEmotion ();
				}
				yield return new WaitForEndOfFrame ();
			}
			SetInstructionSprite.StopWaitingForEmotion ();
			DialogueManager.Main.FadeOut();t = 0.0f;
			while (t < 3.0f ) {
				t += Time.deltaTime * (1/Time.timeScale);
				yield return new WaitForEndOfFrame ();
			}
			m_instance.m_spriteRenderer.sprite = m_instance.m_walkingAway;
			StartCoroutine (LoverFadeOut ());
			//m_rigidbody.AddForce (Vector2.right * c_impulseForce);
			DialogueManager.Main.DisplayBadEndingText (BadEndingEvents.OnIvyResponds,true,em,true);
			t = 0.0f;
			while (t < 4.0f) {
				t += Time.deltaTime * (1/Time.timeScale);
				yield return new WaitForEndOfFrame ();
			}
			waiting = true;
			emotion = DialogueManager.Emotion.Joy;
			ImageResultsListener.TakeSample ();
			while (waiting) {
				if (DialogueManager.CanGetCurrentEmotion ()) {
					emotion = DialogueManager.GetCurrentEmotion ();
					waiting = false;
					DialogueManager.DisableCurrentEmotion ();
				}
				yield return new WaitForEndOfFrame ();
			}
			SetInstructionSprite.StopWaitingForEmotion ();
			DialogueManager.Main.FadeOut();t = 0.0f;
			while (t < 3.0f ) {
				t += Time.deltaTime * (1/Time.timeScale);
				yield return new WaitForEndOfFrame ();
			}
			DialogueManager.Main.DisplayBadEndingText (BadEndingEvents.OnLoverGone,true,em,true);
			t = 0.0f;
			while (t < 4.0f ) {
				t += Time.deltaTime * (1/Time.timeScale);
				yield return new WaitForEndOfFrame ();
			}
			waiting = true;
			emotion = DialogueManager.Emotion.Joy;
			ImageResultsListener.TakeSample ();
			while (waiting) {
				if (DialogueManager.CanGetCurrentEmotion ()) {
					emotion = DialogueManager.GetCurrentEmotion ();
					waiting = false;
					DialogueManager.DisableCurrentEmotion ();
				}
				yield return new WaitForEndOfFrame ();
			}
			SetInstructionSprite.StopWaitingForEmotion ();
			DialogueManager.Main.FadeOut();
			yield return new WaitForSeconds(3.0f);
			FadeInFadeOut.FadeOut (26);

		} else {
			StartCoroutine (SpeedUpTime ());
			m_instance.m_spriteRenderer.sprite = m_instance.m_outstretchingHand;
			DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnPlayerSmile,false,em,true);
			t = 0.0f;
			while (t < 4.0f ) {
				t += Time.deltaTime * (1/Time.timeScale);
				yield return new WaitForEndOfFrame ();
			}
			waiting = true;
			emotion = DialogueManager.Emotion.Joy;
			ImageResultsListener.TakeSample ();
			while (waiting) {
				if (DialogueManager.CanGetCurrentEmotion ()) {
					emotion = DialogueManager.GetCurrentEmotion ();
					waiting = false;
					DialogueManager.DisableCurrentEmotion ();
				}
				yield return new WaitForEndOfFrame ();
			}
			SetInstructionSprite.StopWaitingForEmotion ();
			DialogueManager.Main.FadeOut();
			t = 0.0f;
			while (t < 3.0f ) {
				t += Time.deltaTime * (1/Time.timeScale);
				yield return new WaitForEndOfFrame ();
			}
			DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnAlexTalks,false,em,true,DialogueManager.TextType.Dialog);
			t = 0.0f;
			while (t < 4.0f) {
				t += Time.deltaTime * (1/Time.timeScale);
				yield return new WaitForEndOfFrame ();
			}
			waiting = true;
			emotion = DialogueManager.Emotion.Joy;
			ImageResultsListener.TakeSample ();
			while (waiting) {
				if (DialogueManager.CanGetCurrentEmotion ()) {
					emotion = DialogueManager.GetCurrentEmotion ();
					waiting = false;
					DialogueManager.DisableCurrentEmotion ();
				}
				yield return new WaitForEndOfFrame ();
			}
			SetInstructionSprite.StopWaitingForEmotion ();
			DialogueManager.Main.FadeOut();t = 0.0f;
			while (t < 3.0f) {
				t += Time.deltaTime * (1/Time.timeScale);
				yield return new WaitForEndOfFrame ();
			}
			DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnIvyResponds1, false,em,true,DialogueManager.TextType.Dialog);t = 0.0f;
			while (t < 4.0f ) {
				t += Time.deltaTime * (1/Time.timeScale);
				yield return new WaitForEndOfFrame ();
			}
			waiting = true;
			emotion = DialogueManager.Emotion.Joy;
			ImageResultsListener.TakeSample ();
			while (waiting) {
				if (DialogueManager.CanGetCurrentEmotion ()) {
					emotion = DialogueManager.GetCurrentEmotion ();
					waiting = false;
					DialogueManager.DisableCurrentEmotion ();
				}
				yield return new WaitForEndOfFrame ();
			}
			SetInstructionSprite.StopWaitingForEmotion ();
			DialogueManager.Main.FadeOut();t = 0.0f;
			while (t < 3.0f) {
				t += Time.deltaTime * (1/Time.timeScale);
				yield return new WaitForEndOfFrame ();
			}
			DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnIvyResponds2, false,em,true,DialogueManager.TextType.Dialog);t = 0.0f;
			while (t < 4.0f) {
				t += Time.deltaTime * (1/Time.timeScale);
				yield return new WaitForEndOfFrame ();
			}
			waiting = true;
			emotion = DialogueManager.Emotion.Joy;
			ImageResultsListener.TakeSample ();
			while (waiting) {
				if (DialogueManager.CanGetCurrentEmotion ()) {
					emotion = DialogueManager.GetCurrentEmotion ();
					waiting = false;
					DialogueManager.DisableCurrentEmotion ();
				}
				yield return new WaitForEndOfFrame ();
			}
			SetInstructionSprite.StopWaitingForEmotion ();
			DialogueManager.Main.FadeOut();
			yield return new WaitForSeconds(3.0f);
			FadeInFadeOut.FadeOut ();

		}


	}

	private IEnumerator LoverFadeOut(){
		while(true){
			m_instance.m_spriteRenderer.color = Color.Lerp (m_instance.m_spriteRenderer.color, m_instance.m_invisColor, Time.deltaTime);
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
