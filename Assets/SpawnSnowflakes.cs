using UnityEngine;
using System.Collections;

public class SpawnSnowflakes : MonoBehaviour {
	public GameObject [] m_spawners;
	[SerializeField]private Transform m_parent;

	public static SpawnSnowflakes ms_instance;

	[SerializeField]private float c_spawnWaitTime = 1.0f;
	[SerializeField]private float c_spawnTime = .2f;

	[SerializeField]private float c_minSpawnInterval = .01f;
	[SerializeField]private float c_maxSpawnInterval = .03f;

	[SerializeField]private float m_minTorque = -.5f;
	[SerializeField]private float m_maxTorque = .5f;

	[SerializeField]private float m_minSize = .1f;
	[SerializeField]private float m_maxSize = 1.0f;

	[SerializeField]private int c_flakesNeededToProceed = 7;
	[SerializeField]private float c_snowflakeWaitTime = 6.0f;
	private static bool m_hyperSpawn;
	private static int ms_flakesSpawned = 0;

	private static bool m_firstFlakeSpawned = false;


	[SerializeField]private bool m_tutorial = false;
	[SerializeField]private GameObject m_flake;

	// Use this for initialization
	void Start () {
		if (m_tutorial) {
			StartCoroutine (SnowFlakeTutorial ());
		}
		ms_instance = this;
	}

	private IEnumerator SnowFlakeTutorial(){

		SetInstructionSprite.SetWaitingForFaceIndicator (SetInstructionSprite.FaceState.OPEN);
		DialogueManager.Main.DisplayScene2Text(Scene2Events.OnSnowflakeCommand, true,DialogueManager.Emotion.Joy,DialogueManager.TextType.Command);
		


		m_firstFlakeSpawned = false;
		while (m_firstFlakeSpawned == false) {
			yield return new WaitForEndOfFrame ();
		}

		while (VaryPositionByAttention.GetSnowFlakesSpawned () < c_flakesNeededToProceed) {
			
			yield return new WaitForEndOfFrame ();

		}
		DialogueManager.Main.FadeOut ();
		yield return new WaitForSeconds (4.0f);
		DialogueManager.Main.CenterDialog ();
		DialogueManager.Main.DisplayScene2Text(Scene2Events.OnSnowflakeCompletion, true,DialogueManager.Emotion.Joy,DialogueManager.TextType.Dialog);
		yield return new WaitForSeconds (c_snowflakeWaitTime);

		FadeInFadeOut.FadeOutToCrash ();
	}

	public static void StartSpawningSnowFlakes(){
		ms_instance.StartCoroutine (ms_instance.SpawnSnowFlakesCoroutine ());
	}

	public static void SetHyperSpawn(bool setTo){
		m_hyperSpawn = setTo;
	}

	public static void SpawnASnowflake(){
		ms_instance.StartCoroutine (ms_instance.SpawnASnowFlakeCoroutine ());

	}

	public static void SetBeatEffect(float t){
		ms_instance.StartCoroutine (ms_instance.BeatEffectCoroutine (t));
	}

	public IEnumerator BeatEffectCoroutine(float t){
		m_hyperSpawn = true;
		yield return new WaitForSeconds (t);
		m_hyperSpawn = false;

	}

	public IEnumerator SpawnASnowFlakeCoroutine(){

		SetInstructionSprite.ScaleUpPopInIndicator ();

		bool uberSpawn = m_hyperSpawn;

		yield return new WaitForSeconds (0.1f);
		if (uberSpawn == false) {
			uberSpawn = false;
		}



		float tPassed = 0.0f;

		tPassed += Time.deltaTime;
		m_firstFlakeSpawned = true;
		if (!uberSpawn) {
			int flakes = Random.Range (1, 2);
			for (int i = 0; i < flakes; i++) {
				//Spawn a flake at a random spawner
				GameObject flake = GameObject.Instantiate (m_flake);
				ms_flakesSpawned++;

				VaryPositionByAttention.AddSnowflake ();

				int flakeIndex = Random.Range (0, m_spawners.Length - 1);


				flake.transform.position = m_spawners [flakeIndex].transform.position;
				flake.transform.parent = m_parent;
				flake.GetComponent<Rigidbody2D> ().AddTorque (Random.Range (m_minTorque, m_maxTorque));
				flake.transform.localScale =Vector3.one * Random.Range (m_minSize, m_maxSize);
			}

		} else {
			Debug.LogWarning ("UBERSPAWN");
			int flakes = Random.Range (3, 4);
			for (int i = 0; i < flakes; i++) {
				//Spawn a flake at a random spawner
				GameObject flake = GameObject.Instantiate (m_flake);
				ms_flakesSpawned++;

				VaryPositionByAttention.AddSnowflake ();

				int flakeIndex = Random.Range (0, m_spawners.Length - 1);


				flake.transform.position = m_spawners [flakeIndex].transform.position;
				flake.transform.parent = m_parent;
				flake.GetComponent<Rigidbody2D> ().AddTorque (Random.Range (m_minTorque, m_maxTorque));
				flake.transform.localScale =Vector3.one * Random.Range (m_minSize, m_maxSize);
			}
		}
	}

	public IEnumerator SpawnSnowFlakesCoroutine(){
		yield return new WaitForSeconds (c_spawnWaitTime);

		float tPassed = 0.0f;

		while (tPassed < c_spawnTime) {
			tPassed += Time.deltaTime;

			//Spawn a flake at a random spawner
			GameObject flake = GameObject.Instantiate(m_flake);
			ms_flakesSpawned++;


			int flakeIndex = Random.Range (0, m_spawners.Length - 1);


			flake.transform.position = m_spawners [flakeIndex].transform.position;
			flake.transform.parent = m_parent;
			flake.GetComponent<Rigidbody2D> ().AddTorque (Random.Range (m_minTorque, m_maxTorque));
			flake.transform.localScale =Vector3.one * Random.Range (m_minSize, m_maxSize);
		

			yield return new WaitForSeconds (Random.Range (c_minSpawnInterval, c_maxSpawnInterval) * 10.0f);

		}
	}
}
