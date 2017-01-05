using UnityEngine;
using System.Collections;

public class JumpTimer : MonoBehaviour {
	[SerializeField]private float waitTime = 7;
	
	// Use this for initialization
	void Start () {
        StartCoroutine(DialogueForThisScene());
	}

    private IEnumerator DialogueForThisScene()
    {
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        yield return DialogueManager.Main.DisplayScene1Text(Scene1Events.OnCrash);
        FadeInFadeOut.FadeOut();
    }
    
}
