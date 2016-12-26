using UnityEngine;
using System.Collections;

public class TemporarySceneAdvancer : MonoBehaviour {
	[SerializeField] private float progresionTime = 1.0f;
	// Use this for initialization
	void Start () {
		StartCoroutine (DelayedAdvance ());
	}

	private IEnumerator DelayedAdvance(){
		yield return new WaitForSeconds (progresionTime);
		FadeInFadeOut.FadeOut ();

	}

}
