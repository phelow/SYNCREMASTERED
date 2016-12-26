using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using Affdex;

public class Listener : ImageResultsListener
{ 

	public Text textArea;
	public override void setTextLost (){
		m_text.text = "Move your face into the camera";
	}
	public override void setTextFound(){
		m_text.text = "";
	}
    public override void onImageResults(Dictionary<int, Face> faces)
    {
		Debug.Log("Got results.");
        if(faces.Count > 0)
        {
            DebugFeatureViewer dfv = GameObject.FindObjectOfType<DebugFeatureViewer>();

            if (dfv != null)
                dfv.ShowFace(faces[0]);

            textArea.text = faces[0].ToString();
            textArea.CrossFadeColor(Color.white, 0.2f, true, false);
        }
        else
        {
            textArea.CrossFadeColor(new Color(1, 0.7f, 0.7f), 0.2f, true, false);
        }
    }

    
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}