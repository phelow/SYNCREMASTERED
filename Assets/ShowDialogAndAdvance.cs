using UnityEngine;
using System.Collections;
using Affdex;

public class ShowDialogAndAdvance : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (AdvanceScene ());
	}
	
	private IEnumerator AdvanceScene(){
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
		DialogueManager.Main.FadeOut();
		DialogueManager.Main.DisplayScene2Text (Scene2Events.OnCarCrash);
		yield return new WaitForSeconds (6.0f);

		FadeInFadeOut.FadeOut ();
	}
}
