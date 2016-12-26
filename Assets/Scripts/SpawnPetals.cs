using UnityEngine;
using System.Collections;

public class SpawnPetals : MonoBehaviour {
	public Rect m_spawningRect;

	[SerializeField]private float m_minSpawnTime = .1f;
	[SerializeField]private float m_maxSpawnTime = .4f;


	[SerializeField]private GameObject m_Petal;
	// Use this for initialization
	void Start () {
		StartCoroutine (RandomlySpawnPetals ());
	}

	private IEnumerator RandomlySpawnPetals(){
		while (true) {
			GameObject newPetal = GameObject.Instantiate (m_Petal);

			float startingX = transform.position.x - m_spawningRect.x;
			float startingY = transform.position.y + m_spawningRect.y;

			newPetal.transform.position = new Vector3 (Random.Range(startingX,
				startingX + m_spawningRect.width), Random.Range(startingY,
					startingY + m_spawningRect.height), transform.position.z);

			Rigidbody2D rb = newPetal.GetComponent<Rigidbody2D> ();

			rb.AddForce (new Vector2 (Random.Range(-1.0f,1.0f),Random.Range(-1.0f,1.0f)));
			rb.AddTorque (Random.Range (-10.0f, 10.0f));

			yield return new WaitForSeconds (Random.Range(m_minSpawnTime,m_maxSpawnTime));
		}
	}
}
