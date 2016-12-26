using UnityEngine;
using System.Collections;

public class KickHeartBeat : MonoBehaviour {
	public GameObject AudioBeat;

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
		PetalMovement.TryToAddWind ();

		BeatEffect.BeatInput ();
		StopCoroutine (m_flash);
		m_flash = flashRed (gkick);
		StartCoroutine (m_flash);
		Line2D.HeartBeat.MakeHeartMonitorBeat ();
	}

	private IEnumerator flashRed(GameObject objeto)
	{
		#if flashred
		objeto.GetComponent<Renderer>().material = matOn;
		#endif
		yield return new WaitForSeconds(0.05f * difficulty);
		#if flashred
		objeto.GetComponent<Renderer>().material = matOff;
		#endif
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
		AudioBeat.GetComponent<BeatDetection>().CallBackFunction = MyCallbackEventHandler;
		m_flash = flashRed (gkick);
	}
}
