using UnityEngine;
using System.Collections;

public class KickWindShieldWiper : MonoBehaviour {

	public GameObject m_audioBeat;

	public GameObject gkick;

	public Material matOn;
	public Material matBlue;
	public Material matOff;

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
		if (gkick != null) {
			StopCoroutine (m_flash);
			m_flash = flashRed (gkick);
			StartCoroutine (m_flash);
		}
	}

	private IEnumerator flashRed(GameObject objeto)
	{
		SpawnSnowflakes.SetHyperSpawn (true);
		//objeto.GetComponent<Renderer>().material = matOn;
		yield return new WaitForSeconds(0.05f * difficulty);
		//objeto.GetComponent<Renderer>().material = matOff;
		SpawnSnowflakes.SetHyperSpawn (false);
		yield break;
	}

	private IEnumerator flashBlue(GameObject objeto)
	{
		objeto.GetComponent<Renderer> ().material = matBlue;
		yield return new WaitForSeconds(0.05f * difficulty);
		objeto.GetComponent<Renderer>().material = matOff;
		yield break;
	}

	// Use this for initialization
	void Start () {
		//Register the beat callback function
		m_audioBeat = Camera.main.gameObject;
		GetComponent<BeatDetection>().CallBackFunction = MyCallbackEventHandler;
		if (gkick != null) {
			m_flash = flashRed (gkick);
		}
	}

	void OnLevelWasLoaded(int level){
		m_audioBeat = Camera.main.gameObject;
		GetComponent<BeatDetection>().CallBackFunction = MyCallbackEventHandler;
		gkick = GameObject.FindGameObjectWithTag ("kickTag");
		if (gkick != null) {
			m_flash = flashRed (gkick);
		}
	}
}
