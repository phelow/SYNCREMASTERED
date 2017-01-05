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
        
        yield return DialogueManager.Main.DisplayScene2Text(Scene2Events.OnFirstConversation1);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

        
        yield return new WaitForSeconds(2f);

        yield return DialogueManager.Main.DisplayScene2Text(Scene2Events.OnFirstConversation2);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        
        
        switch (DialogueManager.GetCurrentEmotion())
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

        yield return DialogueManager.Main.DisplayScene2Text(Scene2Events.OnFirstConversation3, true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();


        yield return DialogueManager.Main.DisplayScene2Text(Scene2Events.OnSecondConversation1, true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        
		yield return DialogueManager.Main.DisplayScene2Text(Scene2Events.OnSecondConversation2, true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

        yield return DialogueManager.Main.DisplayScene2Text(Scene2Events.OnSecondConversation3,true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        
		yield return DialogueManager.Main.DisplayScene2Text(Scene2Events.OnThirdConversation1,true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        
        switch (DialogueManager.GetCurrentEmotion())
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

        yield return DialogueManager.Main.DisplayScene2Text(Scene2Events.OnThirdConversation2,true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

        yield return DialogueManager.Main.DisplayScene2Text(Scene2Events.OnThirdConversation3,true);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        
        FadeInFadeOut.FadeOut ();
	}
}
