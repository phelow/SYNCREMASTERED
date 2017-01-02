using UnityEngine;
using System.Collections;
using Affdex;
public class Scene1_City_far : MonoBehaviour {

    //When this animation is finished, jump to the next scene
    //May replace with when a voice acting is over later
	private const float c_timeBeforeJump  = 10.0f;
    void Start () {
        //Display dialogue for this scene
        ShowDialogue();
    }

    void ShowDialogue()
    {
        StartCoroutine(DelayDialogue());
    }

    IEnumerator DelayDialogue()
    {
		SetInstructionSprite.StartWaitingForEmotion ();

		//Wait for player to emote
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


		DialogueManager.Main.DisplayScene1Text(Scene1Events.OnSceneLoad, false, emotion);
		yield return new WaitForSeconds(4f);

		FadeInFadeOut.FadeOut ();
    }
}
