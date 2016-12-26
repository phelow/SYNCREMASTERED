using UnityEngine;
using System.Collections;

public class KickSnowFlakes : MonoBehaviour {

	public GameObject m_audioBeat;

	public bool m_energyTrigger;
	public bool m_hitHatTrigger;
	public bool m_kickTrigger;
	public bool m_snareTrigger;

	private IEnumerator m_flash;

	public float difficulty = 1.0f;

	public void MyCallbackEventHandler(BeatDetection.EventInfo eventInfo)
	{
		switch(eventInfo.messageInfo)
		{
		case BeatDetection.EventType.Energy:
			if (m_energyTrigger) {
				OnBeat ();
			}
			break;
		case BeatDetection.EventType.HitHat:
			if (m_hitHatTrigger) {
				OnBeat ();
			}
			break;
		case BeatDetection.EventType.Kick:
			if (m_kickTrigger) {
				OnBeat ();
			}
			break;
		case BeatDetection.EventType.Snare:
			if (m_snareTrigger) {
				OnBeat ();
			}
			break;
		}
	}

	private void OnBeat(){

		BeatEffect.BeatInput ();
		ParticleManager.BeatInput ();
		SpawnSnowflakes.SetBeatEffect (.5f);


	}

	// Use this for initialization
	void Start () {
		//Register the beat callback function
		m_audioBeat = Camera.main.gameObject;
		GetComponent<BeatDetection>().CallBackFunction = MyCallbackEventHandler;
	}

	void OnLevelWasLoaded(int level){
		m_audioBeat = Camera.main.gameObject;
		GetComponent<BeatDetection>().CallBackFunction = MyCallbackEventHandler;
	}
}
