using UnityEngine;
using System.Collections;
using Affdex;

public class Scene26Dialog : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (playDialog ());
	}

	private IEnumerator playDialog(){
        yield return new WaitForSeconds(5.0f);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
		yield return DialogueManager.Main.DisplayScene2Text(Scene2Events.OnCarCrash,false);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
		yield return DialogueManager.Main.DisplayScene2Text(Scene2Events.OnEndScene,true);
		

        FadeInFadeOut.FadeOut ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
