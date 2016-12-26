using UnityEngine;
using Affdex;
using System.Collections;

public class Scene1_Back_to_Crash : MonoBehaviour {
	//When this animation is finished, jump to the next scene
	//May replace with when a voice acting is over later
	private const float c_timeBeforeJump  = 11.0f;
	void Start () {
		StartCoroutine (Jump ());

        //Display dialogue for this scene
        ShowDialogue();
        
	}

	// Update is called once per frame
	void Update () {
	}
	private IEnumerator Jump(){
		yield return new WaitForSeconds (c_timeBeforeJump);

	}

    void ShowDialogue()
    {
        StartCoroutine(WaitForDialogue());
    }

    //Handle multiple lines of dialogue in this one scene, with a slight pause between them
    IEnumerator WaitForDialogue()
    {
        /*bool waiting = true;
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
		SetInstructionSprite.StopWaitingForEmotion ();*/

        yield return new WaitForSeconds(2f);

		DialogueManager.Main.DisplayScene1Text(Scene1Events.OnReverseCar,true);

        yield return new WaitForSeconds(2f);

		//yield return new WaitForSeconds(1.0f);
        /*waiting = true;
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
		SetInstructionSprite.StopWaitingForEmotion ();*/

        bool waiting = true;
        DialogueManager.Emotion emotion = DialogueManager.Emotion.Joy;
        ImageResultsListener.TakeSample();
        while (waiting)
        {
            if (DialogueManager.CanGetCurrentEmotion())
            {
                emotion = DialogueManager.GetCurrentEmotion();
                waiting = false;
                DialogueManager.DisableCurrentEmotion();
            }
            yield return new WaitForEndOfFrame();
        }
        SetInstructionSprite.StopWaitingForEmotion();


        
        DialogueManager.Main.FadeOut ();
		yield return new WaitForSeconds(3.0f);

		DialogueManager.Main.DisplayScene1Text(Scene1Events.OnTimeTransition,true,emotion);
		yield return new WaitForSeconds(4.0f);
		FadeInFadeOut.FadeOut ();//Application.LoadLevel(5);
    }
}
