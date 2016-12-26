using UnityEngine;
using System.Collections;

public class FollowPetal : MonoBehaviour {
	public Transform m_petalTransform;
	public Rigidbody2D m_petalRigidbody;

	public Rigidbody2D m_cameraRigidbody;

	private const float c_distanceThreshold = 1.0f;
	private const float c_panicThreshold = 2.0f;
	private const float s_cameraForceMultiplier = .1f;
	private const float c_lag = 2.0f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		//calculate distance between the camera and the petal
		float distance = Vector3.Distance(new Vector3(m_petalTransform.position.x,m_petalTransform.position.y,0), new Vector3(transform.position.x,transform.position.y,0));
		/*
		m_cameraRigidbody.velocity *= .95f;
		//if the distance exceeds a certain threshold, add a little force in the direction of the petal.


		if (distance > c_panicThreshold) {
			m_cameraRigidbody.velocity += m_petalRigidbody.velocity;
		}*/
		#if lerpImplementation
		if (distance > c_distanceThreshold) {
			if (m_cameraRigidbody != null) {
				transform.position = Vector3.Lerp (transform.position, new Vector3 (m_petalTransform.position.x, m_petalTransform.position.y, transform.position.z), Time.fixedDeltaTime * (Mathf.Pow (distance, 2.0f) - c_distanceThreshold + m_petalRigidbody.velocity.magnitude) / c_lag);
			} else {
				transform.position = Vector3.Lerp (transform.position, new Vector3 (m_petalTransform.position.x, m_petalTransform.position.y, transform.position.z), Time.fixedDeltaTime * (Mathf.Pow (distance, 2.0f) - c_distanceThreshold) / c_lag);
			}
		}
		#else

		if (distance > c_distanceThreshold) {
			m_cameraRigidbody.AddForce((m_petalTransform.position - transform.position) * distance);
		}
		#endif





	}
}
