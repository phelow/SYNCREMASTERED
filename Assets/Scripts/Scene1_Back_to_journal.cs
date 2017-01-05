
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
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        yield return DialogueManager.Main.DisplayScene1Text(Scene1Events.OnShowCrashEnd, true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        
        yield return DialogueManager.Main.DisplayScene1Text(Scene1Events.OnSceneEnd, true);
        
        yield return DialogueManager.Main.FadeOutRoutine();
        
        FadeInFadeOut.FadeOut ();//Application.LoadLevel(5);
    }
}
