using UnityEngine;
using System.Collections;
using Affdex;

public class RestaurantSceneScript2 : MonoBehaviour {
	[SerializeField]private Sprite m_frown;
	[SerializeField]private Sprite m_smile;
	[SerializeField]private Sprite m_neutral;

	[SerializeField]private SpriteRenderer m_loverCharacter;

	// Use this for initialization
	void Start () {
		StartCoroutine (SceneTwo());
	}
	
	private IEnumerator SceneTwo(){
		yield return new WaitForSeconds(1.0f);
		bool waiting = true;
		DialogueManager.Emotion emotion = DialogueManager.Emotion.Joy;

		DialogueManager.Main.DisplayScene2Text(Scene2Events.OnFirstConversation1,true,emotion,DialogueManager.TextType.Dialog);

        yield return new WaitForSeconds(5f);

        DialogueManager.Main.FadeOut();

		//Wait for blink
		yield return new WaitForSeconds(3.0f);
		waiting = true;
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
		SetInstructionSprite.StopWaitingForEmotion ();

        yield return new WaitForSeconds(2f);


        DialogueManager.Main.DisplayScene2Text(Scene2Events.OnFirstConversation2, true, emotion);
        
		yield return new WaitForSeconds (5.0f);


		DialogueManager.Main.FadeOut();
		yield return new WaitForSeconds(3.0f);


		waiting = true;
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
		SetInstructionSprite.StopWaitingForEmotion ();
		//DialogueManager.Main.FadeOut ();
		yield return new WaitForSeconds (1.0f);

        switch (emotion)
        {
            case DialogueManager.Emotion.Joy:
                m_loverCharacter.sprite = m_smile;
                break;
            case DialogueManager.Emotion.Anger:
                m_loverCharacter.sprite = m_frown;
                break;
            case DialogueManager.Emotion.Surprise:
                m_loverCharacter.sprite = m_frown;
                break;
        }

        DialogueManager.Main.DisplayScene2Text(Scene2Events.OnFirstConversation3, true,emotion);

		yield return new WaitForSeconds(4.0f);
		DialogueManager.Main.FadeOut ();
		yield return new WaitForSeconds (4.0f);

		DialogueManager.Main.DisplayScene2Text(Scene2Events.OnSecondConversation1, true, emotion,DialogueManager.TextType.Dialog);
		
		yield return new WaitForSeconds(5.0f);
        DialogueManager.Main.FadeOut();
        yield return new WaitForSeconds(4f);

		waiting = true;
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
		SetInstructionSprite.StopWaitingForEmotion ();
		//DialogueManager.Main.FadeOut ();
		yield return new WaitForSeconds (3.0f);

		DialogueManager.Main.DisplayScene2Text(Scene2Events.OnSecondConversation2, true, emotion);

        yield return new WaitForSeconds(5f);

		/*yield return new WaitForSeconds(1.0f);
		waiting = true;
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
		DialogueManager.Main.FadeOut ();
		yield return new WaitForSeconds (2.0f);
        
        switch (emotion)
        {
            case DialogueManager.Emotion.Joy:
                m_loverCharacter.sprite = m_smile;
                break;
            case DialogueManager.Emotion.Anger:
                m_loverCharacter.sprite = m_neutral;
                break;
            case DialogueManager.Emotion.Surprise:
                m_loverCharacter.sprite = m_neutral;
                break;
        }
        SetInstructionSprite.StopWaitingForEmotion();
        yield return new WaitForSeconds(3.0f);

        DialogueManager.Main.DisplayScene2Text(Scene2Events.OnSecondConversation3,true,emotion);

		yield return new WaitForSeconds(5.0f);
		DialogueManager.Main.FadeOut ();
		yield return new WaitForSeconds (3.0f);

		DialogueManager.Main.DisplayScene2Text(Scene2Events.OnThirdConversation1,true,emotion,DialogueManager.TextType.Dialog);

        yield return new WaitForSeconds(5.0f);
        DialogueManager.Main.FadeOut();
        yield return new WaitForSeconds(3f);
        
		waiting = true;
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
		SetInstructionSprite.StopWaitingForEmotion ();
		//DialogueManager.Main.FadeOut ();
		yield return new WaitForSeconds (2.0f);
        
        switch (emotion)
        {
            case DialogueManager.Emotion.Joy:
                m_loverCharacter.sprite = m_smile;
                break;
            case DialogueManager.Emotion.Anger:
                m_loverCharacter.sprite = m_frown;
                break;
            case DialogueManager.Emotion.Surprise:
                m_loverCharacter.sprite = m_smile;
                break;
        }

        DialogueManager.Main.DisplayScene2Text(Scene2Events.OnThirdConversation2,true,emotion);

		/*yield return new WaitForSeconds(1.0f);
		waiting = true;
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
		
		yield return new WaitForSeconds (5.0f);

        DialogueManager.Main.FadeOut();

        yield return new WaitForSeconds(3.0f);

        DialogueManager.Main.DisplayScene2Text(Scene2Events.OnThirdConversation3,true,emotion, DialogueManager.TextType.Dialog);
		/*yield return new WaitForSeconds(1.0f);

		yield return new WaitForSeconds(1.0f);
		waiting = true;
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
		
		yield return new WaitForSeconds (5.0f);

        DialogueManager.Main.FadeOut();

        yield return new WaitForSeconds(2.0f);
        
        FadeInFadeOut.FadeOut ();
	}
}
