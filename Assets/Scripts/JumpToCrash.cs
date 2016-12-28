using UnityEngine;
using System.Collections;

public class JumpToCrash : MonoBehaviour {
	float tCrash = 0;
	private static JumpToCrash m_instance;

	private bool m_showingCommand = false;
	private bool m_listening = false;
	private bool m_enabled = false;
	private bool m_lock =false;

	float tSinceFadeOut = 0.0f;

	private const float c_timeSwervingBeforeCrash = 10.0f;
	// Use this for initialization
	void Start () {
		m_instance = this;
		StartCoroutine(ShowDialogue());
	}

	// Update is called once per frame
	void Update () {
		if (VaryPositionByAttention.IsFocusing ()) {
			SetInstructionSprite.GrowIcon (Time.deltaTime);
		} else {

			SetInstructionSprite.ShrinkIcon (Time.deltaTime);
		}

		tSinceFadeOut += Time.deltaTime;

		if (m_enabled) {
			if (VaryPositionByAttention.IsSwerving () && m_showingCommand == false && tSinceFadeOut > 4.0f) {
				m_showingCommand = true;
				//SetInstructionSprite.SetWaitingForFaceIndicator (SetInstructionSprite.FaceState.FOCUS);

			} else if (m_showingCommand && !VaryPositionByAttention.IsSwerving ()) {
				tSinceFadeOut = 0.0f;
			}

			if (VaryPositionByAttention.IsSwerving ()) {
				tCrash += Time.deltaTime;
				if (tCrash > c_timeSwervingBeforeCrash) {
					Application.LoadLevel (7);
				}
			} 
		} else {
			if (VaryPositionByAttention.IsFocusing () && m_lock) {
				PlayerHasStartedFocusing ();
			} 
		}
	}

	private IEnumerator ShowDialogue()
    {
		yield return new WaitForEndOfFrame ();

		DialogueManager.Main.DisplayScene1Text (Scene1Events.OnCommandToPayAttention, true,DialogueManager.Emotion.Joy,DialogueManager.TextType.Command);
		SetInstructionSprite.SetWaitingForFaceIndicator (SetInstructionSprite.FaceState.FOCUS);
		yield return new WaitForSeconds (5.0f);
		m_lock = true;
    }

	public static void PlayerHasStartedFocusing(){
		if (m_instance.m_lock == true) {
			m_instance.m_listening = true;
			m_instance.m_enabled = true;
			m_instance.m_lock = false;
		}
	}
}
