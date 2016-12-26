using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SetOpacity : MonoBehaviour {
	[SerializeField]private Image m_target;
	[SerializeField]private Image m_selfImage;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		m_selfImage.color = m_target.color;
	}
}
