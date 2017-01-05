using UnityEngine;
using System.Collections;

public class BadEndingSceneDriver : MonoBehaviour {

	// Use this for initialization
	void Start () {
		StartCoroutine (BadEndingSceneDialog ());
	}

	private IEnumerator BadEndingSceneDialog(){
		yield return DialogueManager.Main.DisplayBadEndingText (BadEndingEvents.OnRewind1);
		yield return new WaitForSeconds (10.0f);
		yield return DialogueManager.Main.DisplayBadEndingText (BadEndingEvents.OnRewind2);
		yield return new WaitForSeconds (20.0f);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
