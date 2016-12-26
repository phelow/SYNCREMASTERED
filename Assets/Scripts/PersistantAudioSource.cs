using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PersistantAudioSource : MonoBehaviour {
	private static PersistantAudioSource ms_instance;
	[SerializeField]private List<int> m_allowedLevels;

	// Use this for initialization
	void Start () {
		ms_instance = this;
		DontDestroyOnLoad (this.gameObject);
	}
	
	// Update is called once per frame
	void OnLevelWasLoaded(int level){
		if (!m_allowedLevels.Contains (level)) {
			Destroy (this.gameObject);
		}
	}



	public static bool CheckToFadeOutAudio (int m_nextScene){
		try{
		if (!ms_instance.m_allowedLevels.Contains (m_nextScene)) {
				Debug.LogWarning(m_nextScene);
			ms_instance.gameObject.GetComponent<AudioSource> ().loop = false;
			return true;
		}

		}
		catch{

		}
		return false;
	}
}
