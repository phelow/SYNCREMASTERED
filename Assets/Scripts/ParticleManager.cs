using UnityEngine;
using System.Collections;

public class ParticleManager : MonoBehaviour {
	private static ParticleManager ms_instance;

	[SerializeField]private ParticleSystem m_systemOne;
	[SerializeField]private ParticleSystem m_systemTwo;

	private const int c_minParticles = 100;
	private const int c_maxParticles = 1000;

	// Use this for initialization
	void Start () {
		ms_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void BeatInput(){
		if (ms_instance != null) {
			ms_instance.m_systemOne.Emit (Random.Range (c_minParticles, c_maxParticles));
			ms_instance.m_systemTwo.Emit (Random.Range (c_minParticles, c_maxParticles));
		}
	}
}
