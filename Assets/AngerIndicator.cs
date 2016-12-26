using UnityEngine;
using System.Collections;

public class AngerIndicator : Indicator {
	[SerializeField]private ParticleSystem m_ps;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		m_ps.Emit (((int) (m_intensity * Time.deltaTime * .01f)));
		m_intensity *= (1.0f - Time.deltaTime);
	}
}
