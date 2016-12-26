using UnityEngine;
using Affdex;
using System.Collections;

public class DropOnSmile : MonoBehaviour {
	private static Rigidbody2D rb;
	private static bool m_readyToSmile = false;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		StartCoroutine (DialogForScene ());
	}

	private IEnumerator DialogForScene(){
		DialogueManager.Emotion em = DialogueChoiceTracker.GetMostExpressedEmotion ();

		/*
		m_readyToSmile = true;
		//Wait for a smile
		bool waiting = true;


		SetInstructionSprite.StartWaitingForEmotion ();
		DialogueManager.Emotion emotion = DialogueManager.Emotion.Anger;
		while (emotion != DialogueManager.Emotion.Joy) {
			yield return new WaitForEndOfFrame ();

			waiting = true;
			while (waiting) {
				if (DialogueManager.CanGetCurrentEmotion ()) {
					emotion = DialogueManager.GetCurrentEmotion ();
					waiting = false;

					if (emotion != DialogueManager.Emotion.Joy) {
						ImageResultsListener.TakeSample (true, true);
					}
					DialogueManager.DisableCurrentEmotion ();

					Debug.Log ("emotion:" + emotion);
					yield return new WaitForEndOfFrame ();
				}
				Debug.Log (120);
				yield return new WaitForEndOfFrame ();
			}
			Debug.Log ("emotion:" + emotion);
		}*/

		rb.isKinematic = false;
		DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnTakePicture,false,em);
		yield return new WaitForSeconds (7.0f);
		DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnReturnToStart1,false, em);
		yield return new WaitForSeconds (7.0f);
		DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnReturnToStart2,false,em);
		yield return new WaitForSeconds (7.0f);
		FadeInFadeOut.FadeOut();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public static void SmileAtLover(){
		if (m_readyToSmile) {
		}
	
	}
}
