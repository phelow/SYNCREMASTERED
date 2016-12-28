using UnityEngine;
using Affdex;
using System.Collections;

public class DropOnSmile : MonoBehaviour {
	private static Rigidbody2D rb;
	private static bool m_readyToSmile = false;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		StartCoroutine (DialogForScene ());
	}

	private IEnumerator DialogForScene(){
		DialogueManager.Emotion em = DialogueManager.Emotion.Anger;
        
		rb.isKinematic = false;
		DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnTakePicture,false,em);
		yield return new WaitForSeconds (7.0f);
		DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnReturnToStart1,false, em);
		yield return new WaitForSeconds (7.0f);
		DialogueManager.Main.DisplayGoodEndingText (GoodEndingEvents.OnReturnToStart2,false,em);
		yield return new WaitForSeconds (7.0f);
		FadeInFadeOut.FadeOut();
	}

	// Update is called once per frame
	void Update () {
	
	}

	public static void SmileAtLover(){
		if (m_readyToSmile) {
		}
	
	}
}
