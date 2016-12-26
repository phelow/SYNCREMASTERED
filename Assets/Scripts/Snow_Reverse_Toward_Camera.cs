using UnityEngine;
using System.Collections;

public class Snow_Reverse_Toward_Camera : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        this.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - Time.deltaTime*3.0f);
	}
}
