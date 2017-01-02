using UnityEngine;
using Affdex;
using System.Collections;

public class Scene400script : MonoBehaviour {
	[SerializeField]private Scene4Events m_line = Scene4Events.OnFirstJournal;

	// Use this for initialization
	void Start () {
		StartCoroutine (OneLiner ());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	private IEnumerator OneLiner(){
		yield return new WaitForSeconds(1.0f);
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
		DialogueManager.Main.FadeOut();
		yield return new WaitForSeconds(2.0f);


		DialogueManager.Main.DisplayScene4Text(m_line, true, emotion); //M_LINE, EMOTION
		yield return new WaitForSeconds (4.0f);
        DialogueManager.Main.FadeOut();
        yield return new WaitForSeconds(2f);
		FadeInFadeOut.FadeOut ();
	}
}
