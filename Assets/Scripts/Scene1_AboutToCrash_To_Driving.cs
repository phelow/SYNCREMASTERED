using UnityEngine;
using System.Collections;

public class Scene1_AboutToCrash_To_Driving : MonoBehaviour {
    //Jump to next Scene when finishing this animie
    public Animation Animation_For_Jump;
	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        if (!Animation_For_Jump.isPlaying)
        {
            Application.LoadLevel(6);
        }
	}
}
