using UnityEngine;
using System.Collections;

public class FluctuateAnimation : MonoBehaviour {
	public Animator anim;
	// Use this for initialization
	void Start () {
		StartCoroutine (Fluctuate ());
	}

	private IEnumerator Fluctuate(){
		while (true) {
			yield return new WaitForSeconds (Random.Range (.5f, 3.0f));
			//anim.speed = Random.Range(.7f,1.4f);
		}
	}
}
