using UnityEngine;
using System.Collections;

public class ProgressParticleSystem : MonoBehaviour {

	private const float c_modifier = 500.0f;
	private const float c_sadnessModifier = 10.0f;


	[SerializeField]private ParticleSystem m_angerParticleSystem;
	[SerializeField]private ParticleSystem m_joyParticleSystem;
	[SerializeField]private ParticleSystem m_sadnessParticleSystem;
	[SerializeField]private ParticleSystem m_surpriseParticleSystem;
	private static ProgressParticleSystem ms_instance;

	// Use this for initialization
	void Start () {
		ms_instance = this;

		GameObject mainCamera = Camera.main.gameObject;
		/*
		ParticleSystem ps = GetComponent<ParticleSystem>();
		var col = m_joyParticleSystem.colorOverLifetime;
		col.enabled = true;

		Gradient grad = new Gradient();
		grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.green, 0.9f), new GradientColorKey(Color.green, 0.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 1.0f),  new GradientAlphaKey(1.0f, 0.0f) } );

		col.color = new ParticleSystem.MinMaxGradient(grad);

		var col2 = m_angerParticleSystem.colorOverLifetime;
		col2.enabled = true;

		Gradient grad2 = new Gradient();
		grad2.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.red, 0.9f), new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.red, 0.0f) }, new GradientAlphaKey[] {new GradientAlphaKey(0.0f, 1.0f),  new GradientAlphaKey(1.0f, 0.0f) } );

		col2.color = new ParticleSystem.MinMaxGradient (grad2);

		var col3 = m_sadnessParticleSystem.colorOverLifetime;
		col3.enabled = true;

		Gradient grad3 = new Gradient();
		grad3.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.blue, 0.0f), new GradientColorKey(Color.blue, 0.9f) }, new GradientAlphaKey[] {new GradientAlphaKey(0.0f, 1.0f),  new GradientAlphaKey(1.0f, 0.0f)} );

		col3.color = new ParticleSystem.MinMaxGradient(grad3);

		var col4 = m_surpriseParticleSystem.colorOverLifetime;
		col4.enabled = true;

		Gradient grad4 = new Gradient();
		grad4.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.yellow, 0.9f), new GradientColorKey(Color.yellow, 0.9f), new GradientColorKey(Color.yellow, 0.9f) }, new GradientAlphaKey[] { new GradientAlphaKey(0.0f, 1.0f),  new GradientAlphaKey(1.0f, 0.0f)} );

		col4.color = new ParticleSystem.MinMaxGradient(grad4);


		m_angerParticleSystem.transform.parent = mainCamera.transform;
		m_angerParticleSystem.transform.localPosition = new Vector3 (0, 0, 10);


		m_joyParticleSystem.transform.parent = mainCamera.transform;
		m_joyParticleSystem.transform.localPosition = new Vector3 (0, 0, 10);


		m_sadnessParticleSystem.transform.parent = mainCamera.transform;
		m_sadnessParticleSystem.transform.localPosition = new Vector3 (0, 0, 10);


		m_surpriseParticleSystem.transform.parent = mainCamera.transform;
		m_surpriseParticleSystem.transform.localPosition = new Vector3 (0, 0, 10);

		SetEmotions (0, 0, 0, 0);*/
	}
	
	public static void SetEmotions(float anger, float joy, float sadness, float surpise){
		/*
		ms_instance.m_angerParticleSystem.emissionRate = c_modifier * anger/100.0f;
		ms_instance.m_joyParticleSystem.emissionRate = c_modifier*joy/100.0f;
		ms_instance.m_sadnessParticleSystem.emissionRate = c_modifier* c_sadnessModifier*sadness/1000.0f;
		ms_instance.m_surpriseParticleSystem.emissionRate = c_modifier* surpise/100.0f;*/

	}

}
