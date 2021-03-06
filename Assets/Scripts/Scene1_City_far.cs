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
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        

		yield return DialogueManager.Main.DisplayScene1Text(Scene1Events.OnSceneLoad, false);

		FadeInFadeOut.FadeOut ();
    }
}
