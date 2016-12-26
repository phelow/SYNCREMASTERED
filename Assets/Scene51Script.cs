using UnityEngine;
using System.Collections;
using Affdex;

public class Scene51Script : MonoBehaviour {
	private const float c_dialogInBetweenWaitTime = 7.0f;
	// Use this for initialization
	void Start () {
		StartCoroutine (SceneDialog ());
	}

	private IEnumerator SceneDialog(){
		yield return new WaitForSeconds(1.0f);
		bool waiting = true;
		DialogueManager.Emotion emotion = DialogueManager.Emotion.Joy;
		ImageResultsListener.TakeSample ();
		Debug.Log (17);
		while (waiting) {
			if (DialogueManager.CanGetCurrentEmotion ()) {
				emotion = DialogueManager.GetCurrentEmotion ();
				waiting = false;
				DialogueManager.DisableCurrentEmotion ();
			}
			yield return new WaitForEndOfFrame ();
		}
		Debug.Log (26);
		SetInstructionSprite.StopWaitingForEmotion ();
		DialogueManager.Main.FadeOut();
		yield return new WaitForSeconds(2.0f);
		DialogueManager.Main.DisplayScene5Text (Scene5Events.OnSceneLoad,false,emotion,true);
		yield return new WaitForSeconds(3.0f);
        DialogueManager.Main.FadeOut();
        yield return new WaitForSeconds(1.0f);

        waiting = true;
		emotion = DialogueManager.Emotion.Joy;
		Debug.Log (34);
		ImageResultsListener.TakeSample ();
		while (waiting) {
			if (DialogueManager.CanGetCurrentEmotion ()) {
				emotion = DialogueManager.GetCurrentEmotion ();
				waiting = false;
				DialogueManager.DisableCurrentEmotion ();
			}
			yield return new WaitForEndOfFrame ();
		}
		Debug.Log (44);
		SetInstructionSprite.StopWaitingForEmotion ();
		
		yield return new WaitForSeconds(1.0f);
		DialogueManager.Main.DisplayScene5Text (Scene5Events.OnShowPark,false,emotion,true);
		yield return new WaitForSeconds(4.0f);
		/*waiting = true;
		emotion = DialogueManager.Emotion.Joy;
		Debug.Log (52);
		ImageResultsListener.TakeSample ();
		while (waiting) {
			if (DialogueManager.CanGetCurrentEmotion ()) {
				emotion = DialogueManager.GetCurrentEmotion ();
				waiting = false;
				DialogueManager.DisableCurrentEmotion ();
			}
			yield return new WaitForEndOfFrame ();
		}
		Debug.Log (62);
		SetInstructionSprite.StopWaitingForEmotion ();*/
		yield return new WaitForSeconds(1.0f);
		FadeInFadeOut.FadeOut ();

	}
}
