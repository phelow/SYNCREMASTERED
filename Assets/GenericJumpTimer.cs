using UnityEngine;
using System.Collections;

public class GenericJumpTimer : MonoBehaviour {
	[SerializeField]private float time1 = 7;
	private bool hasFaded = true;


	// Update is called once per frame
	void Update () {
		if (time1 > 0) {
			time1 = time1 - Time.deltaTime;
		} else {
			if (hasFaded == true) {
				hasFaded = false;
				FadeInFadeOut.FadeOut ();
			}
		}
	}

}
