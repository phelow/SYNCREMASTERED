using UnityEngine;
using Affdex;
using System.Collections;

public class PetalMovement : MonoBehaviour {
	private static PetalMovement s_instance;

	float petalSize = 1.3f;
	public Transform leftPosition;
	public Transform rightPosition;
	public Rigidbody2D m_rb;

	public PetalWaypoint m_nextTarget;

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

	[SerializeField] private Animator m_animator;

	private IEnumerator m_bringToLife;
	private IEnumerator m_remindToSmile;

	[SerializeField]private float m_animationSpeedModifier = 3.0f;

	public static bool m_alive = false;
	public static bool m_moving = false;

	[SerializeField]private bool m_goodEnding = false;

	private bool notLerping = true;
	private bool m_locked = true;
	public GameObject m_targetPetal;

	// Use this for initialization
	void Start () {
		ms_instance = this;
		s_instance = this;
		interactible = false;
		m_rb.AddTorque (Random.Range (.1f, 1.0f)); //add a random tortional force
		m_bringToLife = BringToLife();
		StartCoroutine(m_bringToLife);


		//Show the beginning dialogue
		if (m_goodEnding == false) {
			StartCoroutine (ShowBeginningDialogue ());
		} else {
			StartCoroutine (ShowGoodEndingDialogue ());
		}
	}
	static Vector2 vel;
	public static void FreezePetal(){
		try{
			vel = ms_instance.m_rb.velocity;
			ms_instance.m_rb.velocity = Vector2.zero;
			ms_instance.m_rb.isKinematic = true;
		}
		catch{
		}
	}

	public static void UnfreezePetal(){
		try{
			ms_instance.m_rb.isKinematic = false;
			ms_instance.m_rb.velocity = vel;
		}
		catch{

		}
	}

	public IEnumerator RemindToSmile(){

		bool hasSmiled = true;

		while (true) {
			hasSmiled = true;
			while (hasSmiled == true) {
				//sample the smiling
				hasSmiled = false;
				for (int i = 0; i < 10; i++) {
					if (SceneZeroListener.IsEmotionalIntensitySufficient ()) {
						hasSmiled = true;
					}

					yield return new WaitForSeconds (.5f);

				}

			}

			while (DialogueManager.IsInUse ()) {
				yield return new WaitForEndOfFrame ();
			}

			DialogueManager.Main.DisplayScene0Text (Scene0Events.OnCommandToSmile, true);

			while (SceneZeroListener.IsEmotionalIntensitySufficient () == false) {
				yield return new WaitForEndOfFrame ();
			}
			DialogueManager.Main.FadeOut ();
			yield return new WaitForEndOfFrame ();
		}
	}

	public static void Grow(){

	}

	public static void Shrink(){

	}

	private void Unlock(){
		m_locked = false;
	}

