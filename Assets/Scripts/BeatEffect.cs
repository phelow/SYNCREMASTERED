using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BeatEffect : MonoBehaviour {
	private static BeatEffect ms_instance;
	private const float c_zoomInTime = .1f;

	[SerializeField]private float c_maxCameraSize = 1.2f;
	[SerializeField]private float c_minCameraSize = 1.0f;


	[SerializeField]private float c_maxCameraSizeNonOrthographic = 5f;
	[SerializeField]private float c_minCameraSizeNonOrthographic = 4.8f;

	[SerializeField]private Camera m_camera;
	int l = 0;



	// Use this for initialization
	void Awake () {
		ms_instance = this;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	private IEnumerator LerpToZoom(){
		float lerpTime = 0.0f;
		m_camera.orthographicSize = c_maxCameraSize;
		while(lerpTime < c_zoomInTime){
			if (l < 4) {

				m_camera.orthographicSize = Mathf.Lerp (c_maxCameraSize, c_minCameraSize, lerpTime / c_zoomInTime);
				m_camera.fieldOfView = Mathf.Lerp (c_maxCameraSize, c_minCameraSize, lerpTime / c_zoomInTime);
			} else {
				m_camera.orthographicSize = Mathf.Lerp (c_maxCameraSizeNonOrthographic, c_minCameraSizeNonOrthographic, lerpTime / c_zoomInTime);
				m_camera.fieldOfView = Mathf.Lerp (c_maxCameraSizeNonOrthographic, c_minCameraSizeNonOrthographic, lerpTime / c_zoomInTime);
			}
			lerpTime += Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}

		while (lerpTime > 0.0f) {

			if (l < 4) {

				m_camera.fieldOfView = Mathf.Lerp ( c_minCameraSize,c_maxCameraSize, lerpTime / c_zoomInTime);
			} else {
				m_camera.fieldOfView = Mathf.Lerp (c_minCameraSizeNonOrthographic, c_maxCameraSizeNonOrthographic,  lerpTime / c_zoomInTime);
				m_camera.orthographicSize = Mathf.Lerp ( c_minCameraSizeNonOrthographic, c_maxCameraSizeNonOrthographic,lerpTime / c_zoomInTime);
				m_camera.fieldOfView = Mathf.Lerp (c_maxCameraSizeNonOrthographic, c_minCameraSizeNonOrthographic, lerpTime / c_zoomInTime);

			}
			lerpTime -= Time.deltaTime;
			yield return new WaitForEndOfFrame ();
		}
	}


	void OnLevelWasLoaded(int level){
		l++;
		Debug.LogWarning (l);
		if (m_camera == null) {
			m_camera = Camera.main;
		}
	}

	public static void BeatInput(){
		Debug.LogWarning ("BEAT INPUT CALLED");
		if (ms_instance != null) {
			ms_instance.StartCoroutine (ms_instance.LerpToZoom ());
		}
	}
}
