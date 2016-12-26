using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InterpolateOpacity : MonoBehaviour {
	[SerializeField]private Image m_lightImage;

	private Color m_fadedIn = new Color (1.0f, 1.0f, 1.0f, 1.0f);
	private Color m_fadedOut = new Color (1.0f, 1.0f, 1.0f, 0.0f);

	[SerializeField]private float m_minTime = .5f;
	[SerializeField]private float m_maxTime = 1.0f;

	[SerializeField]private float m_minLerp = 0.0f;
	[SerializeField]private float m_maxLerp = 1.0f;

	// Use this for initialization
	void Start () {
		StartCoroutine (InterpolateColor ());
	}

	private IEnumerator InterpolateColor(){
		while (true) {
			//Lerp in
			float t = 0.0f;

			float lerpTime = Random.Range (m_minTime, m_maxTime);
			float lerpAmount = Random.Range (m_minLerp, m_maxLerp);

			while (t < lerpTime) {
				m_lightImage.color = Color.Lerp (m_lightImage.color, m_fadedIn, (Time.deltaTime / lerpTime) * lerpAmount);
				t += Time.deltaTime;

				yield return new WaitForEndOfFrame ();
			}

			//Lerp out
			t = 0.0f;

			lerpTime = Random.Range (m_minTime, m_maxTime);
			lerpAmount = Random.Range (m_minLerp, m_maxLerp);

			while (t < lerpTime) {
				m_lightImage.color = Color.Lerp (m_lightImage.color, m_fadedOut, (Time.deltaTime / lerpTime) * lerpAmount);
				t += Time.deltaTime;

				yield return new WaitForEndOfFrame ();
			}
		}
	}
}
