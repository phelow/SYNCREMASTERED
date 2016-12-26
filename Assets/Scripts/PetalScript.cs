using UnityEngine;
using System.Collections;

public class PetalScript : MonoBehaviour {
	private const float maxHeight = 100.0f;
	// Use this for initialization
	void Start () {
		StartCoroutine (KillWhenToHigh ());
	}

	private IEnumerator KillWhenToHigh(){
		while (true) {
			if (transform.position.y > maxHeight) {
				Destroy (gameObject);
			}

			yield return new WaitForEndOfFrame ();
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
