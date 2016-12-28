using UnityEngine;
using System.Collections;

public class JumpTimer : MonoBehaviour {
	[SerializeField]private float waitTime = 7;
	
	// Use this for initialization
	void Start () {
        StartCoroutine(DialogueForThisScene());
	}

    private IEnumerator DialogueForThisScene()
    {
        DialogueManager.Main.DisplayScene1Text(Scene1Events.OnCrash);
        yield return new WaitForSeconds(waitTime);
        FadeInFadeOut.FadeOut();
    }
    
}
