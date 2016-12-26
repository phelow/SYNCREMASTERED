using UnityEngine;
using System.Collections;

public class CursorMasking : MonoBehaviour {
	public const float ms_cursorSize = .1f;
	public const float ms_maxCursorSize = 2.5f;
	public const float ms_maxCursorExpansion = .1f;
	public const float ms_mouseLerpTime = 0.2f;

	private Vector3 m_baseSize;
	private Vector3 m_maxSize;

	[SerializeField]
	private GameObject m_mask;

	public bool m_toDestroy;

	private GameObject m_lastMask;


	void Start ()
	{
		m_baseSize = Vector3.one * ms_cursorSize;
		m_maxSize = Vector3.one * ms_maxCursorSize;
		transform.localScale = Vector3.one * ms_cursorSize;
	}

	public void ResizeMouse(Vector3 position){
		StartCoroutine (ResizeMouseCoroutine (position));
	}

	private IEnumerator ResizeMouseCoroutine(Vector3 position){
		while (m_lastMask != null) {
			//accelerate all current existing masks
			ResizeMaskInstance.IncreaseSpeed();
			ResizeMaskInstance.DestroyThis ();
			yield return new WaitForEndOfFrame ();
		}

		m_lastMask = GameObject.Instantiate (m_mask);

		m_lastMask.transform.position = position;
		m_lastMask.transform.parent = transform.parent;

		ResizeMaskInstance rmi = m_lastMask.GetComponent<ResizeMaskInstance> ();

		rmi.StartResizingMask ();
	}

	void Update ()
	{
		if (Input.GetMouseButtonDown (0)) {
			Vector3 position = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			transform.position = position;
			ResizeMouse(position);
		}

		if (m_toDestroy) {
			ResizeMaskInstance.DestroyThis ();
		}

	}
}
