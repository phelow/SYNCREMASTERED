using UnityEngine;
using System.Collections;
using Affdex;

public class GoodEndingCameraProgression : MonoBehaviour {
	[SerializeField]private Animator m_cameraAnimator;
	[SerializeField]private Animator m_flashAnimator;
	// Use this for initialization
	void Awake(){

		m_cameraAnimator.speed = 0.0f;
		m_flashAnimator.speed = 0.0f;
		//m_cameraAnimator.Stop ();
	}


	void Start () {
		StartCoroutine (CameraDialogueSequence ());
	}

	private IEnumerator CameraDialogueSequence(){
		bool waiting = true;

		DialogueManager.Emotion emotion = DialogueManager.Emotion.Anger;

		DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.Cheese, false, DialogueManager.Emotion.Joy, true);
		yield return new WaitForSeconds (1.0f);
		
		while (emotion != DialogueManager.Emotion.Joy) {
			yield return new WaitForEndOfFrame ();

			waiting = true;
			while (waiting) {
				if (DialogueManager.CanGetCurrentEmotion ()) {
					emotion = DialogueManager.GetCurrentEmotion ();
					waiting = false;
                    
					DialogueManager.DisableCurrentEmotion ();

					yield return new WaitForEndOfFrame ();
				}
				Debug.Log (120);
				yield return new WaitForEndOfFrame ();
			}
			Debug.Log ("emotion:" + emotion);
		}
		DialogueManager.DisableCurrentEmotion ();
		SetInstructionSprite.StopWaitingForEmotion ();

       

        //DialogueManager.Main.FadeOut ();
		//m_cameraAnimator.StartPlayback ();
		m_cameraAnimator.speed = 0.5f;
		m_flashAnimator.speed = 0.5f;

		yield return new WaitForSeconds (6.0f);

        DialogueManager.Main.DisplayGoodEndingText(GoodEndingEvents.OnTakePicture, false, emotion);

        yield return new WaitForSeconds(4.0f);

        DialogueManager.Main.FadeOut();

        yield return new WaitForSeconds(3.0f);

        DialogueManager.Main.DisplayGoodEndingText(GoodEndingEvents.OnReturnToStart1, false, emotion);

        yield return new WaitForSeconds(4.0f);

        DialogueManager.Main.FadeOut();

        yield return new WaitForSeconds(3.0f);

        DialogueManager.Main.DisplayGoodEndingText(GoodEndingEvents.OnReturnToStart2, false, emotion);

        yield return new WaitForSeconds(4.0f);

        DialogueManager.Main.FadeOut();

        yield return new WaitForSeconds(2.0f);

        FadeInFadeOut.FadeOut ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
