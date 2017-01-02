using UnityEngine;
using System.Collections;
using Affdex;

public class Scene26Dialog : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (playDialog ());
	}

	private IEnumerator playDialog(){
		yield return new WaitForSeconds(1.0f);
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
		DialogueManager.Main.FadeOut();
		yield return new WaitForSeconds(2.0f);


		DialogueManager.Main.DisplayScene2Text(Scene2Events.OnCarCrash,true,emotion);
		yield return new WaitForSeconds (3.0f);
        DialogueManager.Main.FadeOut();
        yield return new WaitForSeconds(2.0f);

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
		SetInstructionSprite.StopWaitingForEmotion ();
		//DialogueManager.Main.FadeOut();
		yield return new WaitForSeconds(2.0f);


		DialogueManager.Main.DisplayScene2Text(Scene2Events.OnEndScene,true,emotion);
		yield return new WaitForSeconds (4.0f);
        DialogueManager.Main.FadeOut();
        yield return new WaitForSeconds(1.0f);

        FadeInFadeOut.FadeOut ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
