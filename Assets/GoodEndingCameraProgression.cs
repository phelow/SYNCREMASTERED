using UnityEngine;
using System.Collections;
using Affdex;

public class GoodEndingCameraProgression : MonoBehaviour {
	[SerializeField]private Animator m_cameraAnimator;
	[SerializeField]private Animator m_flashAnimator;
	// Use this for initialization
	void Awake(){

		m_cameraAnimator.speed = 0.0f;
		m_flashAnimator.speed = 0.0f;
		//m_cameraAnimator.Stop ();
	}


	void Start () {
		StartCoroutine (CameraDialogueSequence ());
	}

	private IEnumerator CameraDialogueSequence(){
		bool waiting = true;
        
		yield return DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.Cheese, false, DialogueManager.Emotion.Joy, true);
		yield return new WaitForSeconds (1.0f);
		

		while (DialogueManager.GetCurrentEmotion() != DialogueManager.Emotion.Joy) {
			yield return new WaitForEndOfFrame ();
            yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
			waiting = true;
		}
		DialogueManager.DisableCurrentEmotion ();
		SetInstructionSprite.StopWaitingForEmotion ();

       

        //DialogueManager.Main.FadeOut ();
		//m_cameraAnimator.StartPlayback ();
		m_cameraAnimator.speed = 0.5f;
		m_flashAnimator.speed = 0.5f;

        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        yield return DialogueManager.Main.DisplayGoodEndingText(GoodEndingEvents.OnTakePicture, false);

        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        yield return DialogueManager.Main.DisplayGoodEndingText(GoodEndingEvents.OnReturnToStart1, false);

        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        yield return DialogueManager.Main.DisplayGoodEndingText(GoodEndingEvents.OnReturnToStart2, false);

        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

        FadeInFadeOut.FadeOut (30);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
