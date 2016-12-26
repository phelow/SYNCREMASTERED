using UnityEngine;
using System.Collections;

public class SnowflakeMovement : MonoBehaviour {
	[SerializeField]private Rigidbody2D m_rigidbody;

	private const float c_minForce = 0.1f;
	private const float c_maxForce = 10.0f;

	private const float c_minWaitTime = 0.1f;
	private const float c_maxWaitTime = 1.0f;

	private const int c_minForceIterations = 1;
	private const int c_maxForceIterations = 5;

	private const float c_maxFinishSize = 1.0f;
	private const float c_minFinishSize = .01f;
	private const float c_minStartSize = .8f;
	private const float c_maxStartSize = 1.2f;

	private const float c_minResizeTime = .5f;
	private const float c_maxResizeTime = 4.0f;

	private Vector3 m_startSize;

	// Use this for initialization
	void Start () {
		m_startSize = transform.localScale;
		transform.localScale = m_startSize;
		StartCoroutine (SnowFlakeMovement ());
	}

	private IEnumerator SnowFlakeMovement(){
		//Let the flake stay on the windshield for a bit
		yield return new WaitForSeconds(Random.Range(c_minWaitTime,c_maxWaitTime));
		transform.parent = null;
		StartCoroutine (Resize ());

		int nForceApplications = Random.Range (c_minForceIterations, c_maxForceIterations);

		for (int i = 0; i < nForceApplications; i++) {
			m_rigidbody.AddTorque (Random.Range(-c_maxForce,c_maxForce));

			m_rigidbody.AddForce (new Vector2 (Random.Range (-c_maxForce, c_maxForce), Random.Range (-c_maxForce, c_maxForce)));
			yield return new WaitForSeconds(Random.Range(c_minWaitTime,c_maxWaitTime));
		}
		

		m_rigidbody.gravityScale = -1.0f;
	}

	private IEnumerator Resize(){
		float tPassed = 0.0f;
		Vector3 endSize = Vector3.one * Random.Range (c_minFinishSize, c_maxFinishSize);
		float resizeTime = Random.Range (c_minResizeTime,c_maxResizeTime);
		while (tPassed < resizeTime) {
			tPassed += Time.deltaTime;

			transform.localScale = Vector3.Lerp (m_startSize, endSize, tPassed / resizeTime);
			yield return new WaitForEndOfFrame ();
		}
	}

}
