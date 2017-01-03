using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Affdex;


public class RestuarantSceneScript : MonoBehaviour {
	[SerializeField]private Text [] m_textBoxes;
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


#if InSceneDialog
		while (t <= c_lerpTime) {
			t += Time.deltaTime;
			m_textBoxes [textIndex].color = Color.Lerp(m_invisibleTextColor,m_visibleTextColor,t/c_lerpTime);
			yield return new WaitForEndOfFrame ();
		}
		
#else
        yield return new WaitForSeconds(1f);

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

        yield return new WaitForSeconds(2f);

		DialogueManager.Main.DisplayScene2Text(Scene2Events.OnSceneLoad,true,emotion);
		#endif
		
		yield return new WaitForSeconds(4.0f);
        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds(2.0f);

		waiting = true;
		emotion = DialogueManager.Emotion.Joy;
		
		while (waiting) {
			if (DialogueManager.CanGetCurrentEmotion ()) {
				emotion = DialogueManager.GetCurrentEmotion ();
				waiting = false;
				DialogueManager.DisableCurrentEmotion ();
			}
			yield return new WaitForEndOfFrame ();
		}
		SetInstructionSprite.StopWaitingForEmotion ();
		
		yield return new WaitForSeconds(2.0f);



		DialogueManager.Main.DisplayScene2Text(Scene2Events.OnIntro,true,emotion);
      
        yield return new WaitForSeconds(4.0f);
        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds(1.0f);

		FadeInFadeOut.FadeOut ();
	

	}

	// Update is called once per frame
	void Update () {
	
	}
}
