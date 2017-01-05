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

        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();
        
		yield return DialogueManager.Main.DisplayScene4Text(m_line, true); //M_LINE, EMOTION
		yield return new WaitForSeconds (4.0f);
		FadeInFadeOut.FadeOut ();
	}
}
