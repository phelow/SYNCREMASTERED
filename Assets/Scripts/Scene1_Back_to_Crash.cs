using UnityEngine;
using Affdex;
using System.Collections;

public class Scene1_Back_to_Crash : MonoBehaviour {
	//When this animation is finished, jump to the next scene
	//May replace with when a voice acting is over later
	private const float c_timeBeforeJump  = 11.0f;
	void Start () {
        //Display dialogue for this scene
        StartCoroutine(WaitForDialogue());
    }

    //Handle multiple lines of dialogue in this one scene, with a slight pause between them
    IEnumerator WaitForDialogue()
    {
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

        yield return DialogueManager.Main.DisplayScene1Text(Scene1Events.OnReverseCar,true);

        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
              
        
		yield return DialogueManager.Main.DisplayScene1Text(Scene1Events.OnTimeTransition,true);
		yield return new WaitForSeconds(4.0f);
		FadeInFadeOut.FadeOut ();
    }
}
