using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Affdex;


public class RestuarantSceneScript : MonoBehaviour {
	[SerializeField]private Color m_invisibleColor;
	[SerializeField]private Color m_visibleTextColor;
	[SerializeField]private Color m_invisibleTextColor;



	private const float c_lerpTime = 4.0f;
	// Use this for initialization
	void Start () {
		StartCoroutine (LevelController ());
	}

	private IEnumerator LevelController(){
		//Introductory text

		//When I was with her, I stopped caring what other people thought.
		//Lerp in the text
		int textIndex = 0;
		float t = 0.0f;

        yield return new WaitForSeconds(1f);

        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        yield return new WaitForSeconds(2f);

		yield return DialogueManager.Main.DisplayScene2Text(Scene2Events.OnSceneLoad,true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();



        yield return DialogueManager.Main.DisplayScene2Text(Scene2Events.OnIntro,true);
      
        yield return DialogueManager.Main.FadeOutRoutine();
      
		FadeInFadeOut.FadeOut ();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
