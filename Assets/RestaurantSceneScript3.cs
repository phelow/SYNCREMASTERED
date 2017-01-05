using UnityEngine;
using System.Collections;
using Affdex;

public class RestaurantSceneScript3 : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (RestaurantScene3Script ());
	}
	private IEnumerator RestaurantScene3Script(){
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

		yield return DialogueManager.Main.DisplayScene2Text(Scene2Events.OnWalkingOutside,true);
        
        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds(2.0f);
		FadeInFadeOut.FadeOut ();
	}
}
