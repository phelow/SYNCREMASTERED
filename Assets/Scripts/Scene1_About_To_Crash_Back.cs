using UnityEngine;
using System.Collections;
using Affdex;


public class Scene1_About_To_Crash_Back : MonoBehaviour {

	//When this animation is finished, jump to the next scene
	//May replace with when a voice acting is over later
	private const float c_timeBeforeJump  = 11.0f;

    void Start () {
        //Display text for this scene
        StartCoroutine(DelayDialogue());
    }
	
    IEnumerator DelayDialogue()
    {
        yield return new WaitForSeconds(2.0f);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

        yield return DialogueManager.Main.DisplayScene1Text(Scene1Events.OnShowCrashBeginning,true);
        yield return new WaitForSeconds(1f);

        FadeInFadeOut.FadeOut ();
    }
}
	
