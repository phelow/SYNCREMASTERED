using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CycleThroughBGs : MonoBehaviour {
	[SerializeField]private Sprite[] m_backgrounds;
	[SerializeField]private Image m_backgroundImage;

	[SerializeField]private float m_minWaitTime = .4f;
	[SerializeField]private float m_maxWaitTime = 1.0f;

	// Use this for initialization
	void Start () {
		StartCoroutine (CycleImages ());
	}

	private IEnumerator CycleImages(){
		while (true) {
			for(int i = 0; i < m_backgrounds.Length; i++){
				m_backgroundImage.sprite = m_backgrounds [i];
				yield return new WaitForSeconds (Random.Range(m_minWaitTime,m_maxWaitTime));
			}
		}
	}
}
