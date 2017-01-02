using UnityEngine;
using System.Collections;
using Affdex;

public class RestaurantSceneScript3 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (RestaurantScene3Script ());
	}
	private IEnumerator RestaurantScene3Script(){
		yield return new WaitForSeconds (1.0f);
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
		//DialogueManager.Main.FadeOut();
		//yield return new WaitForSeconds(3.0f);
		/*DialogueManager.Main.DisplayScene2Text(Scene2Events.OnThirdConversation3,true,emotion,DialogueManager.TextType.Dialog);
		yield return new WaitForSeconds(1.0f);
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
		DialogueManager.Main.FadeOut();*/
		yield return new WaitForSeconds(2.0f);


		DialogueManager.Main.DisplayScene2Text(Scene2Events.OnWalkingOutside,true,emotion);

        /*yield return new WaitForSeconds(1.0f);
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
		SetInstructionSprite.StopWaitingForEmotion ();*/

        yield return new WaitForSeconds(4f);

		DialogueManager.Main.FadeOut();
		yield return new WaitForSeconds(2.0f);
		FadeInFadeOut.FadeOut ();
	}
}
