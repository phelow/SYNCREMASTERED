using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {
	protected float m_intensity = 0.0f;

	public void GiveIntensity(float f){
		m_intensity += f;
	}

}
