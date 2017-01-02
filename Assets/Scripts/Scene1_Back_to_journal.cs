
using UnityEngine;
using System.Collections;
using Affdex;

public class Scene1_Back_to_journal: MonoBehaviour {
	//When this animation is finished, jump to the next scene
	//May replace with when a voice acting is over later
	private const float c_timeBeforeJump  = 10.0f;
	void Start () {
        ShowDialogue();
	}

	// Update is called once per frame
	void Update () {
	}

    //Handles displaying the dialogue for this scene
    void ShowDialogue()
    {
        StartCoroutine(WaitForDialogue());

        
    }

    //Handle multiple lines of dialogue in this one scene, with a slight pause between them
    IEnumerator WaitForDialogue()
    {
        yield return new WaitForSeconds(2f);
        DialogueManager.Main.DisplayScene1Text(Scene1Events.OnShowCrashEnd, true, DialogueManager.Emotion.Joy);
        yield return new WaitForSeconds(2.0f);


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

        yield return new WaitForSeconds(2f);

        DialogueManager.Main.DisplayScene1Text(Scene1Events.OnSceneEnd, true, emotion);

        yield return new WaitForSeconds(3f);
        DialogueManager.Main.FadeOut();
        yield return new WaitForSeconds(1f);
        FadeInFadeOut.FadeOut ();//Application.LoadLevel(5);
    }
}
