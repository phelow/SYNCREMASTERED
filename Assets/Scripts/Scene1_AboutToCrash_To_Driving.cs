using UnityEngine;
using System.Collections;

public class Scene1_AboutToCrash_To_Driving : MonoBehaviour {
    //Jump to next Scene when finishing this animie
    public Animation Animation_For_Jump;
	// Use this for initialization
	void Start () {
        StartCoroutine(JumpOverTime());
	}
	
    private IEnumerator JumpOverTime()
    {
        while (Animation_For_Jump.isPlaying)
        {
            yield return new WaitForEndOfFrame();
        }
        FadeInFadeOut.FadeOut();

    }
    
}