	private IEnumerator ShowGoodEndingDialogue(){

		DialogueManager.Emotion em = DialogueChoiceTracker.GetMostExpressedEmotion ();

		DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnPetalBeginningToFall,false,em);
		yield return new WaitForSeconds (10.0f);
		DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnPetalFalling,false,em);
		yield return new WaitForSeconds (10.0f);
		DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnPetalLanding,false,em);
		yield return new WaitForSeconds (10.0f);
		DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnPetalDying1,false,em);
		yield return new WaitForSeconds (10.0f);
		DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnPetalDying2,false,em);
		yield return new WaitForSeconds (10.0f);

		FadeInFadeOut.FadeOut ();
		m_newMutex = true;
	}
	bool m_newMutex = false;
	//Handle the beginning dialogue sequence
	IEnumerator ShowBeginningDialogue()
	{
		yield return new WaitForSeconds (c_startingWaitTime);
		ImageResultsListener.TakeSample ();
		bool waiting = true;
		DialogueManager.Emotion emotion = DialogueManager.Emotion.Joy;
		while (waiting) {
			if (DialogueManager.CanGetCurrentEmotion ()) {
				emotion = DialogueManager.GetCurrentEmotion ();
				waiting = false;
				DialogueManager.DisableCurrentEmotion ();
			}
			yield return new WaitForEndOfFrame ();
		}
		SetInstructionSprite.StopWaitingForEmotion ();
		DialogueManager.Main.FadeOut ();
		yield return new WaitForSeconds (2.0f);
		Debug.Log (147);
		DialogueManager.Main.DisplayScene0Text(Scene0Events.OnSceneLoad,true,emotion);
		yield return new WaitForSeconds (3.0f);
		DialogueManager.Main.FadeOut();
		yield return new WaitForSeconds(2.0f);
		ImageResultsListener.TakeSample ();
		Debug.Log (150);
		waiting = true;
		emotion = DialogueManager.Emotion.Joy;
		while (waiting) {
			if (DialogueManager.CanGetCurrentEmotion ()) {
				emotion = DialogueManager.GetCurrentEmotion ();
				waiting = false;
				DialogueManager.DisableCurrentEmotion ();
			}
			yield return new WaitForEndOfFrame ();
		}
		Debug.Log (161);
		SetInstructionSprite.StopWaitingForEmotion ();
		DialogueManager.Main.FadeOut ();
		DialogueManager.Main.DisplayScene0Text(Scene0Events.OnCommandToSmile, true,emotion);

		yield return new WaitForSeconds (4.0f);
		//SceneZeroListener.SetNotEmotionallySufficient ();
		/*while (SceneZeroListener.IsEmotionalIntensitySufficient () == false) {
			yield return new WaitForEndOfFrame ();
		}*/
		DialogueManager.Main.FadeOut ();


		m_remindToSmile = RemindToSmile ();
		StartCoroutine (m_remindToSmile);
		//ImageResultsListener.StartContinousSample ();
		Debug.Log (175);
		m_newMutex = true;
	}

	private IEnumerator LerpToPosition(){
		Destroy (m_rb);
		StopCoroutine (m_remindToSmile);
		m_moving = true;
		float moveTime = 0;
		float totalMoveTime = 6.0f;
		while (moveTime < totalMoveTime) {
			moveTime += Time.deltaTime;
			Debug.Log("Lerping to target petal");
			if (m_targetPetal != null) {
				transform.position = Vector3.Slerp (transform.position, m_targetPetal.transform.position, moveTime / totalMoveTime);
				transform.rotation = Quaternion.Slerp (transform.rotation, m_targetPetal.transform.rotation, moveTime / totalMoveTime);
			}
			yield return new WaitForEndOfFrame ();
		}

		yield return new WaitForSeconds (c_startingWaitTime);

		Debug.Log ("End Scene");
	}

	public IEnumerator AddWind(){
		//Determine if we are closer to the left or right side of the screen
		if (m_locked == false && m_goodEnding == false) {
			Debug.Log ("192");
			for (int i = 0; i < 10; i++) {
				float leftDist = Vector3.Distance (new Vector3 (leftPosition.position.x, leftPosition.position.y, 0), new Vector3 (transform.position.x, transform.position.y, 0));
				float rightDist = Vector3.Distance (new Vector3 (rightPosition.position.x, rightPosition.position.y, 0), new Vector3 (transform.position.x, transform.position.y, 0));

				float targetDistance = float.PositiveInfinity;

				if (m_nextTarget != null) {
					targetDistance = Vector3.Distance (m_nextTarget.m_transform.position, transform.position);
				}
				if (m_nextTarget != null && m_nextTarget.m_nextPetalWaypoint == null) {
					m_rb.velocity *= Mathf.Lerp (1, .3f, 1 / Vector3.Distance (m_nextTarget.m_transform.position, transform.position));
				}

				//TODO: End level when final target is reached


				if (m_nextTarget != null) {
					Vector2 targetVec = new Vector2 (m_nextTarget.transform.position.x, m_nextTarget.transform.position.y) - new Vector2 (transform.position.x, transform.position.y);
					//Blow from whichever side we are closer too.
					if (leftDist < rightDist) {

						GameObject[] backgroundPetals = GameObject.FindGameObjectsWithTag ("Background Petal");
						Vector2 wind = ((targetVec + Vector2.right * c_horizontalWind) * ms_windForce * .1f);
						foreach (GameObject bg in backgroundPetals) {
							bg.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range (wind.x * .9f, wind.x * 1.1f), Random.Range (wind.y * .9f, wind.y * 1.1f)));
						}

						m_rb.AddForce (wind * Mathf.Lerp (.5f, 1.0f, SceneZeroListener.s_smileMultiplier));
					} else {
						GameObject[] backgroundPetals = GameObject.FindGameObjectsWithTag ("Background Petal");
						Vector2 wind = ((targetVec + Vector2.left * c_horizontalWind) * ms_windForce * .1f);
						foreach (GameObject bg in backgroundPetals) {
							bg.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (Random.Range (wind.x * .9f, wind.x * 1.1f), Random.Range (wind.y * .9f, wind.y * 1.1f)));
						}
						m_rb.AddForce (wind * Mathf.Lerp (.5f, 1.0f, SceneZeroListener.s_smileMultiplier));
					}
				}
				yield return new WaitForEndOfFrame ();
			}
		}
	}

	void AddDownDraft(){

		Debug.Log ("Down draft");

		GameObject[] backgroundPetals = GameObject.FindGameObjectsWithTag ("Background Petal");
		Vector2 wind = (Vector2.down * ms_windForce * ms_downDraftForce);
		foreach (GameObject bg in backgroundPetals) {
			bg.GetComponent<Rigidbody2D> ().AddForce(new Vector2(Random.Range(wind.x*.9f,wind.x*1.1f),Random.Range(wind.y*.9f,wind.y*1.1f)));
		}
		if (m_rb != null) {
			m_rb.AddForce (wind);
		}
	}

	public static void freezeAnimation (){
		s_instance.m_animator.speed = 0.0f;
		s_instance.m_animator.SetBool ("Alive", false);
	}

	private IEnumerator BringToLife(){
		m_animator.SetBool ("Alive", false);
		while (
			m_newMutex == false) {
			yield return new WaitForEndOfFrame ();
		}
		m_animator.speed = 0.0f;
		while (true) {
			if (ms_life < 0.01f) {
				ms_life = 0.01f;
			}

			m_animator.speed = ((SceneZeroListener.s_smileMultiplier)/2.0f ) * m_animationSpeedModifier;

			Debug.Log (m_animator.speed);

			yield return new WaitForEndOfFrame ();
		}
	}

	public static void TryToAddWind(){

		if (interactible) {
			if(SceneZeroListener.IsEmotionalIntensitySufficient()){
				Debug.Log ("266: Trying to add wind");
				s_instance.StartCoroutine (s_instance.AddWind ());
			} else {
				s_instance.AddDownDraft ();
			}
		}
	}

	public IEnumerator ToggleAliveCoroutine(){
		yield return new WaitForEndOfFrame ();
		Debug.Log ("Petal is now alive");
		m_alive = true;
		interactible = true;
		Unlock ();
		s_instance.StopCoroutine (s_instance.m_bringToLife);
		DialogueManager.Main.FadeOut ();
		yield return new WaitForSeconds (3.0f);


		if (s_instance.m_goodEnding == false) {
			//Handle dialogue once the petal comes to life
			DialogueManager.Main.DisplayScene0Text (Scene0Events.OnDeadPetal);
		}
	}


	public static void ToggleAlive(){
		s_instance.StartCoroutine (s_instance.ToggleAliveCoroutine ());
	}

	// Update is called once per frame
	void Update () {

		transform.localScale = Vector3.Lerp (Vector3.one * c_startSize, Vector3.one * c_endSize, transform.position.y / c_resizeDistance);

		float targetDistance = float.PositiveInfinity;
		if (m_nextTarget != null) {
			targetDistance = Vector3.Distance (m_nextTarget.m_transform.position, transform.position);
		}
		if (targetDistance < 10.0f && m_nextTarget.m_nextPetalWaypoint == null && notLerping) {
			GameObject prevTarget = m_nextTarget.gameObject;
			m_nextTarget = m_nextTarget.m_nextPetalWaypoint;
			Destroy (prevTarget);

			Debug.Log ("Beginning Lerp to stem");
			StartCoroutine (LerpToPosition ());
			notLerping = false;

			//Start final dialogue sequence
			StartCoroutine(ShowEndDialogue());

		} else if (targetDistance < 3.0f) {

			GameObject prevTarget = m_nextTarget.gameObject;
			m_nextTarget = m_nextTarget.m_nextPetalWaypoint;
			Destroy (prevTarget);
			Debug.Log ("Targt changed");

			//Determine which lines of dialogue to show as the petal is rising
			if (m_alive)
			{
				ShowMiddleDialogue(m_nextTarget.m_waypointNumber);
			}


		} else {
			if (m_rb != null) {
				m_rb.velocity *= Mathf.Lerp (1.0f, .99f, 1 / targetDistance);
			}
		}
	}
	//Handles displaying the middle lines of dialogue as the petal is rising
	void ShowMiddleDialogue(int _waypointNumber)
	{
		if (m_goodEnding == false) {
			Debug.Log ("_waypointNumber" + _waypointNumber);
			switch (_waypointNumber) {//TODO
			case (3):
				StartCoroutine(SampleEmotionAndPlayDialog (Scene0Events.OnPetalRising));
				break;
			case (4):
				StartCoroutine(SampleEmotionAndPlayDialog (Scene0Events.OnPetalInTree));
				break;
			default:
				break;
			}
		}
	}

	private IEnumerator SampleEmotionAndPlayDialog(Scene0Events dialog){

		PetalMovement.FreezePetal ();
		ImageResultsListener.TakeSample ();
		yield return new WaitForSeconds (3.0f);
		bool waiting = true;
		DialogueManager.Emotion emotion = DialogueManager.Emotion.Joy;
		while (waiting) {
			if (DialogueManager.CanGetCurrentEmotion ()) {
				emotion = DialogueManager.GetCurrentEmotion ();
				waiting = false;
				DialogueManager.DisableCurrentEmotion ();
			}
			yield return new WaitForEndOfFrame ();
		}
		SetInstructionSprite.StopWaitingForEmotion ();
		DialogueManager.Main.FadeOut ();
		PetalMovement.FreezePetal ();
		yield return new WaitForSeconds (4.0f);
		PetalMovement.FreezePetal ();



		DialogueManager.Main.DisplayScene0Text (dialog, false,DialogueManager.GetCurrentEmotion());
		yield return new WaitForSeconds (6.0f);


		PetalMovement.UnfreezePetal ();
	}

	//Handles displaying the last two lines of dialogue at the end of the scene
	IEnumerator ShowEndDialogue()
	{
		bool waiting = true;
		DialogueManager.Emotion emotion = DialogueManager.Emotion.Joy;
		//TODO
		DialogueManager.Main.DisplayScene0Text (Scene0Events.PromptOnSceneEnd1, true, emotion);
		yield return new WaitForSeconds (3f);
		DialogueManager.Main.FadeOut ();
		yield return new WaitForSeconds (3f);

		PetalMovement.FreezePetal ();
		ImageResultsListener.TakeSample ();
		yield return new WaitForSeconds (3.0f);
		waiting = true;
		while (waiting) {
			if (DialogueManager.CanGetCurrentEmotion ()) {
				emotion = DialogueManager.GetCurrentEmotion ();
				waiting = false;
				DialogueManager.DisableCurrentEmotion ();
			}
			yield return new WaitForEndOfFrame ();
		}
		SetInstructionSprite.StopWaitingForEmotion ();
		DialogueManager.Main.FadeOut ();
		PetalMovement.FreezePetal ();
		yield return new WaitForSeconds (4.0f);
		PetalMovement.FreezePetal ();



		DialogueManager.Main.DisplayScene0Text (Scene0Events.OnSceneEnd2, false,DialogueManager.GetCurrentEmotion());
		yield return new WaitForSeconds (6.0f);

		FadeInFadeOut.FadeOut ();
	}
}
