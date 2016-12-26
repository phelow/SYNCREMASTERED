using UnityEngine;
using System.Collections;

public class ResizeMaskInstance : MonoBehaviour {

	private Vector3 m_baseSize;
	private Vector3 m_maxSize;
	private const float c_averageSpeed = .5f;
	private const float c_interpolationSpeed = 4.0f;
	private const float c_speedUpAmount = 1.01f;
	private const float c_maxSpeedForSizing = 1.4f;
	private static float s_speed = c_averageSpeed;
	private static bool s_shouldDestroy = false;

	private static bool lerpDown = true;

	// Use this for initialization
	void Start () {
		s_shouldDestroy = false;
		m_baseSize = Vector3.one * CursorMasking.ms_cursorSize;
		m_maxSize = Vector3.one * CursorMasking.ms_maxCursorSize;
		transform.localScale = Vector3.one * CursorMasking.ms_cursorSize;
	}

	public static void DestroyThis(){
		s_shouldDestroy = true;
	}

	public static void IncreaseSpeed(){
		s_speed *= c_speedUpAmount;
		lerpDown = false;
	}

	public void StartResizingMask(){
		StartCoroutine (ResizeMask ());
	}

	private IEnumerator ResizeMask(){

		while(true){
			float tPassed = 0;
			while (tPassed < CursorMasking.ms_mouseLerpTime) {
				tPassed += Time.deltaTime * s_speed;
				transform.localScale = Vector3.Slerp (m_baseSize, m_maxSize/Mathf.Min(s_speed,c_maxSpeedForSizing), tPassed);
				yield return new WaitForEndOfFrame ();
			}

			while (tPassed > 0) {
				tPassed -= Time.deltaTime* s_speed;
				transform.localScale = Vector3.Slerp (m_baseSize, m_maxSize/Mathf.Min(s_speed,c_maxSpeedForSizing), tPassed);
				yield return new WaitForEndOfFrame ();

			}

			if (s_shouldDestroy) {
				s_shouldDestroy = false;
				Destroy (this.gameObject);
			}
			yield return new WaitForEndOfFrame();
		}
	}
	// Update is called once per frame
	void Update () {
		if (lerpDown) {
			s_speed = Mathf.Lerp (s_speed, c_averageSpeed, Time.deltaTime * c_interpolationSpeed);
		} else {
			lerpDown = true;
		}
	}
}
