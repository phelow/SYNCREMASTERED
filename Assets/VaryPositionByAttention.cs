using UnityEngine;
using System.Collections;

public class VaryPositionByAttention : MonoBehaviour {
	private static VaryPositionByAttention ms_instance;

	public static float m_attention;
	private static int m_snowflakes;
	private float m_stability; //composite of accumulated snowflakes and attention values

	private const float c_maxPosition = 1.0f;
	private const float c_minSpeed = .1f;
	private const float c_maxSpeed = 1.0f;

	private const int m_minAttentionSamples = 5;
	private const int m_maxAttentionSamples = 8;

	[SerializeField]private float c_attentionMinimum = 90.0f;

	private Vector3 m_originalPos;
	// Use this for initialization
	void Start () {
		ms_instance = this;
		m_originalPos = transform.position;
		StartCoroutine (CalculateAverageAttention ());
		StartCoroutine (ChangePositionByAttention ());
	}

	public static int GetSnowFlakesSpawned(){
		return m_snowflakes;
	}


	private IEnumerator ChangePositionByAttention(){
		while (true) {
			//Determine speed and distance as a factor of attention
			float speed = Random.Range(c_minSpeed,c_maxSpeed) * Mathf.Lerp(10.0f,1.0f,(100-m_attention) + m_snowflakes);


			//pick a target
			Vector3 position = new Vector3(m_originalPos.x + Random.Range(-c_maxPosition,c_maxPosition) * Mathf.Lerp(0.1f,1.0f,((100-m_attention) + m_snowflakes)/100),m_originalPos.y,m_originalPos.z);

			//save current position
			Vector3 curPosition = transform.position;
			//lerp from current position to target
			float tPassed = 0.0f;
			while (tPassed < speed) {
				tPassed += Time.deltaTime;
				transform.position = Vector3.Lerp (curPosition, position, tPassed / speed);
				yield return new WaitForEndOfFrame ();
			}

			yield return new WaitForEndOfFrame ();
		}
	}

	public static bool IsSwerving(){
		return (100-m_attention) + (m_snowflakes/10) > ms_instance.c_attentionMinimum;
	}

	public static bool IsFocusing(){

		return (100-m_attention) < ms_instance.c_attentionMinimum;
	}

	private IEnumerator CalculateAverageAttention(){
		while (true) {
			int nSamples = Random.Range (m_minAttentionSamples, m_maxAttentionSamples);
			float lastAttentionValue = -1;
			float attentionSum = 0;
			for (int i = 0; i < nSamples; i++) {
				yield return new WaitForEndOfFrame ();
				lastAttentionValue = CarFaceListener.ms_attention;
				attentionSum += CarFaceListener.ms_attention;
			}
			m_attention = attentionSum / nSamples;
		}
	}

	public static void AddSnowflake(){
		m_snowflakes++;
	}
}
