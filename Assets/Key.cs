using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Key : MonoBehaviour {
	[SerializeField] private Text m_shadow;
	[SerializeField] private Text m_glow;
	[SerializeField] private Text m_dialog;

	private float m_fadeInRate;
	private float m_fadeOutRate;

	private float m_maxOpacity;

	private const float c_maxFade = 1.0f;
	private const float c_minFade = .1f;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update () {
	
	}

	public void setText(string text){
		m_shadow.text = text;
		m_glow.text = text;
		m_dialog.text = text;

		StartCoroutine (OscillateOpacityAndDestroy ());
	}

	private IEnumerator OscillateOpacityAndDestroy(){
		m_fadeInRate = Random.Range (c_minFade, c_maxFade);
		m_fadeOutRate = Random.Range (c_minFade, c_maxFade);
		m_maxOpacity = Random.Range (c_minFade, c_maxFade);

		Color transparent = new Color (0, 0, 0, 0);

		Color OpaqueShadow = new Color (m_shadow.color.r, m_shadow.color.g, m_shadow.color.b, m_maxOpacity);
		Color OpaqueGlow = new Color (m_glow.color.r, m_glow.color.g, m_glow.color.b, m_maxOpacity);
		Color OpaqueDialog = new Color (m_dialog.color.r, m_dialog.color.g, m_dialog.color.b, m_maxOpacity);

		float t = 0;
		//Fade in
		while (t < m_fadeInRate * Time.timeScale) {
			m_shadow.color = Color.Lerp (transparent, OpaqueShadow, t/(m_fadeInRate* Time.timeScale));
			m_glow.color = Color.Lerp (transparent, OpaqueGlow, t/(m_fadeInRate* Time.timeScale));
			m_dialog.color = Color.Lerp (transparent, OpaqueDialog, t/(m_fadeInRate* Time.timeScale));

			t += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		//pause

		//fade out
		t = 0;
		//Fade in
		while (t < m_fadeOutRate * Time.timeScale) {
			m_shadow.color = Color.Lerp (OpaqueShadow,transparent,  t/(m_fadeOutRate* Time.timeScale));
			m_glow.color = Color.Lerp (OpaqueGlow,transparent,  t/(m_fadeOutRate* Time.timeScale));
			m_dialog.color = Color.Lerp (OpaqueDialog,transparent,  t/(m_fadeOutRate* Time.timeScale));

			t += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		//destroy
		Destroy(this.gameObject);
	}
}
